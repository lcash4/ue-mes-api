using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ue_mes_data.dbo
{
    public class InventoryItemData : DbContextBase, IInventoryItemData
    {
        #region AutoMapper Config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.InventoryItem, InventoryItem>()
                .ReverseMap()
                    .ForMember(inventoryItemModel => inventoryItemModel.InventoryItemTypeId, opt => opt.MapFrom(inventoryItemEntity => inventoryItemEntity.InventoryItemType.InventoryItemTypeId))
                    .ForMember(inventoryItemModel => inventoryItemModel.InventoryLocationId, opt => opt.MapFrom(inventoryItemEntity => inventoryItemEntity.InventoryLocation.InventoryLocationId))
                    .ForMember(inventoryItemModel => inventoryItemModel.PartId, opt => opt.MapFrom(inventoryItemEntity => inventoryItemEntity.Part.PartId))
                    .ForMember(inventoryItemModel => inventoryItemModel.SupplierId, opt => opt.MapFrom(inventoryItemEntity => inventoryItemEntity.Supplier.SupplierId))
                    .ForMember(inventoryItemModel => inventoryItemModel.InventoryItemType, opt => opt.Ignore())
                    .ForMember(inventoryItemModel => inventoryItemModel.InventoryLocation, opt => opt.Ignore())
                    .ForMember(inventoryItemModel => inventoryItemModel.Part, opt => opt.Ignore())
                    .ForMember(inventoryItemModel => inventoryItemModel.Supplier, opt => opt.Ignore());

            mapperConfig.CreateMap<Model.InventoryItemType, InventoryItemType>();
            mapperConfig.CreateMap<Model.InventoryLocation, InventoryLocation>();
            mapperConfig.CreateMap<Model.InventoryLocationType, InventoryLocationType>();
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.UnitOfMeasure, UnitOfMeasure>();
            mapperConfig.CreateMap<Model.Supplier, Supplier>();
            mapperConfig.CreateMap<Model.InventoryItemAttribute, InventoryItemAttribute>()
                .ReverseMap()
                    .ForMember(inventoryItemAttributeModel => inventoryItemAttributeModel.ItemAttributeTypeId, opt => opt.MapFrom(inventoryItemAttributeEntity => inventoryItemAttributeEntity.ItemAttributeType.ItemAttributeTypeId))
                    .ForMember(inventoryItemAttributeModel => inventoryItemAttributeModel.ItemAttributeType, opt => opt.Ignore())
                    .ForMember(inventoryItemAttributeModel => inventoryItemAttributeModel.InventoryItem, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.ItemAttributeType, ItemAttributeType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor
        public InventoryItemData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all inventory items
        /// </summary>
        /// <returns></returns>
        public List<InventoryItem> GetAllInventoryItems()
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemRecords = mesProductionContext.InventoryItems
                    .Include(inventoryItem => inventoryItem.InventoryItemType)
                    .Include(inventoryItem => inventoryItem.InventoryLocation)
                    .ThenInclude(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .Include(inventoryItem => inventoryItem.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .Include(inventoryItem => inventoryItem.Supplier)
                    .Include(inventoryItem => inventoryItem.InventoryItemAttributes)
                    .ThenInclude(inventoryItemAttribute => inventoryItemAttribute.ItemAttributeType)
                    .ToList();

                if (inventoryItemRecords != null)
                {
                    inventoryItems = mapper.Map<List<Model.InventoryItem>, List<InventoryItem>>(inventoryItemRecords);
                }
            }

            return inventoryItems;
        }

        /// <summary>
        /// Returns all inventory items for the given location types.
        /// </summary>
        /// <param name="inventoryLocationTypes"></param>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsByLocationTypes(string[] inventoryLocationTypes)
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemRecords = mesProductionContext.InventoryItems
                    .Where(inventoryItem => inventoryLocationTypes.Contains(inventoryItem.InventoryLocation.InventoryLocationType.Name))
                    .Include(inventoryItem => inventoryItem.InventoryItemType)
                    .Include(inventoryItem => inventoryItem.InventoryLocation)
                    .ThenInclude(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .Include(inventoryItem => inventoryItem.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .Include(inventoryItem => inventoryItem.Supplier)
                    .Include(inventoryItem => inventoryItem.InventoryItemAttributes)
                    .ThenInclude(inventoryItemAttribute => inventoryItemAttribute.ItemAttributeType)
                    .ToList();

                if (inventoryItemRecords != null)
                {
                    return mapper.Map<List<Model.InventoryItem>, List<InventoryItem>>(inventoryItemRecords);
                }
            }

            return inventoryItems;
        }

        /// <summary>
        /// Returns all inventory items for the given location ID.
        /// </summary>
        /// <param name="inventoryLocationId"></param>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsByLocationId(int inventoryLocationId)
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemRecords = mesProductionContext.InventoryItems
                    .Where(inventoryItem => inventoryItem.InventoryLocationId == inventoryLocationId)
                    .Include(inventoryItem => inventoryItem.InventoryItemType)
                    .Include(inventoryItem => inventoryItem.InventoryLocation)
                    .ThenInclude(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .Include(inventoryItem => inventoryItem.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .Include(inventoryItem => inventoryItem.Supplier)
                    .Include(inventoryItem => inventoryItem.InventoryItemAttributes)
                    .ThenInclude(inventoryItemAttribute => inventoryItemAttribute.ItemAttributeType)
                    .ToList();

                if (inventoryItemRecords != null)
                {
                    inventoryItems = mapper.Map<List<Model.InventoryItem>, List<InventoryItem>>(inventoryItemRecords);
                }
            }

            return inventoryItems;
        }

        /// <summary>
        /// Returns a sum of each inventory type, location and part.  This data will be used to show an aggregate view of the inventory.
        /// The result will be missing a lot of properties to simplify the group by clause.  Since this is just for a view, we can omit them and only keep the ones we need. 
        /// </summary>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsSummed()
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemRecords = mesProductionContext.InventoryItems
                    .Include(inventoryItem => inventoryItem.InventoryItemType)
                    .Include(inventoryItem => inventoryItem.InventoryLocation)
                    .ThenInclude(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .Include(inventoryItem => inventoryItem.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .GroupBy(x => new { x.InventoryItemTypeId, x.Part.PartNumber, x.Part.PartRevision, PartDescription = x.Part.Description, PartUnitOfMeasure = x.Part.UnitOfMeasure.Name, InventoryLocationName = x.InventoryLocation.Name, InventoryLocationTypeName = x.InventoryLocation.InventoryLocationType.Name })
                    .Select(x => new Model.InventoryItem
                    {
                        InventoryItemId = 0,
                        InventoryItemType = new Model.InventoryItemType() { InventoryItemTypeId = x.Key.InventoryItemTypeId },
                        InventoryLocation = new Model.InventoryLocation() { Name = x.Key.InventoryLocationName, InventoryLocationType = new Model.InventoryLocationType() { Name = x.Key.InventoryLocationTypeName } },
                        Part = new Model.Part() { PartNumber = x.Key.PartNumber, PartRevision = x.Key.PartRevision, Description = x.Key.PartDescription, UnitOfMeasure = new Model.UnitOfMeasure() { Name = x.Key.PartUnitOfMeasure } },
                        SerialNumber = string.Empty,
                        Quantity = x.Sum(y => y.Quantity)
                    })
                    .ToList();

                if (inventoryItemRecords != null)
                {
                    inventoryItems = mapper.Map<List<Model.InventoryItem>, List<InventoryItem>>(inventoryItemRecords);
                }
            }

            return inventoryItems;
        }

        /// <summary>
        /// Returns a inventory items grouped by their part and attributes.
        /// The attributes will be combined into a concatenated string for display purposes as well
        /// </summary>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsWithAttributesConcatenated()
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemRecords = mesProductionContext.InventoryItems
                    .Include(inventoryItem => inventoryItem.InventoryLocation)
                    .ThenInclude(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .Include(inventoryItem => inventoryItem.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .Include(inventoryItem => inventoryItem.Supplier)
                    .Include(inventoryItem => inventoryItem.InventoryItemAttributes)
                    .ThenInclude(inventoryItemAttribute => inventoryItemAttribute.ItemAttributeType)
                    .Select(ii => new  {
                        InventoryItemId = ii.InventoryItemId,
                        InventoryItemType = ii.InventoryItemType,
                        InventoryLocation = ii.InventoryLocation,
                        Part = ii.Part,
                        Supplier = ii.Supplier,
                        UnitOfMeasure = ii.Part.UnitOfMeasure,
                        SerialNumber = ii.SerialNumber,
                        Quantity = ii.Quantity,
                        LastModifiedBy = ii.LastModifiedBy,
                        LastModifiedTime = ii.LastModifiedTime,
                        LastModifiedTimeUtc = ii.LastModifiedTimeUtc,
                        InventoryItemAttributes = ii.InventoryItemAttributes,
                        InventoryItemAttributesConcatenated = string.Join(", ", ii.InventoryItemAttributes.Select(attribute => attribute.ItemAttributeType.Name + ": " + attribute.AttributeValue))
                    })
                    .ToList();

                if (inventoryItemRecords != null)
                {
                    // Need to do some make shift mapping here
                    inventoryItemRecords.ForEach(item =>
                    {
                        // Check if it exist by the natural key (part+location+serial+attributes, and update quantity, otherwise, add it as a new item
                        var itemIndex = inventoryItems.FindIndex(inventoryItemFind => inventoryItemFind.Part.PartId == item.Part.PartId
                            && inventoryItemFind.InventoryLocation.InventoryLocationId == item.InventoryLocation.InventoryLocationId
                            && inventoryItemFind.SerialNumber == item.SerialNumber
                            && inventoryItemFind.InventoryItemAttributesConcatenated == item.InventoryItemAttributesConcatenated);

                        if(itemIndex >= 0)
                        {
                            var updatedItem = inventoryItems[itemIndex];
                            updatedItem.Quantity += item.Quantity;
                            inventoryItems[itemIndex] = updatedItem;
                        }
                        else
                        {
                            var inventoryItem = new InventoryItem()
                            {
                                InventoryItemId = item.InventoryItemId,
                                InventoryItemType = mapper.Map<Model.InventoryItemType, InventoryItemType>(item.InventoryItemType),
                                InventoryLocation = mapper.Map<Model.InventoryLocation, InventoryLocation>(item.InventoryLocation),
                                Part = mapper.Map<Model.Part, Part>(item.Part),
                                Supplier = mapper.Map<Model.Supplier, Supplier>(item.Supplier),
                                SerialNumber = item.SerialNumber,
                                Quantity = item.Quantity,
                                LastModifiedBy = item.LastModifiedBy,
                                LastModifiedTime = item.LastModifiedTime,
                                LastModifiedTimeUtc = item.LastModifiedTimeUtc,
                                InventoryItemAttributes = mapper.Map<List<Model.InventoryItemAttribute>, List<InventoryItemAttribute>>(item.InventoryItemAttributes.ToList()),
                                InventoryItemAttributesConcatenated = item.InventoryItemAttributesConcatenated
                            };

                            inventoryItems.Add(inventoryItem);
                        }
                        
                    });
                }
            }

            return inventoryItems;
        }

        /// <summary>
        /// Get an inventory item by serial number.  This will be used in validation logic for part consumption and identifying the current location
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public InventoryItem GetInventoryItemBySerialNumber(string serialNumber)
        {
            InventoryItem inventoryItem = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemRecord = mesProductionContext.InventoryItems
                    .Where(inventoryItem => inventoryItem.SerialNumber == serialNumber)
                    .Include(inventoryItem => inventoryItem.InventoryItemType)
                    .Include(inventoryItem => inventoryItem.InventoryLocation)
                        .ThenInclude(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .Include(inventoryItem => inventoryItem.Part)
                        .ThenInclude(part => part.UnitOfMeasure)
                    .Include(inventoryItem => inventoryItem.Supplier)
                    .Include(inventoryItem => inventoryItem.InventoryItemAttributes)
                        .ThenInclude(inventoryItemAttribute => inventoryItemAttribute.ItemAttributeType)
                    .FirstOrDefault();

                if (inventoryItemRecord != null)
                {
                    inventoryItem = mapper.Map<Model.InventoryItem, InventoryItem>(inventoryItemRecord);
                }
            }

            return inventoryItem;
        }

        /// <summary>
        /// Get all inventory items by part and serial number.  
        /// </summary>
        /// <param name="partId"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public List<InventoryItem> GetInventoryItemsByPartAndSerialNumber(int partId, string serialNumber)
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemRecords = mesProductionContext.InventoryItems
                    .Where(inventoryItem => inventoryItem.SerialNumber == serialNumber
                        && inventoryItem.PartId == partId)
                    .Include(inventoryItem => inventoryItem.InventoryItemType)
                    .Include(inventoryItem => inventoryItem.InventoryLocation)
                        .ThenInclude(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .Include(inventoryItem => inventoryItem.Part)
                        .ThenInclude(part => part.UnitOfMeasure)
                    .Include(inventoryItem => inventoryItem.Supplier)
                    .Include(inventoryItem => inventoryItem.InventoryItemAttributes)
                    .ThenInclude(inventoryItemAttribute => inventoryItemAttribute.ItemAttributeType)
                    .ToList();

                if (inventoryItemRecords != null)
                {
                    inventoryItems = mapper.Map<List<Model.InventoryItem>, List<InventoryItem>>(inventoryItemRecords);
                }
            }

            return inventoryItems;
        }

        /// <summary>
        /// The natural key of an inventory item is part + serial + location.  This method will be used to prevent duplicate entries into the table
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="partRevision"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public InventoryItem GetInventoryItemByPartSerialAndLocation(string partNumber, string partRevision, string serialNumber, string locationName)
        {
            InventoryItem inventoryItem = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemRecord = mesProductionContext.InventoryItems
                    .Where(inventoryItem => inventoryItem.Part.PartNumber == partNumber
                       && inventoryItem.Part.PartRevision == partRevision
                       && inventoryItem.SerialNumber == serialNumber
                       && inventoryItem.InventoryLocation.Name == locationName)
                    .Include(inventoryItem => inventoryItem.InventoryItemType)
                    .Include(inventoryItem => inventoryItem.InventoryLocation)
                    .ThenInclude(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .Include(inventoryItem => inventoryItem.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .Include(inventoryItem => inventoryItem.Supplier)
                    .Include(inventoryItem => inventoryItem.InventoryItemAttributes)
                    .ThenInclude(inventoryItemAttribute => inventoryItemAttribute.ItemAttributeType)
                    .FirstOrDefault();

                if (inventoryItemRecord != null)
                {
                    inventoryItem = mapper.Map<Model.InventoryItem, InventoryItem>(inventoryItemRecord);
                }
            }

            return inventoryItem;
        }

        /// <summary>
        /// Add a new inventory item
        /// </summary>
        /// <param name="inventoryItem"></param>
        public void AddInventoryItem(InventoryItem inventoryItem)
        {
            var inventoryItemRecord = mapper.Map<Model.InventoryItem>(inventoryItem);

            if (inventoryItemRecord != null)
            {
                inventoryItemRecord.InventoryItemId = 0;
                inventoryItemRecord.LastModifiedTime = DateTime.Now;
                inventoryItemRecord.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                inventoryItemRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(inventoryItem.LastModifiedBy) ? inventoryItem.LastModifiedBy : null;

                // Setup all attributes for adding as well
                if (inventoryItemRecord.InventoryItemAttributes!= null && inventoryItemRecord.InventoryItemAttributes.Count > 0)
                {
                    inventoryItemRecord.InventoryItemAttributes.ToList().ForEach(inventoryItemAttribute =>
                    {
                        inventoryItemAttribute.InventoryItemAttributeId = 0;
                        inventoryItemAttribute.LastModifiedTime = DateTime.Now;
                        inventoryItemAttribute.LastModifiedTimeUtc = DateTime.UtcNow;

                        // Set the Last Modified By if present
                        inventoryItemAttribute.LastModifiedBy = !string.IsNullOrWhiteSpace(inventoryItemRecord.LastModifiedBy) ? inventoryItemRecord.LastModifiedBy : null;
                    });
                }

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.InventoryItems.AddRange(inventoryItemRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing inventory item.  Typically this is done to change its location, or change the quantity.
        /// </summary>
        /// <param name="inventoryItem"></param>
        public void UpdateInventoryItem(InventoryItem inventoryItem)
        {
            var existingInventoryItem = GetInventoryItemByInventoryItemId(inventoryItem.InventoryItemId);

            if (existingInventoryItem == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingInventoryItem.SerialNumber != inventoryItem.SerialNumber || existingInventoryItem.PartId != inventoryItem.Part.PartId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            // Only the location and quantity of an inventory item can change.  The rest of the properties are locked in upon creation (type, serialNumber, part)
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingInventoryItem.InventoryLocationId = inventoryItem.InventoryLocation.InventoryLocationId;
                existingInventoryItem.Quantity = inventoryItem.Quantity;
                existingInventoryItem.LastModifiedTime = DateTime.Now;
                existingInventoryItem.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingInventoryItem.LastModifiedBy = !string.IsNullOrWhiteSpace(inventoryItem.LastModifiedBy) ? inventoryItem.LastModifiedBy : existingInventoryItem.LastModifiedBy;

                mesProductionContext.Entry(existingInventoryItem).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        /// <summary>
        /// Delete an existing inventory item and its attributes. Typically this is done when all of an inventory item's quantity has been moved somewhere else.
        /// </summary>
        /// <param name="inventoryItem"></param>
        public void DeleteInventoryItem(long inventoryItemId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemToDelete = mesProductionContext.InventoryItems
                    .Where(inventoryItem => inventoryItem.InventoryItemId == inventoryItemId)
                    .Include(inventoryItem => inventoryItem.InventoryItemAttributes);

                if (inventoryItemToDelete != null)
                {
                    mesProductionContext.RemoveRange(inventoryItemToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        #endregion

        #region Private Methods

        private Model.InventoryItem GetInventoryItemByInventoryItemId(long inventoryItemId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.InventoryItems
                    .Where(inventoryItem => inventoryItem.InventoryItemId == inventoryItemId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
