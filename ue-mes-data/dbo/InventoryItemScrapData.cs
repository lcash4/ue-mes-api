using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.Authorization;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class InventoryItemScrapData : DbContextBase, IInventoryItemScrapData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.InventoryItemScrap, InventoryItemScrap>()
                .ReverseMap()
                    .ForMember(inventoryItemScrapModel => inventoryItemScrapModel.InventoryItemId, opt => opt.MapFrom(inventoryItemScrapEntity => inventoryItemScrapEntity.InventoryItem.InventoryItemId))
                    .ForMember(inventoryItemScrapModel => inventoryItemScrapModel.DefectId, opt => opt.MapFrom(inventoryItemScrapEntity => inventoryItemScrapEntity.Defect.DefectId))
                    .ForMember(inventoryItemScrapModel => inventoryItemScrapModel.UserId, opt => opt.MapFrom(inventoryItemScrapEntity => inventoryItemScrapEntity.User.UserId))
                    .ForMember(inventoryItemScrapModel => inventoryItemScrapModel.InventoryItem, opt => opt.Ignore())
                    .ForMember(inventoryItemScrapModel => inventoryItemScrapModel.Defect, opt => opt.Ignore())
                    .ForMember(inventoryItemScrapModel => inventoryItemScrapModel.User, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.InventoryItemType, InventoryItemType>();
            mapperConfig.CreateMap<Model.InventoryLocation, InventoryLocation>();
            mapperConfig.CreateMap<Model.InventoryLocationType, InventoryLocationType>();
            mapperConfig.CreateMap<Model.Defect, Defect>();
            mapperConfig.CreateMap<Model.DefectCategory, DefectCategory>();
            mapperConfig.CreateMap<Model.User, User>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public InventoryItemScrapData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return an InventoryItemScrap object by its serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number to search</param>
        /// <returns></returns>
        public InventoryItemScrap GetInventoryItemScrapBySerialNumber(string serialNumber)
        {
            InventoryItemScrap inventoryItemScrap = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemScrapRecord = mesProductionContext.InventoryItemScraps
                    .Where(inventoryItemScrap => inventoryItemScrap.InventoryItem.SerialNumber == serialNumber)
                    .Include(inventoryItemScrap => inventoryItemScrap.InventoryItem)
                        .ThenInclude(inventoryItem => inventoryItem.InventoryItemType)
                    .Include(inventoryItemScrap => inventoryItemScrap.InventoryItem)
                        .ThenInclude(inventoryItem => inventoryItem.InventoryLocation)
                    .Include(inventoryItemScrap => inventoryItemScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(inventoryItemScrap => inventoryItemScrap.User)
                    .FirstOrDefault();

                if (inventoryItemScrapRecord != null)
                {
                    inventoryItemScrap = mapper.Map<Model.InventoryItemScrap, InventoryItemScrap>(inventoryItemScrapRecord);
                }
            }

            return inventoryItemScrap;
        }

        /// <summary>
        /// Add a new inventory item scrap.  Typically, this will be done from a quality screen
        /// </summary>
        /// <param name="inventoryItemScrap"></param>
        public void AddInventoryItemScrap(InventoryItemScrap inventoryItemScrap)
        {
            var inventoryItemScrapRecord = mapper.Map<Model.InventoryItemScrap>(inventoryItemScrap);
            if (inventoryItemScrapRecord != null)
            {
                // Set the Last Modified By if present
                inventoryItemScrapRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(inventoryItemScrap.LastModifiedBy) ? inventoryItemScrap.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.InventoryItemScraps.Add(inventoryItemScrapRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete an existing inventory item scrap record. Typically this is done when a scrapped item is reworked successfully.
        /// </summary>
        /// <param name="inventoryItemScrapId"></param>
        public void DeleteInventoryItemScrap(long inventoryItemScrapId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemScrapToDelete = mesProductionContext.OrderItemUnitScraps
                    .Where(inventoryItemScrap => inventoryItemScrap.OrderItemUnitScrapId == inventoryItemScrapId);

                if (inventoryItemScrapToDelete != null)
                {
                    mesProductionContext.RemoveRange(inventoryItemScrapToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        #endregion

        #region Private Methods

        private Model.InventoryItemScrap GetInventoryItemScrapByInventoryItemScrapId(long inventoryItemScrapId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.InventoryItemScraps
                    .Where(inventoryItemScrap => inventoryItemScrap.InventoryItemScrapId == inventoryItemScrapId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
