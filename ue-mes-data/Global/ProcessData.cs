using AutoMapper;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;

namespace ue_mes_data.Global
{
    public class ProcessData : DbContextBase, IProcessData
    {
        #region AutoMapper Config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig => {
            mapperConfig.CreateMap<Model.Process, Process>();
        });

        private IMapper mapper;

        #endregion

        #region Constructor

        public ProcessData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return all production processes for the factory
        /// </summary>
        /// <returns></returns>
        public List<Process> GetProcesses()
        {
            List<Process> processes = new List<Process>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var processRecords = mesProductionContext.Processes
                    .OrderBy(process => process.Number)
                    .ToList();

                if (processRecords != null)
                {
                    processes = mapper.Map<List<Model.Process>, List<Process>>(processRecords);
                }
            }

            return processes;
        }

        /// <summary>
        /// Return all production processes that are not already included in the provided bill of process
        /// </summary>
        /// <param name="localSiteName">The local site to filter to.  Not all processes exist in every site.</param>
        /// <param name="billOfProcessId">Bill of Process ID to filter</param>
        /// <returns></returns>
        public List<Process> GetProcessesNotInBillOfProcessForLocalSite(string localSiteName, int billOfProcessId)
        {
            List<Process> processes = new List<Process>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var processesInBillOfProcess = mesProductionContext.BillOfProcessProcesses
                    .Where(billOfProcessProcess => billOfProcessProcess.BillOfProcessId == billOfProcessId)
                    .Select(billOfProcessProcess => billOfProcessProcess.ProcessId);

                var localSiteEquipmentProcessIds = mesProductionContext.Equipment
                    .Where(equipment => equipment.Line.Unit.Plant.Site.DisplayName == localSiteName)
                    .Select(equipment => equipment.Process.ProcessId);

                var processRecords = mesProductionContext.Processes
                    .Where(process => !processesInBillOfProcess.Contains(process.ProcessId)
                        && localSiteEquipmentProcessIds.Contains(process.ProcessId))
                    .OrderBy(process => process.Number)
                    .ToList();

                if (processRecords != null)
                {
                    processes = mapper.Map<List<Model.Process>, List<Process>>(processRecords);
                }
            }

            return processes;
        }

        #endregion
    }
}
