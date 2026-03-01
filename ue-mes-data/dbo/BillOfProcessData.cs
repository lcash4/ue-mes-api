using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class BillOfProcessData : DbContextBase, IBillOfProcessData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.BillOfProcess, BillOfProcess>()
                .ReverseMap()
                    .ForMember(billOfProcessModel => billOfProcessModel.PartId, opt => opt.MapFrom(billOfProcessEntity => billOfProcessEntity.Part.PartId))
                    .ForMember(billOfProcessModel => billOfProcessModel.Part, opt => opt.Ignore());

            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.UnitOfMeasure, UnitOfMeasure>();
            mapperConfig.CreateMap<Model.BillOfProcessProcess, BillOfProcessProcess>()
            .ForPath(billOfProcessProcessEntity => billOfProcessProcessEntity.BillOfProcess.BillOfProcessId, opt => opt.MapFrom(billOfProcessProcessRecord => billOfProcessProcessRecord.BillOfProcessId));
            mapperConfig.CreateMap<Model.Process, ue_mes_entities.Global.Process>();
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElement, BillOfProcessProcessWorkElement>();
            mapperConfig.CreateMap<Model.WorkElementType, WorkElementType>();
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElementAttribute, BillOfProcessProcessWorkElementAttribute>();
            mapperConfig.CreateMap<Model.WorkElementTypeAttribute, WorkElementTypeAttribute>();
            mapperConfig.CreateMap<Model.WorkElementTypeAttributeListItem, WorkElementTypeAttributeListItem>()
                .ForPath(workElementTypeAttributeListItemEntity => workElementTypeAttributeListItemEntity.WorkElementTypeAttribute.WorkElementTypeAttributeId, opt => opt.MapFrom(workElementTypeAttributeListItemRecord => workElementTypeAttributeListItemRecord.WorkElementTypeAttributeId));
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public BillOfProcessData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfProcess"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="billOfProcessId">Unique Bill of Process ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        public BillOfProcess GetBillOfProcessById(int billOfProcessId)
        {
            BillOfProcess billOfProcess = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessRecord = mesProductionContext.BillOfProcesses
                    .Where(billOfProcess => billOfProcess.BillOfProcessId == billOfProcessId)
                    .Include(billOfProcess => billOfProcess.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .Include(billOfProcess => billOfProcess.BillOfProcessProcesses.Where(billOfProcessProcess => billOfProcessProcess.IsActive).OrderBy(billOfProcessProcess => billOfProcessProcess.Sequence))
                    .ThenInclude(billOfProcessProcesses => billOfProcessProcesses.Process)
                    .Include(billOfProcess => billOfProcess.BillOfProcessProcesses.Where(billOfProcessProcess => billOfProcessProcess.IsActive).OrderBy(billOfProcessProcess => billOfProcessProcess.Sequence))
                    .ThenInclude(billOfProcessProcesses => billOfProcessProcesses.BillOfProcessProcessWorkElements.Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.IsActive).OrderBy(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.Sequence))
                    .ThenInclude(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.WorkElementType)
                    .Include(billOfProcess => billOfProcess.BillOfProcessProcesses.Where(billOfProcessProcess => billOfProcessProcess.IsActive).OrderBy(billOfProcessProcess => billOfProcessProcess.Sequence))
                    .ThenInclude(billOfProcessProcesses => billOfProcessProcesses.BillOfProcessProcessWorkElements.Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.IsActive).OrderBy(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.Sequence))
                    .ThenInclude(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementAttributes)
                    .ThenInclude(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.WorkElementTypeAttribute)
                    .ThenInclude(workElementTypeAttribute => workElementTypeAttribute.WorkElementTypeAttributeListItems)
                    .OrderByDescending(billOfProcess => billOfProcess.EffectiveStartDateUtc)
                    .FirstOrDefault();

                if (billOfProcessRecord != null)
                {
                    billOfProcess = mapper.Map<BillOfProcess>(billOfProcessRecord);
                }
            }

            return billOfProcess;
        }

        /// <summary>
        /// Returns a full <see cref="BillOfProcess"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="partId">Unique Part ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        public BillOfProcess GetBillOfProcessByPartId(int partId)
        {
            BillOfProcess billOfProcess = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessRecord = mesProductionContext.BillOfProcesses
                    .Where(billOfProcess => billOfProcess.PartId == partId)
                    .Include(billOfProcess => billOfProcess.Part)
                    .ThenInclude(part => part.UnitOfMeasure)
                    .Include(billOfProcess => billOfProcess.BillOfProcessProcesses.Where(billOfProcessProcess => billOfProcessProcess.IsActive).OrderBy(billOfProcessProcess => billOfProcessProcess.Sequence))
                    .ThenInclude(billOfProcessProcesses => billOfProcessProcesses.Process)
                    .Include(billOfProcess => billOfProcess.BillOfProcessProcesses.Where(billOfProcessProcess => billOfProcessProcess.IsActive).OrderBy(billOfProcessProcess => billOfProcessProcess.Sequence))
                    .ThenInclude(billOfProcessProcesses => billOfProcessProcesses.BillOfProcessProcessWorkElements.Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.IsActive).OrderBy(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.Sequence))
                    .ThenInclude(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.WorkElementType)
                    .Include(billOfProcess => billOfProcess.BillOfProcessProcesses.Where(billOfProcessProcess => billOfProcessProcess.IsActive).OrderBy(billOfProcessProcess => billOfProcessProcess.Sequence))
                    .ThenInclude(billOfProcessProcesses => billOfProcessProcesses.BillOfProcessProcessWorkElements.Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.IsActive).OrderBy(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.Sequence))
                    .ThenInclude(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementAttributes)
                    .ThenInclude(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.WorkElementTypeAttribute)
                    .ThenInclude(workElementTypeAttribute => workElementTypeAttribute.WorkElementTypeAttributeListItems)
                    .OrderByDescending(billOfProcess => billOfProcess.EffectiveStartDateUtc)
                    .FirstOrDefault();

                if (billOfProcessRecord != null)
                {
                    billOfProcess = mapper.Map<BillOfProcess>(billOfProcessRecord);
                }
            }

            return billOfProcess;
        }

        /// <summary>
        /// Returns a list of <see cref="BillOfProcess"/> entities with part child entities included.
        /// The starting process ID is used to find BOPs where the process sequence is 1 and matches the process.  This indicates an process where an order item unit can be started.
        /// </summary>
        /// <param name="processId">Unique Process ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        public List<BillOfProcess> GetBillOfProcessesByStartingProcessId(int processId)
        {
            List<BillOfProcess> billOfProcesses = new List<BillOfProcess>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessRecords = mesProductionContext.BillOfProcesses
                    .Where(billOfProcess => billOfProcess.EffectiveStartDateUtc < DateTime.UtcNow && (billOfProcess.EffectiveEndDateUtc == null || billOfProcess.EffectiveEndDateUtc > DateTime.UtcNow))
                    .Include(billOfProcess => billOfProcess.BillOfProcessProcesses.Where(billOfProcessProcess => billOfProcessProcess.ProcessId == processId && billOfProcessProcess.Sequence == 1))
                    .ThenInclude(billOfProcessProcesses => billOfProcessProcesses.Process)
                    .Include(billOfProcess => billOfProcess.Part)
                    .ToList();

                if (billOfProcessRecords != null)
                {
                    billOfProcesses = mapper.Map<List<Model.BillOfProcess>, List<BillOfProcess>>(billOfProcessRecords);
                }
            }

            return billOfProcesses;
        }

        /// <summary>
        /// Add a new bill of process
        /// </summary>
        /// <param name="billOfProcess"></param>
        public void AddBillOfProcess(BillOfProcess billOfProcess)
        {
            var billOfProcessRecord = mapper.Map<Model.BillOfProcess>(billOfProcess);
            if (billOfProcessRecord != null)
            {
                // With the model object mapped and validated, we can now set the UTC versions of the effective dates.
                billOfProcessRecord.EffectiveStartDate = billOfProcess.EffectiveStartDateUtc.ToLocalTime(); // The caller will only provide UTC time
                billOfProcessRecord.EffectiveEndDate = billOfProcess.EffectiveEndDateUtc?.ToLocalTime(); // The caller will only provide UTC time

                // Set the Last Modified By if present
                billOfProcessRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcess.LastModifiedBy) ? billOfProcess.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.BillOfProcesses.Add(billOfProcessRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing bill of process.  
        /// </summary>
        /// <param name="billOfProcess"></param>
        public void UpdateBillOfProcess(BillOfProcess billOfProcess)
        {
            var existingBillOfProcess = GetBillOfProcessByBillOfProcessId(billOfProcess.BillOfProcessId);

            if (existingBillOfProcess == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingBillOfProcess.Name != billOfProcess.Name || existingBillOfProcess.PartId != billOfProcess.Part.PartId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingBillOfProcess.Description = billOfProcess.Description;
                existingBillOfProcess.EffectiveStartDateUtc = billOfProcess.EffectiveStartDateUtc;
                existingBillOfProcess.EffectiveStartDate = billOfProcess.EffectiveStartDateUtc.ToLocalTime(); // The caller will only provide UTC time
                existingBillOfProcess.EffectiveEndDateUtc = billOfProcess.EffectiveEndDateUtc;
                existingBillOfProcess.EffectiveEndDate = billOfProcess.EffectiveEndDateUtc?.ToLocalTime(); // The caller will only provide UTC time
                existingBillOfProcess.LastModifiedTime = DateTime.Now;
                existingBillOfProcess.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingBillOfProcess.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcess.LastModifiedBy) ? billOfProcess.LastModifiedBy : existingBillOfProcess.LastModifiedBy;

                mesProductionContext.Entry(existingBillOfProcess).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to get the existing record for modification or deletion.
        /// </summary>
        /// <param name="billOfProcessId"></param>
        /// <returns></returns>
        private Model.BillOfProcess GetBillOfProcessByBillOfProcessId(int billOfProcessId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.BillOfProcesses
                    .Where(billOfProcess => billOfProcess.BillOfProcessId == billOfProcessId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
