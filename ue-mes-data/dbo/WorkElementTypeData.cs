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
    public class WorkElementTypeData : DbContextBase, IWorkElementTypeData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig => {
            mapperConfig.CreateMap<Model.WorkElementType, WorkElementType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public WorkElementTypeData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Simply return all work element types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<WorkElementType> GetWorkElementTypes()
        {
            List<WorkElementType> workElementTypes = new List<WorkElementType>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var workElementTypeRecords = mesProductionContext.WorkElementTypes
                    .OrderBy(workElementType => workElementType.Name).ToList();

                if (workElementTypeRecords != null)
                {
                    workElementTypes = mapper.Map<List<Model.WorkElementType>, List<WorkElementType>>(workElementTypeRecords);
                }
            }
            return workElementTypes;
        }

        #endregion
    }
}
