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
    public class WorkElementStatusData : DbContextBase, IWorkElementStatusData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig => {
            mapperConfig.CreateMap<Model.WorkElementStatus, WorkElementStatus>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public WorkElementStatusData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Simply return all work element statuses for use in a client list or dropdown, or business logic
        /// </summary>
        /// <returns></returns>
        public List<WorkElementStatus> GetWorkElementStatuses()
        {
            List<WorkElementStatus> workElementStatuses = new List<WorkElementStatus>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var workElementStatusRecords = mesProductionContext.WorkElementStatuses
                    .OrderBy(workElementStatus => workElementStatus.Name).ToList();

                if (workElementStatusRecords != null)
                {
                    workElementStatuses = mapper.Map<List<Model.WorkElementStatus>, List<WorkElementStatus>>(workElementStatusRecords);
                }
            }
            return workElementStatuses;
        }

        #endregion
    }
}
