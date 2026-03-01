using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Authorization;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class ItemAttributeTypeData : DbContextBase, IItemAttributeTypeData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.ItemAttributeType, ItemAttributeType>();
        });

        private IMapper mapper;

        #endregion

        #region Constructor

        public ItemAttributeTypeData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all <see cref="ItemAttributeType"/> entities.
        /// </summary>
        /// <returns></returns>
        public List<ItemAttributeType> GetAllItemAttributeTypes()
        {
            List<ItemAttributeType> itemAttributeTypes = new List<ItemAttributeType>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var itemAttributeTypeRecords = mesProductionContext.ItemAttributeTypes
                    .ToList();

                if (itemAttributeTypeRecords != null)
                {
                    itemAttributeTypes = mapper.Map<List<Model.ItemAttributeType>, List<ItemAttributeType>>(itemAttributeTypeRecords);
                }
            }

            return itemAttributeTypes;
        }

        #endregion
    }
}
