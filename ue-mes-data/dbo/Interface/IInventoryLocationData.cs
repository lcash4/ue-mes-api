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
    public interface IInventoryLocationData
    {
        /// <summary>
        /// Simply return an inventory location and its type by the supplied ID
        /// </summary>
        /// <returns></returns>
        public InventoryLocation GetInventoryLocationById(int locationId);

        /// <summary>
        /// Simply return an inventory location and its type by the supplied name
        /// </summary>
        /// <returns></returns>
        public InventoryLocation GetInventoryLocationByName(string locationName);

        /// <summary>
        /// Simply return all inventory locations and their type for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<InventoryLocation> GetInventoryLocations();

        /// <summary>
        /// Return all location types with the given list of names
        /// </summary>
        /// <param name="locationTypes"></param>
        /// <returns></returns>
        public List<InventoryLocation> GetInventoryLocationsByLocationTypes(string[] locationTypes);

        /// <summary>
        /// Returns a sum of inventory for each location.  This data will be used to show an aggregate view of the inventory, as well as prevent location type changes when the sum is greater than 0
        /// The result will be missing a lot of properties to simplify the group by clause.  Since this is just for a view, we can omit them and only keep the ones we need. 
        /// </summary>
        /// <returns></returns>
        public List<(InventoryLocation InventoryLocation, decimal Quantity)> GetInventorySummedByLocation();

        /// <summary>
        /// Add a new inventory location
        /// </summary>
        /// <param name="inventoryLocation"></param>
        public void AddInventoryLocation(InventoryLocation inventoryLocation);

        /// <summary>
        /// Update an existing inventory location
        /// </summary>
        /// <param name="inventoryLocation"></param>
        public void UpdateInventoryLocation(InventoryLocation inventoryLocation);

        /// <summary>
        /// Will delete all <see cref="InventoryLocation"/> records that match the incoming Ids
        /// </summary>
        /// <param name="inventoryLocationIds"></param>
        public void DeleteInventoryLocationsById(List<int> inventoryLocationIds);
    }
}
