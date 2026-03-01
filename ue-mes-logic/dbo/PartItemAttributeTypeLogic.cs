using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class PartItemAttributeTypeLogic : LogicBase
    {
        IPartItemAttributeTypeData PartItemAttributeTypeData { get; }

        #region Constructor

        public PartItemAttributeTypeLogic(IPartItemAttributeTypeData partItemAttributeTypeData)
        {
            PartItemAttributeTypeData = partItemAttributeTypeData ?? throw new ArgumentNullException(nameof(partItemAttributeTypeData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of <see cref="PartItemAttributeType"/> entities that match the part ID parameter
        /// </summary>
        /// <param name="partId">Unique part id to search</param>
        /// <returns></returns>
        public List<PartItemAttributeType> GetPartItemAttributeTypesByPartId(int partId)
        {
            return PartItemAttributeTypeData.GetPartItemAttributeTypesByPartId(partId);
        }

        /// <summary>
        /// Replace all existing part item attribute types with a new set.
        /// </summary>
        /// <param name="partId">ID of the part to update</param>
        /// <param name="partItemAttributeTypes">The new list of <see cref="PartItemAttributeType"/> to assign to this bill of process</param>
        public void UpdatePartItemAttributeTypeList(int partId, List<PartItemAttributeType> partItemAttributeTypes)
        {
            // First, delete the old
            PartItemAttributeTypeData.DeletePartItemAttributeTypesByPart(partId);

            // Then add the new list
            PartItemAttributeTypeData.AddPartItemAttributeTypes(partId, partItemAttributeTypes);
        }

        #endregion
    }
}
