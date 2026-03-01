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
    public class ItemAttributeTypeLogic : LogicBase
    {
        IItemAttributeTypeData ItemAttributeTypeData { get; }

        #region Constructor

        public ItemAttributeTypeLogic(IItemAttributeTypeData itemAttributeTypeData)
        {
            ItemAttributeTypeData = itemAttributeTypeData ?? throw new ArgumentNullException(nameof(itemAttributeTypeData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all <see cref="ItemAttributeType"/> entities.
        /// </summary>
        /// <returns></returns>
        public List<ItemAttributeType> GetAllItemAttributeTypes()
        {
            return ItemAttributeTypeData.GetAllItemAttributeTypes();
        }

        #endregion
    }
}
