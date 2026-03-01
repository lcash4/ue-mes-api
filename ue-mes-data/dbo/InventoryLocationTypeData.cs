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
    public class InventoryLocationTypeData : DbContextBase, IInventoryLocationTypeData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.InventoryLocationType, InventoryLocationType>();
        });

        private IMapper mapper;

        #endregion

        #region Constructor

        public InventoryLocationTypeData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all <see cref="InventoryLocationType"/> entities.
        /// </summary>
        /// <returns></returns>
        public List<InventoryLocationType> GetAllInventoryLocationTypes()
        {
            List<InventoryLocationType> inventoryLocationTypes = new List<InventoryLocationType>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var inventoryLocationTypeRecords = mesProductionContext.InventoryLocationTypes
                    .ToList();

                if (inventoryLocationTypeRecords != null)
                {
                    inventoryLocationTypes = mapper.Map<List<Model.InventoryLocationType>, List<InventoryLocationType>>(inventoryLocationTypeRecords);
                }
            }

            return inventoryLocationTypes;
        }

        #endregion
    }
}
