using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class BillOfMaterialData : DbContextBase, IBillOfMaterialData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.BillOfMaterial, BillOfMaterial>()
                .ReverseMap()
                    .ForMember(billOfMaterialModel => billOfMaterialModel.PartId, opt => opt.MapFrom(billOfMaterialEntity => billOfMaterialEntity.Part.PartId))
                    .ForMember(billOfMaterialModel => billOfMaterialModel.Part, opt => opt.Ignore());

            mapperConfig.CreateMap<Model.BillOfMaterialPart, BillOfMaterialPart>()
                .ReverseMap()
                    .ForMember(billOfMaterialPartModel => billOfMaterialPartModel.PartId, opt => opt.MapFrom(billOfMaterialPartEntity => billOfMaterialPartEntity.Part.PartId))
                    .ForMember(billOfMaterialPartModel => billOfMaterialPartModel.Part, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.UnitOfMeasure, UnitOfMeasure>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public BillOfMaterialData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="partId">Unique Part ID that is built from this <see cref="BillOfMaterial"/></param>
        /// <returns></returns>
        public BillOfMaterial GetBillOfMaterialByPartId(int partId)
        {
            BillOfMaterial billOfMaterial = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfMaterialRecord = mesProductionContext.BillOfMaterials
                    .Where(billOfMaterial => billOfMaterial.PartId == partId)
                    .Include(billOfMaterial => billOfMaterial.Part)
                    .Include(billOfMaterial => billOfMaterial.BillOfMaterialParts)
                    .ThenInclude(billOfMaterialPart => billOfMaterialPart.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .OrderByDescending(billOfMaterial => billOfMaterial.EffectiveStartDateUtc)
                    .FirstOrDefault();

                if (billOfMaterialRecord != null)
                {
                    billOfMaterial = mapper.Map<BillOfMaterial>(billOfMaterialRecord);
                }
            }

            return billOfMaterial;
        }

        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="billOfMaterialId">Unique Bill of Material ID for the <see cref="BillOfMaterial"/></param>
        /// <returns></returns>
        public BillOfMaterial GetBillOfMaterialById(int billOfMaterialId)
        {
            BillOfMaterial billOfMaterial = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfMaterialRecord = mesProductionContext.BillOfMaterials
                    .Where(billOfMaterial => billOfMaterial.BillOfMaterialId == billOfMaterialId)
                    .Include(billOfMaterial => billOfMaterial.Part)
                    .Include(billOfMaterial => billOfMaterial.BillOfMaterialParts)
                    .ThenInclude(billOfMaterialPart => billOfMaterialPart.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .OrderByDescending(billOfMaterial => billOfMaterial.EffectiveStartDateUtc)
                    .FirstOrDefault();

                if (billOfMaterialRecord != null)
                {
                    billOfMaterial = mapper.Map<BillOfMaterial>(billOfMaterialRecord);
                }
            }

            return billOfMaterial;
        }

        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="consumedPartId">Unique Part ID that needs to be included in the BOM part list</param>
        /// <returns></returns>
        public List<BillOfMaterial> GetBillOfMaterialsThatConsumePartId(int consumedPartId)
        {
            List<BillOfMaterial> billOfMaterials = new List<BillOfMaterial>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfMaterialRecords = mesProductionContext.BillOfMaterials
                    .Include(billOfMaterial => billOfMaterial.Part)
                    .Join(mesProductionContext.BillOfMaterialParts,
                        billOfMaterial => billOfMaterial.BillOfMaterialId,
                        billOfMaterialPart => billOfMaterialPart.BillOfMaterialId,
                        (billOfMaterialResult, billOfMaterialPartResult) => new { billOfMaterialResult, billOfMaterialPartResult })
                    .Where(billOfMaterialAndPartsResult => billOfMaterialAndPartsResult.billOfMaterialResult.EffectiveStartDateUtc < DateTime.UtcNow && (billOfMaterialAndPartsResult.billOfMaterialResult.EffectiveEndDateUtc == null || billOfMaterialAndPartsResult.billOfMaterialResult.EffectiveEndDateUtc > DateTime.UtcNow)
                        && billOfMaterialAndPartsResult.billOfMaterialPartResult.PartId == consumedPartId)
                    .Select(billOfMaterialAndPartsResult => billOfMaterialAndPartsResult.billOfMaterialResult)
                    .ToList();

                if (billOfMaterialRecords != null)
                {
                    billOfMaterials = mapper.Map<List<Model.BillOfMaterial>, List<BillOfMaterial>>(billOfMaterialRecords);
                }
            }

            return billOfMaterials;
        }

        /// <summary>
        /// Add a new bill of material
        /// </summary>
        /// <param name="billOfMaterial"></param>
        public void AddBillOfMaterial(BillOfMaterial billOfMaterial)
        {
            var billOfMaterialRecord = mapper.Map<Model.BillOfMaterial>(billOfMaterial);
            if (billOfMaterialRecord != null)
            {
                // With the model object mapped and validated, we can now set the UTC versions of the effective dates.
                billOfMaterialRecord.EffectiveStartDate = billOfMaterial.EffectiveStartDateUtc.ToLocalTime(); // The caller will only provide UTC time
                billOfMaterialRecord.EffectiveEndDate = billOfMaterial.EffectiveEndDateUtc?.ToLocalTime(); // The caller will only provide UTC time

                // Set the Last Modified By if present
                billOfMaterialRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfMaterial.LastModifiedBy) ? billOfMaterial.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.BillOfMaterials.Add(billOfMaterialRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing bill of material.  
        /// </summary>
        /// <param name="billOfMaterial"></param>
        public void UpdateBillOfMaterial(BillOfMaterial billOfMaterial)
        {
            var existingBillOfMaterial = GetBillOfMaterialByBillOfMaterialId(billOfMaterial.BillOfMaterialId);

            if (existingBillOfMaterial == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingBillOfMaterial.Name != billOfMaterial.Name || existingBillOfMaterial.PartId != billOfMaterial.Part.PartId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingBillOfMaterial.Description = billOfMaterial.Description;
                existingBillOfMaterial.EffectiveStartDateUtc = billOfMaterial.EffectiveStartDateUtc;
                existingBillOfMaterial.EffectiveStartDate = billOfMaterial.EffectiveStartDateUtc.ToLocalTime(); // The caller will only provide UTC time
                existingBillOfMaterial.EffectiveEndDateUtc = billOfMaterial.EffectiveEndDateUtc;
                existingBillOfMaterial.EffectiveEndDate = billOfMaterial.EffectiveEndDateUtc?.ToLocalTime(); // The caller will only provide UTC time
                existingBillOfMaterial.LastModifiedTime = DateTime.Now;
                existingBillOfMaterial.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingBillOfMaterial.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfMaterial.LastModifiedBy) ? billOfMaterial.LastModifiedBy : existingBillOfMaterial.LastModifiedBy;

                mesProductionContext.Entry(existingBillOfMaterial).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to get the existing record for modification or deletion.
        /// </summary>
        /// <param name="billOfMaterialId"></param>
        /// <returns></returns>
        private Model.BillOfMaterial GetBillOfMaterialByBillOfMaterialId(int billOfMaterialId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.BillOfMaterials
                    .Where(billOfMaterial => billOfMaterial.BillOfMaterialId == billOfMaterialId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
