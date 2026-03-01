using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo
{
    public class BillOfProcessProcessData : DbContextBase, IBillOfProcessProcessData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.BillOfProcessProcess, BillOfProcessProcess>()
                // .ForPath(billOfProcessProcessEntity => billOfProcessProcessEntity.BillOfProcess.BillOfProcessId, opt => opt.MapFrom(billOfProcessProcessRecord => billOfProcessProcessRecord.BillOfProcessId))
                .ReverseMap()
                    .ForMember(billOfProcessProcessModel => billOfProcessProcessModel.BillOfProcessId, opt => opt.MapFrom(billOfProcessProcessEntity => billOfProcessProcessEntity.BillOfProcess.BillOfProcessId))
                    .ForMember(billOfProcessProcessModel => billOfProcessProcessModel.ProcessId, opt => opt.MapFrom(billOfProcessProcessEntity => billOfProcessProcessEntity.Process.ProcessId))
                    .ForMember(billOfProcessProcessModel => billOfProcessProcessModel.BillOfProcess, opt => opt.Ignore())
                    .ForMember(billOfProcessProcessModel => billOfProcessProcessModel.Process, opt => opt.Ignore());

            mapperConfig.CreateMap<Model.BillOfProcess, BillOfProcess>();
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.Process, ue_mes_entities.Global.Process>();
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElement, BillOfProcessProcessWorkElement>()
            .ForPath(billOfProcessProcessWorkElementEntity => billOfProcessProcessWorkElementEntity.BillOfProcessProcess.BillOfProcessProcessId, opt => opt.MapFrom(billOfProcessProcessWorkElementRecord => billOfProcessProcessWorkElementRecord.BillOfProcessProcessId))
                .ReverseMap()
                    .ForMember(billOfProcessProcessWorkElementModel => billOfProcessProcessWorkElementModel.BillOfProcessProcessId, opt => opt.MapFrom(billOfProcessProcessWorkElementEntity => billOfProcessProcessWorkElementEntity.BillOfProcessProcess.BillOfProcessProcessId))
                    .ForMember(billOfProcessProcessWorkElementModel => billOfProcessProcessWorkElementModel.WorkElementTypeId, opt => opt.MapFrom(billOfProcessProcessWorkElementEntity => billOfProcessProcessWorkElementEntity.WorkElementType.WorkElementTypeId))
                    .ForMember(billOfProcessProcessWorkElementModel => billOfProcessProcessWorkElementModel.BillOfProcessProcess, opt => opt.Ignore())
                    .ForMember(billOfProcessProcessWorkElementModel => billOfProcessProcessWorkElementModel.WorkElementType, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.WorkElementType, WorkElementType>();
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElementAttribute, BillOfProcessProcessWorkElementAttribute>()
                .ReverseMap()
                    .ForMember(billOfProcessProcessWorkElementAttributeModel => billOfProcessProcessWorkElementAttributeModel.BillOfProcessProcessWorkElementId, opt => opt.MapFrom(billOfProcessProcessWorkElementAttributeEntity => billOfProcessProcessWorkElementAttributeEntity.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId))
                    .ForMember(billOfProcessProcessWorkElementAttributeModel => billOfProcessProcessWorkElementAttributeModel.WorkElementTypeAttributeId, opt => opt.MapFrom(billOfProcessProcessWorkElementAttributeEntity => billOfProcessProcessWorkElementAttributeEntity.WorkElementTypeAttribute.WorkElementTypeAttributeId))
                    .ForMember(billOfProcessProcessWorkElementAttributeModel => billOfProcessProcessWorkElementAttributeModel.BillOfProcessProcessWorkElement, opt => opt.Ignore())
                    .ForMember(billOfProcessProcessWorkElementAttributeModel => billOfProcessProcessWorkElementAttributeModel.WorkElementTypeAttribute, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.WorkElementTypeAttribute, WorkElementTypeAttribute>();
            mapperConfig.CreateMap<Model.WorkElementTypeAttributeListItem, WorkElementTypeAttributeListItem>()
                .ForPath(workElementTypeAttributeListItemEntity => workElementTypeAttributeListItemEntity.WorkElementTypeAttribute.WorkElementTypeAttributeId, opt => opt.MapFrom(workElementTypeAttributeListItemRecord => workElementTypeAttributeListItemRecord.WorkElementTypeAttributeId));
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public BillOfProcessProcessData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcess"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessId">Unique Bill of Process Process ID for the <see cref="BillOfProcessProcess"/></param>
        /// <returns></returns>
        public BillOfProcessProcess GetBillOfProcessProcessById(int billOfProcessProcessId)
        {
            BillOfProcessProcess billOfProcessProcess = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessRecord = mesProductionContext.BillOfProcessProcesses
                    .Where(billOfProcessProcess => billOfProcessProcess.BillOfProcessProcessId == billOfProcessProcessId)
                    .Include(billOfProcessProcess => billOfProcessProcess.BillOfProcess)
                    .ThenInclude(billOfProcess => billOfProcess.Part)
                    .Include(billOfProcessProcesses => billOfProcessProcesses.Process)
                    .Include(billOfProcessProcesses => billOfProcessProcesses.BillOfProcessProcessWorkElements.Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.IsActive).OrderBy(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.Sequence))
                    .ThenInclude(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.WorkElementType)
                    .Include(billOfProcessProcesses => billOfProcessProcesses.BillOfProcessProcessWorkElements.Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.IsActive).OrderBy(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.Sequence))
                    .ThenInclude(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementAttributes)
                    .ThenInclude(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.WorkElementTypeAttribute)
                    .ThenInclude(workElementTypeAttribute => workElementTypeAttribute.WorkElementTypeAttributeListItems)
                    .FirstOrDefault();

                if (billOfProcessProcessRecord != null)
                {
                    billOfProcessProcess = mapper.Map<BillOfProcessProcess>(billOfProcessProcessRecord);
                }
            }

            return billOfProcessProcess;
        }

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcess"/> entity with child entities included.
        /// This method will primarily be used to validate adding a new Bill of Process Process and preventing duplicates
        /// </summary>
        /// <param name="billOfProcesId">Unique Bill of Process ID for the <see cref="BillOfProcessProcess"/></param>
        /// <param name="processId">Unique Process ID for the <see cref="BillOfProcessProcess"/></param>
        /// <returns></returns>
        public BillOfProcessProcess GetBillOfProcessProcessByBillOfProcessIdAndProcessId(int billOfProcesId, int processId)
        {
            BillOfProcessProcess billOfProcessProcess = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessRecord = mesProductionContext.BillOfProcessProcesses
                    .Where(billOfProcessProcess => billOfProcessProcess.BillOfProcessId == billOfProcesId
                        && billOfProcessProcess.ProcessId == processId)
                    .Include(billOfProcessProcess => billOfProcessProcess.BillOfProcess)
                    .ThenInclude(billOfProcess => billOfProcess.Part)
                    .Include(billOfProcessProcesses => billOfProcessProcesses.Process)
                    .FirstOrDefault();

                if (billOfProcessProcessRecord != null)
                {
                    billOfProcessProcess = mapper.Map<BillOfProcessProcess>(billOfProcessProcessRecord);
                }
            }

            return billOfProcessProcess;
        }

        /// <summary>
        /// Returns a list of <see cref="BillOfProcessProcess"/> entities with child entities included.
        /// </summary>
        /// <param name="billOfProcessId">Unique Bill of Process ID to search.</param>
        /// <returns></returns>
        public List<BillOfProcessProcess> GetBillOfProcessProcessesByBillOfProcessId(int billOfProcessId)
        {
            List<BillOfProcessProcess> billOfProcessProcesses = new List<BillOfProcessProcess>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessRecords = mesProductionContext.BillOfProcessProcesses
                    .Where(billOfProcessProcess => billOfProcessProcess.BillOfProcessId == billOfProcessId
                        && billOfProcessProcess.IsActive)
                    .Include(billOfProcessProcess => billOfProcessProcess.BillOfProcess)
                    .ThenInclude(billOfProcess => billOfProcess.Part)
                    .Include(billOfProcessProcesses => billOfProcessProcesses.Process)
                    .Include(billOfProcessProcesses => billOfProcessProcesses.BillOfProcessProcessWorkElements.Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.IsActive).OrderBy(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.Sequence))
                    .ThenInclude(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.WorkElementType)
                    .Include(billOfProcessProcesses => billOfProcessProcesses.BillOfProcessProcessWorkElements.Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.IsActive).OrderBy(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.Sequence))
                    .ThenInclude(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementAttributes)
                    .ThenInclude(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.WorkElementTypeAttribute)
                    .ThenInclude(workElementTypeAttribute => workElementTypeAttribute.WorkElementTypeAttributeListItems)
                    .OrderBy(billOfProcessProcess => billOfProcessProcess.Sequence)
                    .ToList();

                if (billOfProcessProcessRecords != null)
                {
                    billOfProcessProcesses = mapper.Map<List<Model.BillOfProcessProcess>, List<BillOfProcessProcess>>(billOfProcessProcessRecords);
                }
            }

            return billOfProcessProcesses;
        }

        /// <summary>
        /// Returns a list of entities with BOP Process IDs, their work elements and whether those elements have any order item unit history.
        /// </summary>
        /// <param name="billOfProcessId">Unique Bill of Process ID to search.</param>
        /// <returns></returns>
        public List<(int BillOfProcessId, int BillOfProcessProcessId, int ProcessId, int WorkElementId, bool WorkElementIsActive, int WorkElementHistoryCount)> GetBillOfProcessProcessesWithWorkElementHistoryByBillOfProcessId(int billOfProcessId)
        {
            List<(int BillOfProcessId, int BillOfProcessProcessId, int ProcessId, int WorkElementId, bool WorkElementIsActive, int WorkElementHistoryCount)> billOfProcessProcessesWithWorkElementHistoryCount = new List<(int BillOfProcessId, int BillOfProcessProcessId, int ProcessId, int WorkElementId, bool WorkElementIsActive, int WorkElementHistoryCount)>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                // Will look to convert this to Lambda later.  Could only get it to work in LINQ
                var query = from bopp in mesProductionContext.BillOfProcessProcesses
                            where bopp.BillOfProcessId == billOfProcessId && bopp.IsActive
                            join bopwe in mesProductionContext.BillOfProcessProcessWorkElements
                                    on bopp.BillOfProcessProcessId equals bopwe.BillOfProcessProcessId into grouping
                            from p in grouping.DefaultIfEmpty()
                                join oiuweh in mesProductionContext.OrderItemUnitWorkElementHistories
                                    on p.BillOfProcessProcessWorkElementId equals oiuweh.BillOfProcessProcessWorkElementId into grouping2
                            from q in grouping2.DefaultIfEmpty()
                                group new { bopp, p, q } by new { bopp.BillOfProcessId, bopp.BillOfProcessProcessId, bopp.ProcessId, p.BillOfProcessProcessWorkElementId, p.IsActive } into boppg
                                select new
                                {
                                    boppg.Key.BillOfProcessId,
                                    boppg.Key.BillOfProcessProcessId,
                                    boppg.Key.ProcessId,
                                    IsActive = boppg.Key.IsActive != null ? boppg.Key.IsActive != null : false,
                                    WorkElementId = boppg.Key.BillOfProcessProcessWorkElementId != null ? boppg.Key.BillOfProcessProcessWorkElementId : 0,
                                    WorkElementHistoryCount = boppg.Where(x => x.q.OrderItemUnitWorkElementHistoryId != null).Count()
                                };

                if (query != null)
                {
                    billOfProcessProcessesWithWorkElementHistoryCount = query.AsEnumerable().Select(result => (result.BillOfProcessId, result.BillOfProcessProcessId, result.ProcessId, result.WorkElementId, result.IsActive, result.WorkElementHistoryCount)).ToList();
                }
            }

            return billOfProcessProcessesWithWorkElementHistoryCount;
        }

        /// <summary>
        /// Add a new bill of process process
        /// </summary>
        /// <param name="billOfProcessProcess"></param>
        public void AddBillOfProcessProcess(BillOfProcessProcess billOfProcessProcess)
        {
            var billOfProcessProcessRecord = mapper.Map<Model.BillOfProcessProcess>(billOfProcessProcess);
            if (billOfProcessProcessRecord != null)
            {
                // Set the Last Modified By if present
                billOfProcessProcessRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcessProcess.LastModifiedBy) ? billOfProcessProcess.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.BillOfProcessProcesses.Add(billOfProcessProcessRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Add a range of new bill of process process
        /// </summary>
        /// <param name="billOfProcessProcesses"></param>
        public void AddBillOfProcessProcesses(List<BillOfProcessProcess> billOfProcessProcesses)
        {
            List<Model.BillOfProcessProcess> billOfProcessProcessRecords = new List<Model.BillOfProcessProcess>();

            billOfProcessProcesses.ForEach(billOfProcessProcess =>
            {
                var billOfProcessProcessRecord = mapper.Map<Model.BillOfProcessProcess>(billOfProcessProcess);
                billOfProcessProcessRecord.BillOfProcessProcessId = 0;
                
                // Set the Last Modified By if present
                billOfProcessProcessRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcessProcess.LastModifiedBy) ? billOfProcessProcess.LastModifiedBy : null;

                billOfProcessProcessRecords.Add(billOfProcessProcessRecord);
            });

            if (billOfProcessProcessRecords != null && billOfProcessProcessRecords.Count > 0)
            {
                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.BillOfProcessProcesses.AddRange(billOfProcessProcessRecords);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Add a range of new bill of process process
        /// </summary>
        /// <param name="billOfProcessProcesses"></param>
        public void AddBillOfProcessProcessesWithWorkElements(List<BillOfProcessProcess> billOfProcessProcesses)
        {
            List<Model.BillOfProcessProcess> billOfProcessProcessRecords = new List<Model.BillOfProcessProcess>();

            billOfProcessProcesses.ForEach(billOfProcessProcess =>
            {
                var billOfProcessProcessRecord = mapper.Map<Model.BillOfProcessProcess>(billOfProcessProcess);
                billOfProcessProcessRecord.BillOfProcessProcessId = 0;
                billOfProcessProcessRecord.IsActive = true;

                // Set the Last Modified By if present
                billOfProcessProcessRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcessProcess.LastModifiedBy) ? billOfProcessProcess.LastModifiedBy : null;

                if (billOfProcessProcessRecord.BillOfProcessProcessWorkElements != null && billOfProcessProcessRecord.BillOfProcessProcessWorkElements.Count > 0)
                {
                    billOfProcessProcessRecord.BillOfProcessProcessWorkElements.ToList().ForEach(billOfProcessProcessWorkElement =>
                    {
                        var billOfProcessProcessWorkElementRecord = mapper.Map<Model.BillOfProcessProcessWorkElement>(billOfProcessProcessWorkElement);

                        billOfProcessProcessWorkElementRecord.BillOfProcessProcessWorkElementId = 0;
                        billOfProcessProcessWorkElementRecord.IsActive = true;

                        // Set the Last Modified By if present
                        billOfProcessProcessWorkElementRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcessProcessWorkElement.LastModifiedBy) ? billOfProcessProcessWorkElement.LastModifiedBy : null;

                        if (billOfProcessProcessWorkElementRecord.BillOfProcessProcessWorkElementAttributes != null && billOfProcessProcessWorkElementRecord.BillOfProcessProcessWorkElementAttributes.Count > 0)
                        {
                            billOfProcessProcessWorkElementRecord.BillOfProcessProcessWorkElementAttributes.ToList().ForEach(billOfProcessProcessWorkElementAttribute =>
                            {
                                billOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId = 0;
                                billOfProcessProcessWorkElementAttribute.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcessProcessWorkElement.LastModifiedBy) ? billOfProcessProcessWorkElement.LastModifiedBy : null;
                            });
                        }
                    });
                }

                billOfProcessProcessRecords.Add(billOfProcessProcessRecord);
            });

            if (billOfProcessProcessRecords != null && billOfProcessProcessRecords.Count > 0)
            {
                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.BillOfProcessProcesses.AddRange(billOfProcessProcessRecords);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing bill of process process.  
        /// </summary>
        /// <param name="billOfProcessProcess"></param>
        public void UpdateBillOfProcessProcess(BillOfProcessProcess billOfProcessProcess)
        {
            var existingBillOfProcessProcess = GetBillOfProcessProcessByBillOfProcessProcessId(billOfProcessProcess.BillOfProcessProcessId);

            if (existingBillOfProcessProcess == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingBillOfProcessProcess.BillOfProcessId != billOfProcessProcess.BillOfProcess.BillOfProcessId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingBillOfProcessProcess.ProcessId = billOfProcessProcess.Process.ProcessId;
                existingBillOfProcessProcess.Sequence = billOfProcessProcess.Sequence;
                existingBillOfProcessProcess.IsActive = billOfProcessProcess.IsActive;
                existingBillOfProcessProcess.LastModifiedTime = DateTime.Now;
                existingBillOfProcessProcess.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingBillOfProcessProcess.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcessProcess.LastModifiedBy) ? billOfProcessProcess.LastModifiedBy : existingBillOfProcessProcess.LastModifiedBy;

                mesProductionContext.Entry(existingBillOfProcessProcess).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        /// <summary>
        /// Will delete all <see cref="BillOfProcessProcess"/> records that match the incoming BillOfProcessId
        /// </summary>
        /// <param name="billOfProcessId"></param>
        public void DeleteBillOfProcessProcesses(int billOfProcessId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessRecordsToDelete = mesProductionContext.BillOfProcessProcesses
                    .Where(billOfProcessProcess => billOfProcessProcess.BillOfProcessId == billOfProcessId)
                    .Include(billOfProcessProcess => billOfProcessProcess.BillOfProcessProcessWorkElements)
                    .ThenInclude(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementAttributes);

                if (billOfProcessRecordsToDelete != null)
                {
                    mesProductionContext.RemoveRange(billOfProcessRecordsToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }


        /// <summary>
        /// Will delete all <see cref="BillOfProcessProcess"/> records that match the incoming BOP Process Ids 
        /// </summary>
        /// <param name="billOfProcessProcessIds"></param>
        public void DeleteBillOfProcessProcessesById(List<int> billOfProcessProcessIds)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessRecordsToDelete = mesProductionContext.BillOfProcessProcesses
                    .Where(billOfProcessProcess => billOfProcessProcessIds.Contains(billOfProcessProcess.BillOfProcessProcessId)
                        && billOfProcessProcess.IsActive);

                if (billOfProcessRecordsToDelete != null)
                {
                    mesProductionContext.RemoveRange(billOfProcessRecordsToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Will set all <see cref="BillOfProcessProcess"/> records that match the incoming BOP Process Ids to IsActive = false
        /// They have to be deactivated instead of deleted because they have production data associated with them.
        /// </summary>
        /// <param name="billOfProcessProcessIds"></param>
        public void DeactivateBillOfProcessProcessesById(List<int> billOfProcessProcessIds)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessRecordsToDeactivate = mesProductionContext.BillOfProcessProcesses
                    .Where(billOfProcessProcess => billOfProcessProcessIds.Contains(billOfProcessProcess.BillOfProcessProcessId)
                        && billOfProcessProcess.IsActive);

                if (billOfProcessProcessRecordsToDeactivate != null)
                {
                    billOfProcessProcessRecordsToDeactivate.ToList().ForEach(billOfProcessProcessRecordToDeactivate =>
                    {
                        billOfProcessProcessRecordToDeactivate.IsActive = false;
                    });

                    mesProductionContext.UpdateRange(billOfProcessProcessRecordsToDeactivate);
                    mesProductionContext.SaveChanges();
                }
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to get the existing record for modification or deletion.
        /// </summary>
        /// <param name="billOfProcessProcessId"></param>
        /// <returns></returns>
        private Model.BillOfProcessProcess GetBillOfProcessProcessByBillOfProcessProcessId(int billOfProcessProcessId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.BillOfProcessProcesses
                    .Where(billOfProcessProcess => billOfProcessProcess.BillOfProcessProcessId == billOfProcessProcessId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
