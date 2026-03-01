using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class InventoryItemLogic : LogicBase
    {
        IInventoryItemData InventoryItemData { get; }

        #region Constructor

        public InventoryItemLogic(IInventoryItemData inventoryItemData)
        {
            InventoryItemData = inventoryItemData ?? throw new ArgumentNullException(nameof(inventoryItemData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get an inventory item by serial number.  This will be used in validation logic for part consumption and identifying the current location
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public InventoryItem GetInventoryItemBySerialNumber(string serialNumber)
        {
            return InventoryItemData.GetInventoryItemBySerialNumber(serialNumber);
        }

        /// <summary>
        /// Returns all inventory items
        /// </summary>
        /// <returns></returns>
        public List<InventoryItem> GetAllInventoryItems()
        {
            return InventoryItemData.GetAllInventoryItems();
        }

        /// <summary>
        /// Returns a sum of each inventory type, location and part.  This data will be used to show an aggregate view of the inventory.
        /// The result will be missing a lot of properties to simplify the group by clause.  Since this is just for a view, we can omit them and only keep the ones we need.
        /// </summary>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsSummed()
        {
            var inventoryItemsWithAttributes = InventoryItemData.GetInventoryItemsWithAttributesConcatenated();

            return inventoryItemsWithAttributes
            .GroupBy(x => new { x.InventoryItemType.InventoryItemTypeId, x.Part.PartNumber, x.Part.PartRevision, PartDescription = x.Part.Description, PartUnitOfMeasure = x.Part.UnitOfMeasure.Name, InventoryLocationName = x.InventoryLocation.Name, InventoryLocationTypeName = x.InventoryLocation.InventoryLocationType.Name, x.InventoryItemAttributesConcatenated })
                .Select(x => new InventoryItem
                {
                    InventoryItemId = 0,
                    InventoryItemType = new InventoryItemType() { InventoryItemTypeId = x.Key.InventoryItemTypeId },
                    InventoryLocation = new InventoryLocation() { Name = x.Key.InventoryLocationName, InventoryLocationType = new InventoryLocationType() { Name = x.Key.InventoryLocationTypeName } },
                    Part = new Part() { PartNumber = x.Key.PartNumber, PartRevision = x.Key.PartRevision, Description = x.Key.PartDescription, UnitOfMeasure = new UnitOfMeasure() { Name = x.Key.PartUnitOfMeasure } },
                    SerialNumber = string.Empty,
                    InventoryItemAttributes = x.SelectMany(y => y.InventoryItemAttributes).Select(z => new InventoryItemAttribute() { ItemAttributeType = z.ItemAttributeType, AttributeValue = z.AttributeValue }).DistinctBy(distinctAttribute => new { distinctAttribute.AttributeValue, distinctAttribute.ItemAttributeType.ItemAttributeTypeId }).ToList(),
                    InventoryItemAttributesConcatenated = x.Key.InventoryItemAttributesConcatenated,
                    Quantity = x.Sum(y => y.Quantity)
                })
                .ToList();
        }

        /// <summary>
        /// Returns a inventory items grouped by their part and attributes.
        /// The attributes will be combined into a concatenated string for display purposes as well
        /// </summary>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsWithAttributesConcatenated()
        {
            return InventoryItemData.GetInventoryItemsWithAttributesConcatenated();
        }


        /// <summary>
        /// Validates if a serial number can be consumed by checking existing inventory for the matching part.
        /// The logic will return true if:
        ///   - The serial number exists in inventory
        ///   - It's location type is "Production" meaning, the inventory has been allocated for assembly
        ///   - It is not already consumed by another assembly (does not exist in consumption table, or exists with isRemoved = false)
        ///   - the partId argument matches the partId of the inventory item
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<bool, string> IsSerialValidForConsumption(string serialNumber, int partId)
        {
            using (var orderItemUnitConsumptionData = new OrderItemUnitConsumptionData())
            using (var partData = new PartData())
            {
                var serialNumberInventoryItems = InventoryItemData.GetInventoryItemsByPartAndSerialNumber(partId, serialNumber);
                var productionSerialNumberInventoryItem = serialNumberInventoryItems.Where(inventoryItem => inventoryItem.InventoryLocation.InventoryLocationType.Name == "Production").FirstOrDefault();
                var expectedPart = partData.GetPartEntityByPartId(partId);

                // Serial doesn't exist in inventory, so return false
                if (productionSerialNumberInventoryItem == null && (serialNumberInventoryItems == null || serialNumberInventoryItems.Count == 0))
                    return new KeyValuePair<bool, string>(false, string.Format("The serial number {0} does not exist in inventory or is not assigned to part {1}({2}).  Please work with Inventory Control to update the inventory if this is a valid part.", serialNumber, expectedPart.PartNumber, expectedPart.PartRevision));

                // The inventory item needs to have been allocated to a Production location from an order.  Parts sitting in a warehouse or other location cannot be consumed.
                if (serialNumberInventoryItems != null && serialNumberInventoryItems.Count > 0 && productionSerialNumberInventoryItem == null)
                    return new KeyValuePair<bool, string>(false, string.Format("The serial number {0} has not been allocated to Production inventory.  Please work with Inventory Control to allocate this inventory to an in progress order if this is a valid part.", serialNumber));

                var serialNumberConsumptions = orderItemUnitConsumptionData.GetOrderItemUnitConsumptionsByConsumedSerialNumber(serialNumber);

                // Finally, the serial number cannot already be consumed, so return false with the message if it is.
                if (serialNumberConsumptions != null && productionSerialNumberInventoryItem != null && serialNumberConsumptions.Count == productionSerialNumberInventoryItem.Quantity)
                    return new KeyValuePair<bool, string>(false, string.Format("The serial number {0} is already fully consumed in other assemblies.  It will need to be removed from another assembly before it can be consumed here.  Or, you will need to have inventory control allocate more inventory", serialNumber));

                // We've reached the end of validations so return true
                return new KeyValuePair<bool, string>(true, string.Empty);
            }
        }

        /// <summary>
        /// Returns all inventory items for the given location types.
        /// </summary>
        /// <param name="inventoryLocationTypes"></param>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsByLocationTypes(string[] inventoryLocationTypes)
        {
            return InventoryItemData.GetInventoryItemsByLocationTypes(inventoryLocationTypes);
        }

        /// <summary>
        /// Add a new inventory item
        /// </summary>
        /// <param name="inventoryItem"></param>
        public void AddInventoryItem(InventoryItem inventoryItem)
        {
            using (var partItemAttributeTypeData = new PartItemAttributeTypeData())
            {
                // Due to different attributes that could be applied to a lot, we can't assume Lot#+Part is a duplicate.  For example, 1 rack of glass could provide multiple sizes and they need to be treated as separate inventory.
                // We do need to ensure that all necessary attributes have been provided values.
                var partItemAttributeTypes = partItemAttributeTypeData.GetPartItemAttributeTypesByPartId(inventoryItem.Part.PartId);
                inventoryItem.InventoryItemAttributes.ForEach(inventoryItemAttribute =>
                {
                    var matchingPartItemAttributeType = partItemAttributeTypes.Where(partItemAttributeType => partItemAttributeType.ItemAttributeType.ItemAttributeTypeId == inventoryItemAttribute.ItemAttributeType.ItemAttributeTypeId).FirstOrDefault();

                    // If the attribute is not found for this part, then a bad payload was sent and we need to throw the exception
                    if (matchingPartItemAttributeType == null)
                        throw new InvalidDataException(string.Format("The inventory item has an attribute {0} that is not assigned to its part {1}({2})", inventoryItemAttribute.ItemAttributeType.Name, inventoryItem.Part.PartNumber, inventoryItem.Part.PartRevision));

                    // If the attribute is found, is required, but does not have a value, then we need to throw an exception
                    if (matchingPartItemAttributeType != null && matchingPartItemAttributeType.IsRequired && inventoryItemAttribute.AttributeValue.IsNullOrEmpty())
                        throw new InvalidDataException(string.Format("The inventory item has an attribute {0} that is required, but does not have a value", inventoryItemAttribute.ItemAttributeType.Name));

                });
                InventoryItemData.AddInventoryItem(inventoryItem);
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
        /// <param name="inventoryItem"></param>
        /// <param name="destinationLocation"></param>
        /// <param name="quantityToMove"></param>
        public void MoveInventory(InventoryItem inventoryItem, InventoryLocation destinationLocation, decimal quantityToMove)
        {
            // If for some reason the quantityToMove is 0 or less, we will just bail.  The caller is dumb
            if (quantityToMove <= 0)
                return;

            // Start off by getting source and destination records.
            // The source should exist because that is how we got here, but we'll check it anyway in case the API is somehow hit directly.
            // The destination may not exist yet, which is fine.  It just means we need to add a new record.
            var sourceInventoryItem = InventoryItemData.GetInventoryItemByPartSerialAndLocation(inventoryItem.Part.PartNumber, inventoryItem.Part.PartRevision, inventoryItem.SerialNumber, inventoryItem.InventoryLocation.Name);
            var destinationInventoryItem = InventoryItemData.GetInventoryItemByPartSerialAndLocation(inventoryItem.Part.PartNumber, inventoryItem.Part.PartRevision, inventoryItem.SerialNumber, destinationLocation.Name);

            // If the source item is somehow not found for a destination location other than Receiving (new item), we'll return an invalid data exception so that the caller knows the record does not exist.
            if (destinationLocation.InventoryLocationType != null && destinationLocation.InventoryLocationType.Name != "Receiving" && (sourceInventoryItem == null || sourceInventoryItem.InventoryItemId <= 0))
                throw new InvalidDataException(string.Format("This inventory item was not found.  Check to make sure the part, serial number and location are correct.  Part:{0}({1}), Serial Number: {2}, Location: {3}", inventoryItem.Part.PartNumber, inventoryItem.Part.PartRevision, inventoryItem.SerialNumber, inventoryItem.InventoryLocation.Name));

            // If somehow the quantityToMove is greater than the remaining source quantity, then we need to fail the method
            if (destinationLocation.InventoryLocationType != null && destinationLocation.InventoryLocationType.Name != "Receiving" && sourceInventoryItem.Quantity < quantityToMove)
                throw new InvalidDataException(string.Format("This inventory item does not have enough quantity to move.  Current quantiy = {0}, quantity to move = {1}.", inventoryItem.Quantity, quantityToMove));

            // Since we've passed validations, first thing we will do is check whether a destination is present.  If so, we manage the updates.  If not, this is a simple add of a new record.
            // Note that all inventory item attributes have to match as well, or else we assume it is a new item.
            if (destinationInventoryItem == null
                || destinationInventoryItem != null
                    && inventoryItem.InventoryItemAttributes.Where(inventoryItemAttribute => destinationInventoryItem.InventoryItemAttributes.Any(destinationInventoryItemAttribute => destinationInventoryItemAttribute.ItemAttributeType.ItemAttributeTypeId == inventoryItemAttribute.ItemAttributeType.ItemAttributeTypeId && destinationInventoryItemAttribute.AttributeValue != inventoryItemAttribute.AttributeValue)).Count() > 0)
            {
                // no existing destination item was found, so we can simply add the sourceInventoryItem as a new item.
                // First, we need to change the location to the destination, reset the ID to 0 and then set the Quantity property to the quantityToMove parameter.
                var sourceItemWithNewDestination = inventoryItem;
                sourceItemWithNewDestination.InventoryItemId = 0;
                sourceItemWithNewDestination.InventoryLocation = destinationLocation;
                sourceItemWithNewDestination.Quantity = quantityToMove;

                AddInventoryItem(sourceItemWithNewDestination);
                UpdateSourceQuantityOrRemoveIfEmpty(sourceInventoryItem, quantityToMove, InventoryItemData);
            }
            else
            {
                // Since we have an existing destination record, we start by updating the quantity at the existing destination
                var existingDestinationItemWithNewQuantity = destinationInventoryItem;
                existingDestinationItemWithNewQuantity.Quantity += quantityToMove;
                InventoryItemData.UpdateInventoryItem(existingDestinationItemWithNewQuantity);
                UpdateSourceQuantityOrRemoveIfEmpty(sourceInventoryItem, quantityToMove, InventoryItemData);
            }
        }

        private void UpdateSourceQuantityOrRemoveIfEmpty(InventoryItem sourceInventoryItem, decimal quantityToMove, IInventoryItemData inventoryItemData)
        {
            if (sourceInventoryItem != null && sourceInventoryItem.InventoryItemId != 0)
            {
                // Now we need to reduce the source by the same amount, or if they are equal, delete the source as it no longer has any quantity left.
                // Here, they are equal, so we delete
                if (sourceInventoryItem.Quantity == quantityToMove)
                    inventoryItemData.DeleteInventoryItem(sourceInventoryItem.InventoryItemId);

                // Here, the quantity is still greater, so we simply reduce.
                if (sourceInventoryItem.Quantity > quantityToMove)
                {
                    var existingSourceItemWithNewQuantity = sourceInventoryItem;
                    existingSourceItemWithNewQuantity.Quantity -= quantityToMove;
                    inventoryItemData.UpdateInventoryItem(existingSourceItemWithNewQuantity);
                }
            }
        }

        #endregion
    }
}
