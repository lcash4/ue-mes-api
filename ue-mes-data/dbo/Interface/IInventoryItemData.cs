using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IInventoryItemData
    {
        /// <summary>
        /// Returns all inventory items
        /// </summary>
        /// <returns></returns>
        public List<InventoryItem> GetAllInventoryItems();

        /// <summary>
        /// Returns all inventory items for the given location types.
        /// </summary>
        /// <param name="inventoryLocationTypes"></param>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsByLocationTypes(string[] inventoryLocationTypes);

        /// <summary>
        /// Returns all inventory items for the given location ID.
        /// </summary>
        /// <param name="inventoryLocationId"></param>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsByLocationId(int inventoryLocationId);

        /// <summary>
        /// Returns a sum of each inventory type, location and part.  This data will be used to show an aggregate view of the inventory.
        /// The result will be missing a lot of properties to simplify the group by clause.  Since this is just for a view, we can omit them and only keep the ones we need. 
        /// </summary>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsSummed();

        /// <summary>
        /// Returns a inventory items grouped by their part and attributes.
        /// The attributes will be combined into a concatenated string for display purposes as well
        /// </summary>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsWithAttributesConcatenated();

        /// <summary>
        /// Get an inventory item by serial number.  This will be used in validation logic for part consumption and identifying the current location
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public InventoryItem GetInventoryItemBySerialNumber(string serialNumber);

        /// <summary>
        /// Get all inventory items by part and serial number.  
        /// </summary>
        /// <param name="partId"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsByPartAndSerialNumber(int partId, string serialNumber);

        /// <summary>
        /// The natural key of an inventory item is part + serial + location.  This method will be used to prevent duplicate entries into the table
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="partRevision"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public InventoryItem GetInventoryItemByPartSerialAndLocation(string partNumber, string partRevision, string serialNumber, string locationName);

        /// <summary>
        /// Add a new inventory item
        /// </summary>
        /// <param name="inventoryItem"></param>
        public void AddInventoryItem(InventoryItem inventoryItem);

        /// <summary>
        /// Update an existing inventory item.  Typically this is done to change its location, or change the quantity.
        /// </summary>
        /// <param name="inventoryItem"></param>
        public void UpdateInventoryItem(InventoryItem inventoryItem);

        /// <summary>
        /// Delete an existing inventory item and its attributes. Typically this is done when all of an inventory item's quantity has been moved somewhere else.
        /// </summary>
        /// <param name="inventoryItem"></param>
        public void DeleteInventoryItem(long inventoryItemId);
    }
}
