using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.Global;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;
using ue_mes_entities.Enums;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global.Interface;

namespace ue_mes_logic.dbo
{
    public class PartLogic : LogicBase
    {
        IBillOfMaterialData BillOfMaterialData { get; }
        IEquipmentData EquipmentData { get; }
        IPartData PartData { get; }
        IPartItemAttributeTypeData PartItemAttributeTypeData { get; }

        #region Constructor

        public PartLogic(IBillOfMaterialData billOfMaterialData,
            IEquipmentData equipmentData, 
            IPartData partData,
            IPartItemAttributeTypeData partItemAttributeTypeData)
        {
            BillOfMaterialData = billOfMaterialData ?? throw new ArgumentNullException(nameof(billOfMaterialData));
            EquipmentData = equipmentData ?? throw new ArgumentNullException(nameof(equipmentData));
            PartData = partData ?? throw new ArgumentNullException(nameof(partData));
            PartItemAttributeTypeData = partItemAttributeTypeData ?? throw new ArgumentNullException(nameof(partItemAttributeTypeData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return all parts and include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<Part> GetAllParts(bool includeInactive = false)
        {
            return PartData.GetAllParts(includeInactive);
        }

        /// <summary>
        /// Returns a list of <see cref="Part"/> entities with part child entities included.
        /// The starting equipmant name is used to find BOPs where the process sequence is 1 and matches the equipment process.  This indicates an equipment where an order item unit can be started.
        /// </summary>
        /// <param name="equipmentName">Unique name for the <see cref="Equipment"/> to search</param>
        /// <returns></returns>
        public List<Part> GetPartsForBillOfProcessAtStartingEquipment(string equipmentName)
        {
                var equipmentToSearch = EquipmentData.GetEquipmentByName(equipmentName);
                if (equipmentToSearch == null)
                    return null;

                return PartData.GetPartsForBillOfProcessAtStartingEquipmentProcess(equipmentToSearch.Process.ProcessId);
        }


        /// <summary>
        /// Returns a list of <see cref="Part"/> entities with part child entities included.
        /// The sub assembly part can be consumed in one to many finished good parts.  Since a FG part is needed to identify an order item, we will return all possible parts that could consume this subassembly
        /// </summary>
        /// <param name="subAssemblyPart">Subassembly <see cref="Part"/> to search</param>
        /// <returns></returns>
        public List<Part> GetConsumingFinishedGoodPartsForSubAssemblyPart(Part subAssemblyPart)
        {
            // We need to do some recursion here.  We will extract BillOfMaterials that consume the subassembly part until the BOM is no longer a sub assembly.
            // In a real world scenario, this should never be more than 5 levels of BOM, so the processing should be low.
            // All extracted BOM finished good parts will be returned to the caller.

            List<Part> finishedGoodConsumingParts = new List<Part>();

            Queue<Part> subAssemblyPartsToCheck = new Queue<Part>();
            subAssemblyPartsToCheck.Enqueue(subAssemblyPart);

            while (subAssemblyPartsToCheck.Count > 0)
            {
                var subAssemblyPartToCheck = subAssemblyPartsToCheck.Dequeue();
                var consumingBillOfMaterials = BillOfMaterialData.GetBillOfMaterialsThatConsumePartId(subAssemblyPartToCheck.PartId);
                consumingBillOfMaterials.ForEach(billOfMaterial =>
                {
                    // if the part is a finished good, then we add it to the final result.  
                    if (billOfMaterial.Part.PartType == PartTypeEnum.FinishedGood && !finishedGoodConsumingParts.Any(finishedGoodConsumingPart => finishedGoodConsumingPart.PartId == billOfMaterial.Part.PartId))
                        finishedGoodConsumingParts.Add(billOfMaterial.Part);

                    if (billOfMaterial.Part.PartType == PartTypeEnum.SubAssembly && !subAssemblyPartsToCheck.Any(subAssemblyPartToCheck => subAssemblyPartToCheck.PartId == billOfMaterial.Part.PartId))
                        subAssemblyPartsToCheck.Enqueue(billOfMaterial.Part);
                });
            }

            return finishedGoodConsumingParts;
        }

        /// <summary>
        /// Return all parts where the part type matches a type in the partTypes list.
        /// Also, include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="partTypes">List of <see cref="PartTypeEnum"/> to return</param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<Part> GetPartsByPartTypes(List<PartTypeEnum> partTypes, bool includeInactive = false)
        {
            return PartData.GetPartsByPartTypes(partTypes, includeInactive);
        }

        /// <summary>
        /// Return a part by part number and revision
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="partRevision"></param>
        /// <returns></returns>
        public Part GetPartByPartNumberAndRevision(string partNumber, string partRevision)
        {
            return PartData.GetPartByPartNumberAndRevision(partNumber, partRevision);
        }

        /// <summary>
        /// Return a part by unique part ID
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public Part GetPartByPartId(int partId)
        {
            return PartData.GetPartEntityByPartId(partId);
        }

        /// <summary>
        /// Add a new part
        /// </summary>
        /// <param name="part"></param>
        public void AddPart(Part part)
        {
            // Need to check if part already exists.  If so, return "part already exists" exception
            var existingPart = PartData.GetPartByPartNumberAndRevision(part.PartNumber, part.PartRevision);
            if (existingPart != null)
                throw new Exception(string.Format("Part {0}, with revision {1} already exists", part.PartNumber, part.PartRevision));

            PartData.AddPart(part);

            var newlyAddedPart = PartData.GetPartByPartNumberAndRevision(part.PartNumber, part.PartRevision);
            if (newlyAddedPart != null)
            {
                // With the new part added, we then retreive it back from the database, so that its ID can be used in saving the attribute list.
                using (var partItemAttributeTypeLogic = new PartItemAttributeTypeLogic(PartItemAttributeTypeData))
                {
                    if (part.PartItemAttributeTypes != null)
                    {
                        partItemAttributeTypeLogic.UpdatePartItemAttributeTypeList(newlyAddedPart.PartId, part.PartItemAttributeTypes);
                    }
                }
            }
        }

        /// <summary>
        /// Update an existing part.  
        /// Note, the part number and part revision columns are NOT mutable per our own logic.
        /// If the caller somehow changes them before calling this method, an exception will be thrown.
        /// </summary>
        /// <param name="part"></param>
        public void UpdatePart(Part part)
        {
            PartData.UpdatePart(part);

            // Also need to refresh the attributes list
            using (var partItemAttributeTypeLogic = new PartItemAttributeTypeLogic(PartItemAttributeTypeData))
            {
                if (part.PartItemAttributeTypes != null)
                {
                    partItemAttributeTypeLogic.UpdatePartItemAttributeTypeList(part.PartId, part.PartItemAttributeTypes);
                }
            }
        }

        #endregion
    }
}
