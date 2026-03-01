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
    public class BillOfProcessProcessWorkElementData : DbContextBase, IBillOfProcessProcessWorkElementData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElement, BillOfProcessProcessWorkElement>()
                .ReverseMap()
                    .ForMember(billOfProcessProcessWorkElementModel => billOfProcessProcessWorkElementModel.BillOfProcessProcessId, opt => opt.MapFrom(billOfProcessProcessWorkElementEntity => billOfProcessProcessWorkElementEntity.BillOfProcessProcess.BillOfProcessProcessId))
                    .ForMember(billOfProcessProcessWorkElementModel => billOfProcessProcessWorkElementModel.WorkElementTypeId, opt => opt.MapFrom(billOfProcessProcessWorkElementEntity => billOfProcessProcessWorkElementEntity.WorkElementType.WorkElementTypeId))
                    .ForMember(billOfProcessProcessWorkElementModel => billOfProcessProcessWorkElementModel.BillOfProcessProcess, opt => opt.Ignore())
                    .ForMember(billOfProcessProcessWorkElementModel => billOfProcessProcessWorkElementModel.WorkElementType, opt => opt.Ignore());

            mapperConfig.CreateMap<Model.WorkElementType, WorkElementType>();
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElementAttribute, BillOfProcessProcessWorkElementAttribute>()
            .ForPath(billOfProcessProcessWorkElementAttributeEntity => billOfProcessProcessWorkElementAttributeEntity.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId, opt => opt.MapFrom(billOfProcessProcessWorkElementAttributeRecord => billOfProcessProcessWorkElementAttributeRecord.BillOfProcessProcessWorkElementId))
            .ReverseMap()
                //.ForPath(billOfProcessProcessWorkElementAttributeRecord => billOfProcessProcessWorkElementAttributeRecord.BillOfProcessProcessWorkElementId, opt => opt.MapFrom(billOfProcessProcessWorkElementAttributeEntity => billOfProcessProcessWorkElementAttributeEntity.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId))
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

        public BillOfProcessProcessWorkElementData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcessWorkElement"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementId">Unique Bill of Process Process Work Element ID for the <see cref="BillOfProcessProcessWorkElement"/></param>
        /// <returns></returns>
        public BillOfProcessProcessWorkElement GetBillOfProcessProcessWorkElementById(int billOfProcessProcessWorkElementId)
        {
            BillOfProcessProcessWorkElement billOfProcessProcessWorkElement = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessWorkElementRecord = mesProductionContext.BillOfProcessProcessWorkElements
                    .Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId == billOfProcessProcessWorkElementId
                        && billOfProcessProcessWorkElement.IsActive)
                    .Include(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.WorkElementType)
                    .Include(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementAttributes)
                    .ThenInclude(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.WorkElementTypeAttribute)
                    .ThenInclude(workElementTypeAttribute => workElementTypeAttribute.WorkElementTypeAttributeListItems)
                    .FirstOrDefault();

                if (billOfProcessProcessWorkElementRecord != null)
                {
                    billOfProcessProcessWorkElement = mapper.Map<BillOfProcessProcessWorkElement>(billOfProcessProcessWorkElementRecord);
                }
            }

            return billOfProcessProcessWorkElement;
        }

        /// <summary>
        /// Returns a list of <see cref="BillOfProcessProcessWorkElement"/> entities with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessId">Unique Bill of Process Process ID for the <see cref="BillOfProcessProcessWorkElement"/> entities to return</param>
        /// <returns></returns>
        public List<BillOfProcessProcessWorkElement> GetBillOfProcessProcessWorkElementsByBillOfProcessProcessId(int billOfProcessProcessId)
        {
            List<BillOfProcessProcessWorkElement> billOfProcessProcessWorkElements = new List<BillOfProcessProcessWorkElement>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessWorkElementRecords = mesProductionContext.BillOfProcessProcessWorkElements
                    .Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessId == billOfProcessProcessId
                        && billOfProcessProcessWorkElement.IsActive)
                    .Include(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.WorkElementType)
                    .Include(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementAttributes)
                    .ThenInclude(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.WorkElementTypeAttribute)
                    .ThenInclude(workElementTypeAttribute => workElementTypeAttribute.WorkElementTypeAttributeListItems)
                    .OrderBy(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.Sequence)
                    .ToList();

                if (billOfProcessProcessWorkElementRecords != null)
                {
                    billOfProcessProcessWorkElements = mapper.Map<List<Model.BillOfProcessProcessWorkElement>, List<BillOfProcessProcessWorkElement>>(billOfProcessProcessWorkElementRecords);
                }
            }

            return billOfProcessProcessWorkElements;
        }

        /// <summary>
        /// Add a new bill of process process work element
        /// </summary>
        /// <param name="billOfProcessProcessWorkElement"></param>
        public void AddBillOfProcessProcessWorkElement(BillOfProcessProcessWorkElement billOfProcessProcessWorkElement)
        {
            var billOfProcessProcessWorkElementRecord = mapper.Map<Model.BillOfProcessProcessWorkElement>(billOfProcessProcessWorkElement);
            if (billOfProcessProcessWorkElementRecord != null)
            {
                // Set the Last Modified By if present
                billOfProcessProcessWorkElementRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcessProcessWorkElement.LastModifiedBy) ? billOfProcessProcessWorkElement.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.BillOfProcessProcessWorkElements.Add(billOfProcessProcessWorkElementRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Add a range of new bill of process process work elements
        /// </summary>
        /// <param name="billOfProcessProcessWorkElements"></param>
        public void AddBillOfProcessProcessWorkElements(List<BillOfProcessProcessWorkElement> billOfProcessProcessWorkElements)
        {
            List<Model.BillOfProcessProcessWorkElement> billOfProcessProcessWorkElementRecords = new List<Model.BillOfProcessProcessWorkElement>();

            billOfProcessProcessWorkElements.ForEach(billOfProcessProcessWorkElement =>
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

                billOfProcessProcessWorkElementRecords.Add(billOfProcessProcessWorkElementRecord);
            });

            if (billOfProcessProcessWorkElementRecords != null && billOfProcessProcessWorkElementRecords.Count > 0)
            {
                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.BillOfProcessProcessWorkElements.AddRange(billOfProcessProcessWorkElementRecords);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing bill of process process work element.  
        /// </summary>
        /// <param name="billOfProcessProcessWorkElement"></param>
        public void UpdateBillOfProcessProcessWorkElement(BillOfProcessProcessWorkElement billOfProcessProcessWorkElement)
        {
            var existingBillOfProcessProcessWorkElement = GetBillOfProcessProcessWorkElementByBillOfProcessProcessWorkElementId(billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId);

            if (existingBillOfProcessProcessWorkElement == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingBillOfProcessProcessWorkElement.BillOfProcessProcessId != billOfProcessProcessWorkElement.BillOfProcessProcess.BillOfProcessProcessId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                //1	Pre-scribe clean	1	1
                existingBillOfProcessProcessWorkElement.WorkElementTypeId = billOfProcessProcessWorkElement.WorkElementType.WorkElementTypeId;
                existingBillOfProcessProcessWorkElement.Name = billOfProcessProcessWorkElement.Name;
                existingBillOfProcessProcessWorkElement.Sequence = billOfProcessProcessWorkElement.Sequence;
                existingBillOfProcessProcessWorkElement.IsRequired = billOfProcessProcessWorkElement.IsRequired;
                existingBillOfProcessProcessWorkElement.IsActive = billOfProcessProcessWorkElement.IsActive;
                existingBillOfProcessProcessWorkElement.LastModifiedTime = DateTime.Now;
                existingBillOfProcessProcessWorkElement.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingBillOfProcessProcessWorkElement.LastModifiedBy = !string.IsNullOrWhiteSpace(billOfProcessProcessWorkElement.LastModifiedBy) ? billOfProcessProcessWorkElement.LastModifiedBy : existingBillOfProcessProcessWorkElement.LastModifiedBy;

                mesProductionContext.Entry(existingBillOfProcessProcessWorkElement).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        /// <summary>
        /// Will delete all <see cref="BillOfProcessProcessWorkElement"/> records that match the incoming BOP Process Id
        /// </summary>
        /// <param name="billOfProcessProcessId"></param>
        public void DeleteBillOfProcessProcessWorkElements(int billOfProcessProcessId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessWorkElementRecordsToDelete = mesProductionContext.BillOfProcessProcessWorkElements
                    .Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessId == billOfProcessProcessId
                        && billOfProcessProcessWorkElement.IsActive)
                    .Include(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementAttributes);

                if (billOfProcessProcessWorkElementRecordsToDelete != null)
                {
                    mesProductionContext.RemoveRange(billOfProcessProcessWorkElementRecordsToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Will delete all <see cref="BillOfProcessProcessWorkElement"/> records that match the incoming BOP Process Work Element Ids
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementIds"></param>
        public void DeleteBillOfProcessProcessWorkElementsById(List<int> billOfProcessProcessWorkElementIds)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessWorkElementRecordsToDelete = mesProductionContext.BillOfProcessProcessWorkElements
                    .Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElementIds.Contains(billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId)
                        && billOfProcessProcessWorkElement.IsActive)
                    .Include(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementAttributes);

                if (billOfProcessProcessWorkElementRecordsToDelete != null)
                {
                    mesProductionContext.RemoveRange(billOfProcessProcessWorkElementRecordsToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Will set all <see cref="BillOfProcessProcessWorkElement"/> records that match the incoming BOP Process Work Element Ids to IsActive = false
        /// They have to be deactivated instead of deleted because they have production data associated with them.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementIds"></param>
        public void DeactivateBillOfProcessProcessWorkElementsById(List<int> billOfProcessProcessWorkElementIds)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var billOfProcessProcessWorkElementRecordsToDeactivate = mesProductionContext.BillOfProcessProcessWorkElements
                    .Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElementIds.Contains(billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId)
                        && billOfProcessProcessWorkElement.IsActive);

                if (billOfProcessProcessWorkElementRecordsToDeactivate != null)
                {
                    billOfProcessProcessWorkElementRecordsToDeactivate.ToList().ForEach(billOfProcessProcessWorkElementRecordToDeactivate =>
                    {
                        billOfProcessProcessWorkElementRecordToDeactivate.IsActive = false;
                    });

                    mesProductionContext.UpdateRange(billOfProcessProcessWorkElementRecordsToDeactivate);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to get the existing record for modification or deletion.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementId"></param>
        /// <returns></returns>
        private Model.BillOfProcessProcessWorkElement GetBillOfProcessProcessWorkElementByBillOfProcessProcessWorkElementId(int billOfProcessProcessWorkElementId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.BillOfProcessProcessWorkElements
                    .Where(billOfProcessProcessWorkElement => billOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId == billOfProcessProcessWorkElementId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
