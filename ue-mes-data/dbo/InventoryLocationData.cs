using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class InventoryLocationData : DbContextBase, IInventoryLocationData
    {
        #region AutoMapper Config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.InventoryLocation, InventoryLocation>()
                .ReverseMap()
                    .ForMember(inventoryLocationModel => inventoryLocationModel.InventoryLocationTypeId, opt => opt.MapFrom(inventoryLocationEntity => inventoryLocationEntity.InventoryLocationType.InventoryLocationTypeId))
                    .ForMember(inventoryLocationModel => inventoryLocationModel.InventoryLocationType, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.InventoryLocationType, InventoryLocationType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor
        public InventoryLocationData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Simply return an inventory location and its type by the supplied ID
        /// </summary>
        /// <returns></returns>
        public InventoryLocation GetInventoryLocationById(int locationId)
        {
            InventoryLocation inventoryLocation = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryLocationRecord = mesProductionContext.InventoryLocations
                    .Where(inventoryLocation => inventoryLocation.InventoryLocationId == locationId)
                    .Include(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .FirstOrDefault();

                if (inventoryLocationRecord != null)
                {
                    inventoryLocation = mapper.Map<Model.InventoryLocation, InventoryLocation>(inventoryLocationRecord);
                }
            }
            return inventoryLocation;
        }

        /// <summary>
        /// Simply return an inventory location and its type by the supplied name
        /// </summary>
        /// <returns></returns>
        public InventoryLocation GetInventoryLocationByName(string locationName)
        {
            InventoryLocation inventoryLocation = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryLocationRecord = mesProductionContext.InventoryLocations
                    .Where(inventoryLocation => inventoryLocation.Name == locationName)
                    .Include(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .FirstOrDefault();

                if (inventoryLocationRecord != null)
                {
                    inventoryLocation = mapper.Map<Model.InventoryLocation, InventoryLocation>(inventoryLocationRecord);
                }
            }
            return inventoryLocation;
        }

        /// <summary>
        /// Simply return all inventory locations and their type for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<InventoryLocation> GetInventoryLocations()
        {
            List<InventoryLocation> inventoryLocations = new List<InventoryLocation>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryLocationRecords = mesProductionContext.InventoryLocations
                    .Include(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .OrderBy(inventoryLocation => inventoryLocation.Name).ToList();

                if (inventoryLocationRecords != null)
                {
                    inventoryLocations = mapper.Map<List<Model.InventoryLocation>, List<InventoryLocation>>(inventoryLocationRecords);
                }
            }
            return inventoryLocations;
        }

        /// <summary>
        /// Return all location types with the given list of names
        /// </summary>
        /// <param name="locationTypes"></param>
        /// <returns></returns>
        public List<InventoryLocation> GetInventoryLocationsByLocationTypes(string[] locationTypes) 
        {
            List<InventoryLocation> inventoryLocations = new List<InventoryLocation>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryLocationRecords = mesProductionContext.InventoryLocations
                    .Where(inventoryLocation => locationTypes.Contains(inventoryLocation.InventoryLocationType.Name))
                    .Include(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .OrderBy(inventoryLocation => inventoryLocation.Name).ToList();

                if (inventoryLocationRecords != null)
                {
                    inventoryLocations = mapper.Map<List<Model.InventoryLocation>, List<InventoryLocation>>(inventoryLocationRecords);
                }
            }
            return inventoryLocations;
        }

        /// <summary>
        /// Returns a sum of inventory for each location.  This data will be used to show an aggregate view of the inventory, as well as prevent location type changes when the sum is greater than 0
        /// The result will be missing a lot of properties to simplify the group by clause.  Since this is just for a view, we can omit them and only keep the ones we need. 
        /// </summary>
        /// <returns></returns>
        public List<(InventoryLocation InventoryLocation, decimal Quantity)> GetInventorySummedByLocation()
        {
            List<(InventoryLocation InventoryLocation, decimal Quantity)> inventoryLocationsWithQuantity = new List<(InventoryLocation InventoryLocation, decimal Quantity)>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryLocationRecords = mesProductionContext.InventoryLocations
                    .Include(inventoryLocation => inventoryLocation.InventoryLocationType)
                    .Include(inventoryItem => inventoryItem.InventoryItems)
                    .GroupBy(x => new {
                        InventoryLocationId = x.InventoryLocationId,
                        InventoryLocationName = x.Name,
                        InventoryLocationTypeId = x.InventoryLocationType.InventoryLocationTypeId,
                        InventoryLocationTypeName = x.InventoryLocationType.Name,
                        ItemSum = x.InventoryItems.Sum(item => item.Quantity)
                    })
                    .Select(x => new {
                        InventoryLocation = new InventoryLocation()
                        {
                            InventoryLocationId = x.Key.InventoryLocationId,
                            Name = x.Key.InventoryLocationName,
                            InventoryLocationType = new InventoryLocationType()
                            {
                                InventoryLocationTypeId = x.Key.InventoryLocationTypeId,
                                Name = x.Key.InventoryLocationTypeName
                            }
                        },
                        Quantity = x.Key.ItemSum
                    }).ToList();

                if (inventoryLocationRecords != null)
                {
                    inventoryLocationsWithQuantity = inventoryLocationRecords.AsEnumerable().Select(result => (result.InventoryLocation, result.Quantity)).ToList();
                }
            }

            return inventoryLocationsWithQuantity;
        }

        /// <summary>
        /// Add a new inventory location
        /// </summary>
        /// <param name="inventoryLocation"></param>
        public void AddInventoryLocation(InventoryLocation inventoryLocation)
        {
            var inventoryLocationRecord = mapper.Map<Model.InventoryLocation>(inventoryLocation);
            if (inventoryLocationRecord != null)
            {
                inventoryLocationRecord.InventoryLocationId = 0;

                // Set the Last Modified By if present
                inventoryLocationRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(inventoryLocation.LastModifiedBy) ? inventoryLocation.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.InventoryLocations.Add(inventoryLocationRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing inventory location
        /// </summary>
        /// <param name="inventoryLocation"></param>
        public void UpdateInventoryLocation(InventoryLocation inventoryLocation)
        {
            var existingInventoryLocation = GetinventoryLocationByinventoryLocationId(inventoryLocation.InventoryLocationId);
            
            if (existingInventoryLocation == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            using (var inventoryItemData = new InventoryItemData())
            {
                var inventoryLocationIdList = new List<int>() { inventoryLocation.InventoryLocationId };
                var existingInventoryLocationInventory = inventoryItemData.GetInventoryItemsByLocationId(inventoryLocation.InventoryLocationId);

                if (existingInventoryLocation.InventoryLocationTypeId != inventoryLocation.InventoryLocationType.InventoryLocationTypeId && existingInventoryLocationInventory != null && existingInventoryLocationInventory.Count > 0)
                    throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");
            }

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingInventoryLocation.Name = inventoryLocation.Name;
                existingInventoryLocation.InventoryLocationTypeId = inventoryLocation.InventoryLocationType.InventoryLocationTypeId;
                existingInventoryLocation.LastModifiedTime = DateTime.Now;
                existingInventoryLocation.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingInventoryLocation.LastModifiedBy = !string.IsNullOrWhiteSpace(inventoryLocation.LastModifiedBy) ? inventoryLocation.LastModifiedBy : existingInventoryLocation.LastModifiedBy;

                mesProductionContext.Entry(existingInventoryLocation).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        /// <summary>
        /// Will delete all <see cref="InventoryLocation"/> records that match the incoming Ids
        /// </summary>
        /// <param name="inventoryLocationIds"></param>
        public void DeleteInventoryLocationsById(List<int> inventoryLocationIds)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryLocationRecordsToDelete = mesProductionContext.InventoryLocations
                    .Where(inventoryLocation => inventoryLocationIds.Contains(inventoryLocation.InventoryLocationId));

                if (inventoryLocationRecordsToDelete != null)
                {
                    mesProductionContext.RemoveRange(inventoryLocationRecordsToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to get the existing record for modification or deletion.
        /// </summary>
        /// <param name="inventoryLocationId"></param>
        /// <returns></returns>
        private Model.InventoryLocation GetinventoryLocationByinventoryLocationId(int inventoryLocationId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.InventoryLocations
                    .Where(inventoryLocation => inventoryLocation.InventoryLocationId == inventoryLocationId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
