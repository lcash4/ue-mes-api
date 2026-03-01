using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IInventoryItemTypeData
    {
        /// <summary>
        /// Simply return all inventory item types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<InventoryItemType> GetInventoryItemTypes();

        /// <summary>
        /// Return the inventory item type by name
        /// </summary>
        /// <param name="inventoryItemTypeName">inventory item type name to search</param>
        /// <returns></returns>
        public InventoryItemType GetInventoryItemTypeByName(string inventoryItemTypeName);
    }
}
