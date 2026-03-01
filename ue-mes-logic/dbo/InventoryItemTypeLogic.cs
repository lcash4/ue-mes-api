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
    public class InventoryItemTypeLogic : LogicBase
    {
        IInventoryItemTypeData InventoryItemTypeData { get; }

        #region Constructor

        public InventoryItemTypeLogic(IInventoryItemTypeData inventoryItemTypeData)
        {
            InventoryItemTypeData = inventoryItemTypeData ?? throw new ArgumentNullException(nameof(inventoryItemTypeData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Simply return all inventory item types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<InventoryItemType> GetInventoryItemTypes()
        {
            return InventoryItemTypeData.GetInventoryItemTypes();
        }

        /// <summary>
        /// Return the inventory item type by name
        /// </summary>
        /// <param name="inventoryItemTypeName">inventory item type name to search</param>
        /// <returns></returns>
        public InventoryItemType GetInventoryItemTypeByName(string inventoryItemTypeName)
        {
            return InventoryItemTypeData.GetInventoryItemTypeByName(inventoryItemTypeName);
        }

        #endregion
    }
}
