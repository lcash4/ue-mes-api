using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;

namespace ue_mes_data.Global
{
    public class EquipmentData : DbContextBase, IEquipmentData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.Equipment, Equipment>()
                .ReverseMap()
                    .ForMember(equipmentModel => equipmentModel.LineId, opt => opt.MapFrom(equipmentEntity => equipmentEntity.Line.LineId))
                    .ForMember(equipmentModel => equipmentModel.ProcessId, opt => opt.MapFrom(equipmentEntity => equipmentEntity.Process.ProcessId))
                    .ForMember(equipmentModel => equipmentModel.Line, opt => opt.Ignore())
                    .ForMember(equipmentModel => equipmentModel.ProcessId, opt => opt.Ignore());

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

        public EquipmentData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an equipment for a given name.
        /// </summary>
        /// <param name="equipmentName">Unique equipment name</param>
        /// <returns></returns>
        public Equipment GetEquipmentByName(string equipmentName)
        {
            Equipment equipment = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var equipmentRecord = mesProductionContext.Equipment
                    .Where(equipment => equipment.Name == equipmentName)
                    .Include(equipment => equipment.Process)
                    .Include(equipment => equipment.Line)
                        .ThenInclude(line => line.Unit)
                        .ThenInclude(unit => unit.Plant)
                        .ThenInclude(plant => plant.Site)
                    .FirstOrDefault();

                if (equipmentRecord != null)
                {
                    equipment = mapper.Map<Model.Equipment, Equipment>(equipmentRecord);
                }
            }

            return equipment;
        }

        /// <summary>
        /// Returns a list of equipment for a given site.
        /// </summary>
        /// <param name="siteDisplayName">Unique site display name</param>
        /// <returns></returns>
        public List<Equipment> GetEquipmentBySiteDisplayName(string siteDisplayName)
        {
            List<Equipment> equipment = new List<Equipment>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var equipmentRecords = mesProductionContext.Equipment
                    .Where(equipment => equipment.Line.Unit.Plant.Site.DisplayName == siteDisplayName)
                    .Include(equipment => equipment.Process)
                    .Include(equipment => equipment.Line)
                        .ThenInclude(line => line.Unit)
                        .ThenInclude(unit => unit.Plant)
                        .ThenInclude(plant => plant.Site)
                    .ToList();

                if (equipmentRecords != null)
                {
                    equipment = mapper.Map<List<Model.Equipment>, List<Equipment>>(equipmentRecords);
                }
            }

            return equipment;
        }

        /// <summary>
        /// Returns a list of equipment for a given site and process
        /// </summary>
        /// <param name="siteDisplayName">Unique site display name</param>
        /// <param name="processId">Unique process id</param>
        /// <returns></returns>
        public List<Equipment> GetEquipmentBySiteDisplayNameAndProcessId(string siteDisplayName, int processId)
        {
            List<Equipment> equipment = new List<Equipment>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var equipmentRecords = mesProductionContext.Equipment
                    .Where(equipment => equipment.Line.Unit.Plant.Site.DisplayName == siteDisplayName
                        && equipment.ProcessId == processId)
                    .Include(equipment => equipment.Process)
                    .Include(equipment => equipment.Line)
                        .ThenInclude(line => line.Unit)
                        .ThenInclude(unit => unit.Plant)
                        .ThenInclude(plant => plant.Site)
                    .ToList();

                if (equipmentRecords != null)
                {
                    equipment = mapper.Map<List<Model.Equipment>, List<Equipment>>(equipmentRecords);
                }
            }

            return equipment;
        }

        #endregion
    }
}
