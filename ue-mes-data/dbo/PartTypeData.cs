using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.Enums;

namespace ue_mes_data.dbo
{
    public class PartTypeData : DbContextBase, IPartTypeData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig => {
        mapperConfig.CreateMap<Model.PartType, PartTypeEnum>()
            .ConvertUsing(partTypeData => (PartTypeEnum)Convert.ToInt32(partTypeData.PartTypeId));
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public PartTypeData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Simply return all part types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<PartTypeEnum> GetPartTypes()
        {
            List<PartTypeEnum> partTypeEnums = new List<PartTypeEnum>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var partTypeRecords = mesProductionContext.PartTypes
                    .OrderBy(partType => partType.PartTypeId).ToList();

                if (partTypeRecords != null)
                {
                    partTypeEnums = mapper.Map<List<Model.PartType>, List<PartTypeEnum>>(partTypeRecords);
                }
            }
            return partTypeEnums;
        }

        #endregion
    }
}
