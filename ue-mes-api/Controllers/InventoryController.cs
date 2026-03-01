using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using ue_mes_api.Dto;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;
using ue_mes_logic.dbo;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InventoryController : ControllerBase
    {
        #region Constructor

        ILogger Logger { get; }
        IInventoryItemData InventoryItemData { get; }
        IInventoryItemTypeData InventoryItemTypeData { get; }
        IInventoryLocationData InventoryLocationData { get; }
        IInventoryLocationTypeData InventoryLocationTypeData { get; }

        public InventoryController(ILogger<InventoryController> logger, 
            IInventoryItemData inventoryItemData, 
            IInventoryItemTypeData inventoryItemTypeData, 
            IInventoryLocationData inventoryLocationData,
            IInventoryLocationTypeData inventoryLocationTypeData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InventoryItemData = inventoryItemData ?? throw new ArgumentNullException(nameof(inventoryItemData));
            InventoryItemTypeData = inventoryItemTypeData ?? throw new ArgumentNullException(nameof(inventoryItemTypeData));
            InventoryLocationData = inventoryLocationData ?? throw new ArgumentNullException(nameof(inventoryLocationData));
            InventoryLocationTypeData = inventoryLocationTypeData ?? throw new ArgumentNullException(nameof(inventoryLocationTypeData));
        }

        #endregion

        #region Public API Methods

        #region Inventory Locations

        /// <summary>
        /// Simply return all inventory locations and their type for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<InventoryLocation>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("InventoryLocations")]
        public List<InventoryLocation> GetInventoryLocations()
        {
            using (var inventoryLocationLogic = new InventoryLocationLogic(InventoryLocationData))
            {
                return inventoryLocationLogic.GetInventoryLocations();
            }
        }

        /// <summary>
        /// Simply return all inventory locations and their type for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<InventoryLocation>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("InventoryLocations/{sourceLocation}")]
        public List<InventoryLocation> GetInventoryLocationsForSourceLocation(string sourceLocation)
        {
            using (var inventoryLocationLogic = new InventoryLocationLogic(InventoryLocationData))
            {
                return inventoryLocationLogic.GetInventoryLocationsForSourceLocation(sourceLocation);
            }
        }



        /// <summary>
        /// Returns a sum of each inventory type, location and part.  This data will be used to show an aggregate view of the inventory.
        /// The result will be missing a lot of properties to simplify the group by clause.  Since this is just for a view, we can omit them and only keep the ones we need.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<InventoryItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("InventoryLocationsWithItemsSummed")]
        public List<InventoryLocationWithQuantityViewModel> GetInventoryLocationsWithItemsSummed()
        {
            using (var inventoryLocationLogic = new InventoryLocationLogic(InventoryLocationData))
            {
                var inventoryLocationsWithItemsSummed = inventoryLocationLogic.GetInventorySummedByLocation();

                List<InventoryLocationWithQuantityViewModel> inventoryLocationViewModels = new List<InventoryLocationWithQuantityViewModel>();
                inventoryLocationsWithItemsSummed.ForEach(inventoryLocationWithQuantity =>
                {
                    inventoryLocationViewModels.Add(new InventoryLocationWithQuantityViewModel() { 
                        InventoryLocation = inventoryLocationWithQuantity.InventoryLocation, 
                        InventoryItemQuantity = inventoryLocationWithQuantity.Quantity 
                    });
                });
                return inventoryLocationViewModels;
            }
        }

        /// <summary>
        /// Updating inventory locations is used to add any new locations, as well as update any existing.
        /// The caller should pass in all items, with IDs assigned when already existing.
        /// The logic will add or update accordingly, as well as validate each item before saving.
        /// </summary>
        /// <param name="inventoryLocations"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("InventoryLocations/Update")]
        public IActionResult UpdateInventoryLocations([FromBody] List<InventoryLocation> inventoryLocations)
        {
            using (var inventoryLocationLogic = new InventoryLocationLogic(InventoryLocationData))
            {
                try
                {
                    inventoryLocationLogic.UpdateInventoryLocations(inventoryLocations);
                    Logger.LogInformation("Successfully updated {0} inventory locations", inventoryLocations.Count);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when updating the inventory locations.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when updating the inventory locations.  Please try again or contact MES support for help."));
                }
            }
        }

        #endregion

        #region Inventory Location Types

        /// <summary>
        /// Return a list of <see cref="InventoryLocationType"/> for use in client drop downs or lists
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<InventoryLocationType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("InventoryLocationTypes")]
        public List<InventoryLocationType> GetAllInventoryLocationTypes()
        {
            using (var inventoryLocationTypeLogic = new InventoryLocationTypeLogic(InventoryLocationTypeData))
            {
                return inventoryLocationTypeLogic.GetAllInventoryLocationTypes();
            }
        }

        #endregion

        #region Inventory Item Types

        /// <summary>
        /// Simply return all inventory item types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<InventoryItemType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("InventoryItemTypes")]
        public List<InventoryItemType> GetInventoryItemTypes()
        {
            using (var inventoryItemTypeLogic = new InventoryItemTypeLogic(InventoryItemTypeData))
            {
                return inventoryItemTypeLogic.GetInventoryItemTypes();
            }
        }

        #endregion

        #region Inventory Items

        /// <summary>
        /// Get an inventory item by serial number.  This will be used in validation logic for part consumption and identifying the current location
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(InventoryItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("{serialNumber}")]
        public InventoryItem GetInventoryItemBySerialNumber(string serialNumber)
        {
            using (var inventoryItemLogic = new InventoryItemLogic(InventoryItemData))
            {
                return inventoryItemLogic.GetInventoryItemBySerialNumber(serialNumber);
            }
        }

        /// <summary>
        /// Returns all inventory items
        /// TODO: Might be able to can this method at some point
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<InventoryItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("InventoryItemsAll")]
        public List<InventoryItem> GetAllInventoryItems()
        {
            using (var inventoryItemLogic = new InventoryItemLogic(InventoryItemData))
            {
                return inventoryItemLogic.GetAllInventoryItems();
            }
        }

        /// <summary>
        /// Returns all inventory items
        /// TODO: Might be able to can this method at some point
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<InventoryItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("ReceivingAndWarehouseItems")]
        public List<InventoryItem> GetReceivingAndWarehouseInventoryItems()
        {
            var receivingAndWarehouseLocationTypes = new string[] { "Receiving", "Warehouse" };

            using (var inventoryItemLogic = new InventoryItemLogic(InventoryItemData))
            {
                return inventoryItemLogic.GetInventoryItemsByLocationTypes(receivingAndWarehouseLocationTypes);
            }
        }

        /// <summary>
        /// Returns a sum of each inventory type, location and part.  This data will be used to show an aggregate view of the inventory.
        /// The result will be missing a lot of properties to simplify the group by clause.  Since this is just for a view, we can omit them and only keep the ones we need.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<InventoryItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("InventoryItemsSummed")]
        public List<InventoryItem> GetInventoryItemsSummed()
        {
            using (var inventoryItemLogic = new InventoryItemLogic(InventoryItemData))
            {
                return inventoryItemLogic.GetInventoryItemsSummed();
            }
        }

        #endregion

        #region Inventory Validation

        /// <summary>
        /// Validates if a serial number can be consumed by checking existing inventory for the matching part.
        /// The logic will return warning or error messages for each validation issue.  If no messages are returned, the item is valid
        /// The logic to confirm a valid serial is:
        ///   - The serial number exists in inventory
        ///   - It's location type is "Production" meaning, the inventory has been allocated for assembly
        ///   - It is not already consumed by another assembly (does not exist in consumption table, or exists with isRemoved = false)
        ///   - the partId argument matches the partId of the inventory item
        /// If not, a detailed message will be returned that will explain why it was invalid, so that the user can correct it.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ValidationMessages), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("SerialValidForConsumptionMessages/{serialNumber}/{partId}")]
        public ValidationMessages SerialValidForConsumptionMessages(string serialNumber, int partId)
        {
            using (var inventoryItemLogic = new InventoryItemLogic(InventoryItemData))
            {
                var serialValidForConsumptionKeyValuePair = inventoryItemLogic.IsSerialValidForConsumption(serialNumber, partId);
                var validationMessage = new ValidationMessages(){
                    ErrorMessages = !serialValidForConsumptionKeyValuePair.Key ? new string[1] { serialValidForConsumptionKeyValuePair.Value } : Array.Empty<string>(),
                    WarningMessages = Array.Empty<string>()

                };
                return validationMessage;
            }
        }

        #endregion

        #region Adding or Moving Inventory

        /// <summary>
        /// Adding inventory will typically take place when a new subassembly or finished goods assembly is completed.
        /// This is different from "Moving" inventory as it is being created new, and not received from a supplier or different location.
        /// </summary>
        /// <param name="inventoryItem"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("AddInventory")]
        public IActionResult AddInventory([FromBody] InventoryItem inventoryItem)
        {
            using (var inventoryItemLogic = new InventoryItemLogic(InventoryItemData))
            {
                try
                {
                    inventoryItemLogic.AddInventoryItem(inventoryItem);
                    Logger.LogInformation("Successfully added {0} quantity of part {1}({2}) to {3}", inventoryItem.Quantity.ToString(), inventoryItem.Part.PartNumber, inventoryItem.Part.PartRevision, inventoryItem.InventoryLocation.Name);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the inventory.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the inventory.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Moving inventory can take place through several actions, like moving received material into a warehouse, or moving a finished good out of the production line.
        /// The logic will either update an existing quantity for the record supplied (if the destinationLocation is already present in InventoryItem)
        /// Or, the logic will add a new record if this is the first move.
        /// After a successful update of the destination, we then need to reduce the source.
        /// That means either reducing the quantity, or if quantityToMove = current quantity, then delete the old record.
        /// TODO: Finally, we want to keep full history of any movements in the InventoryItemHistory table.  We need to insert a history for any change that happens in the description above.
        /// </summary>
        /// <param name="moveInventoryItem"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("MoveInventory")]
        public IActionResult MoveInventory([FromBody] MoveInventoryItem moveInventoryItem)
        {
            using (var inventoryItemLogic = new InventoryItemLogic(InventoryItemData))          
            {
                try
                {
                    inventoryItemLogic.MoveInventory(moveInventoryItem.InventoryItem, moveInventoryItem.DestinationLocation, moveInventoryItem.QuantityToMove);
                    Logger.LogInformation("Successfully moved {0} quantity of part {1}({2}) from {3} to {4}", moveInventoryItem.QuantityToMove.ToString(), moveInventoryItem.InventoryItem.Part.PartNumber, moveInventoryItem.InventoryItem.Part.PartRevision, moveInventoryItem.InventoryItem.InventoryLocation.Name, moveInventoryItem.DestinationLocation.Name);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when moving the inventory.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when moving the inventory.  Please try again or contact MES support for help."));
                }
            }
        }


        /// <summary>
        /// This API call is similar to MoveInventory, except it will receive a list of items and call the logic method for each.
        /// </summary>
        /// <param name="moveInventoryItems"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("MoveInventories")]
        public IActionResult MoveInventories([FromBody] List<MoveInventoryItem> moveInventoryItems)
        {
            using (var inventoryItemLogic = new InventoryItemLogic(InventoryItemData))
            {
                try
                {
                    moveInventoryItems.ForEach(moveInventoryItem =>
                    {
                        inventoryItemLogic.MoveInventory(moveInventoryItem.InventoryItem, moveInventoryItem.DestinationLocation, moveInventoryItem.QuantityToMove);
                        Logger.LogInformation("Successfully moved {0} quantity of part {1}({2}) from {3} to {4}", moveInventoryItem.QuantityToMove.ToString(), moveInventoryItem.InventoryItem.Part.PartNumber, moveInventoryItem.InventoryItem.Part.PartRevision, moveInventoryItem.InventoryItem.InventoryLocation.Name, moveInventoryItem.DestinationLocation.Name);
                    });
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when moving the list of inventory items.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when moving the list of inventory items.  Please try again or contact MES support for help."));
                }
            }
        }

        #endregion

        #endregion
    }
}
