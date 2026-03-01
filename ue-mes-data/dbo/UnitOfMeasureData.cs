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
    public class UnitOfMeasureData : DbContextBase, IUnitOfMeasureData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig => {
            mapperConfig.CreateMap<Model.UnitOfMeasure, UnitOfMeasure>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public UnitOfMeasureData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Simply return all units of measure for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<UnitOfMeasure> GetUnitsOfMeasure()
        {
            List<UnitOfMeasure> unitsOfMeasure = new List<UnitOfMeasure>();
            
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var unitOfMeasureRecords = mesProductionContext.UnitOfMeasures
                    .OrderBy(unitOfMeasure => unitOfMeasure.Name).ToList();

                if (unitOfMeasureRecords != null)
                {
                    unitsOfMeasure = mapper.Map<List<Model.UnitOfMeasure>, List<UnitOfMeasure>>(unitOfMeasureRecords); 
                }
            }
            return unitsOfMeasure;
        }

        #endregion
    }
}
