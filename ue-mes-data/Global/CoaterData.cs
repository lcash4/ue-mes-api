using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;

namespace ue_mes_data.Global
{
    public class CoaterData : DbContextBase, ICoaterData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.Coater, Coater>()
                .ReverseMap()
                    .ForMember(coaterModel => coaterModel.EquipmentId, opt => opt.MapFrom(coaterEntity => coaterEntity.Equipment.EquipmentId))
                    .ForMember(coaterModel => coaterModel.Equipment, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.Equipment, Equipment>();
            mapperConfig.CreateMap<Model.Line, Line>()
                .ForPath(lineModel => lineModel.Equipment, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.Process, Process>();
            mapperConfig.CreateMap<Model.Unit, Unit>();
            mapperConfig.CreateMap<Model.Plant, Plant>();
            mapperConfig.CreateMap<Model.Site, Site>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public CoaterData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the first active layer coater for the site name
        /// </summary>
        /// <param name="siteName">Unique site name to search</param>
        /// <returns></returns>
        public Coater GetActiveLayerCoaterBySiteName(string siteName)
        {
            Coater coater = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var coaterRecord = mesProductionContext.Coaters
                    .Where(coater => coater.Equipment.Process.Name.Contains("ACTIVE_LAYER_COATER")
                        && coater.Equipment.Line.Unit.Plant.Site.DisplayName == siteName)
                    .Include(coater => coater.Equipment)
                        .ThenInclude(equipment => equipment.Line)
                        .ThenInclude(line => line.Unit)
                        .ThenInclude(unit => unit.Plant)
                        .ThenInclude(plant => plant.Site)
                    .OrderBy(coater => coater.Equipment.Process.Number)
                    .FirstOrDefault();

                if (coaterRecord != null)
                {
                    coater = mapper.Map<Model.Coater, Coater>(coaterRecord);
                }
            }

            return coater;
        }

        #endregion
    }
}
