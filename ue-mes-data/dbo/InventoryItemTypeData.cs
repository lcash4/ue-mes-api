using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class InventoryItemTypeData : DbContextBase, IInventoryItemTypeData
    {
        #region AutoMapper Config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.InventoryItemType, InventoryItemType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor
        public InventoryItemTypeData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Simply return all inventory item types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<InventoryItemType> GetInventoryItemTypes()
        {
            List<InventoryItemType> inventoryItemTypes = new List<InventoryItemType>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemTypeRecords = mesProductionContext.InventoryItemTypes
                    .Where(inventoryItemType => inventoryItemType.InventoryItemTypeId > 0)
                    .OrderBy(inventoryItemType => inventoryItemType.Name).ToList();

                if (inventoryItemTypeRecords != null)
                {
                    inventoryItemTypes = mapper.Map<List<Model.InventoryItemType>, List<InventoryItemType>>(inventoryItemTypeRecords);
                }
            }
            return inventoryItemTypes;
        }

        /// <summary>
        /// Return the inventory item type by name
        /// </summary>
        /// <param name="inventoryItemTypeName">inventory item type name to search</param>
        /// <returns></returns>
        public InventoryItemType GetInventoryItemTypeByName(string inventoryItemTypeName)
        {
            InventoryItemType inventoryItemType = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryItemTypeRecord = mesProductionContext.InventoryItemTypes
                    .Where(inventoryItemType => inventoryItemType.Name == inventoryItemTypeName)
                    .FirstOrDefault();

                if (inventoryItemTypeRecord != null)
                {
                    inventoryItemType = mapper.Map<Model.InventoryItemType, InventoryItemType>(inventoryItemTypeRecord);
                }
            }
            return inventoryItemType;
        }

        #endregion
    }
}
