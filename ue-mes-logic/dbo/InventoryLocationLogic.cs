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
    public class InventoryLocationLogic : LogicBase
    {
        IInventoryLocationData InventoryLocationData { get; }

        #region Constructor

        public InventoryLocationLogic(IInventoryLocationData inventoryLocationData)
        {
            InventoryLocationData = inventoryLocationData ?? throw new ArgumentNullException(nameof(inventoryLocationData));
        }

        #endregion

        /// <summary>
        /// Simply return an inventory location and its type by the supplied ID
        /// </summary>
        /// <returns></returns>
        public InventoryLocation GetInventoryLocationById(int locationId)
        {
            return InventoryLocationData.GetInventoryLocationById(locationId);
        }

        /// <summary>
        /// Simply return an inventory location and its type by the supplied name
        /// </summary>
        /// <returns></returns>
        public InventoryLocation GetInventoryLocationByName(string locationName)
        {
            return InventoryLocationData.GetInventoryLocationByName(locationName);
        }

        /// <summary>
        /// Simply return all inventory locations and their type for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<InventoryLocation> GetInventoryLocations()
        {
            return InventoryLocationData.GetInventoryLocations();
        }

        /// <summary>
        /// Return all inventory locations based on the sourceLocation.  Moving inventory is restricted to a specific flow.  For example, you cannot move Finished Goods into the Receiving location.
        /// The logic in this call will restrict the locations returned based on the calling source.
        /// </summary>
        /// <param name="sourceLocation"></param>
        /// <returns></returns>
        public List<InventoryLocation> GetInventoryLocationsForSourceLocation(string sourceLocation)
        {
            // The way this will work is through a switch statement.  
            // The switch will assign an array of InventoryLocationType names based on the source.  Those will then be passed into the data class and used in the WHERE clause.
            string[] locationTypesArray = null;// new string[0];

            switch (sourceLocation)
            {
                // New is a hard coded value for when inventory is received from a supplier for the first time
                case "New":
                    locationTypesArray = new string[] { "Receiving" };
                    break;
                case "Receiving":
                    locationTypesArray = new string[] { "Warehouse" };
                    break;
                default:
                    break;
            }

            // If for some reason a bogus sourceLocation is provided, throw an exception to fail the API call
            if (locationTypesArray == null || locationTypesArray.Length == 0)
                throw new InvalidDataException(string.Format("The supplied source location of {0} does not contain any inventory locations.", sourceLocation));

            return InventoryLocationData.GetInventoryLocationsByLocationTypes(locationTypesArray);
        }

        /// <summary>
        /// Returns a sum of inventory for each location.  This data will be used to show an aggregate view of the inventory, as well as prevent location type changes when the sum is greater than 0
        /// The result will be missing a lot of properties to simplify the group by clause.  Since this is just for a view, we can omit them and only keep the ones we need. 
        /// </summary>
        /// <returns></returns>
        public List<(InventoryLocation InventoryLocation, decimal Quantity)> GetInventorySummedByLocation()
        {
            return InventoryLocationData.GetInventorySummedByLocation();
        }

        /// <summary>
        /// This method will add any new locations, and update any existing.  
        /// Deletes will be handled by first extracting the list of locations and then deleting any that are not in the new list.
        /// The caller should be passing in the existing location IDs, but we will also do lookups to ensure there are no duplicates.
        /// </summary>
        /// <param name="inventoryLocations"></param>
        public void UpdateInventoryLocations(List<InventoryLocation> inventoryLocations)
        {
            var newInventoryLocationIds = inventoryLocations.Select(inventoryLocation => inventoryLocation.InventoryLocationId).ToList();

            // Delete any that are not in the new list.
            var currentInventoryLocations = InventoryLocationData.GetInventorySummedByLocation();
            var inventoryLocationsToDelete = currentInventoryLocations.Where(currentInventoryLocation => !newInventoryLocationIds.Contains(currentInventoryLocation.InventoryLocation.InventoryLocationId)).ToList();
            if (inventoryLocationsToDelete != null && inventoryLocationsToDelete.Count > 0)
            {
                // We have records to delete.  If the caller somehow bybassed the client and submitted an item to delete that has existing quantity, we will throw an exception.
                if (inventoryLocationsToDelete.Any(inventoryLocationToDelete => inventoryLocationToDelete.Quantity > 0))
                    throw new InvalidDataException("There was an attempt to delete a location that has quantity greater than 0, which is not valid.  The existing inventory will need moved before this record can be deleted.");

                // Otherwise, the delete can be processed
                var inventoryLocationIdsToDelete = inventoryLocationsToDelete.Select(inventoryLocation => inventoryLocation.InventoryLocation.InventoryLocationId).ToList();
                InventoryLocationData.DeleteInventoryLocationsById(inventoryLocationIdsToDelete);
            }

            inventoryLocations.ForEach(inventoryLocation =>
            {
                // Update if the caller passed in a location with ID > 0 (existing)
                if (inventoryLocation.InventoryLocationId > 0)
                {
                    InventoryLocationData.UpdateInventoryLocation(inventoryLocation);
                }
                else
                {
                    // Add a new location since the ID is 0.  If the caller sends a duplicate name, throw an exception back to the caller.
                    var existingInventoryLocation = InventoryLocationData.GetInventoryLocationByName(inventoryLocation.Name);

                    if (existingInventoryLocation != null)
                        throw new InvalidDataException(string.Format("There was an attempt to add a new location with name = {0}, but this location already exist.  The record will need a different name, or the existing record can be updated.", inventoryLocation.Name));

                    InventoryLocationData.AddInventoryLocation(inventoryLocation);
                }
            });
        }
    }
}
