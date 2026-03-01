using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.Authorization;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;

namespace ue_mes_data.dbo
{
    public class OrderItemUnitWorkElementHistoryData : DbContextBase, IOrderItemUnitWorkElementHistoryData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.OrderItemUnitWorkElementHistory, OrderItemUnitWorkElementHistory>()
                .ReverseMap()
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.OrderItemUnitId, opt => opt.MapFrom(orderItemUnitWorkElementHistoryEntity => orderItemUnitWorkElementHistoryEntity.OrderItemUnit.OrderItemUnitId))
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.BillOfProcessProcessWorkElementId, opt => opt.MapFrom(orderItemUnitWorkElementHistoryEntity => orderItemUnitWorkElementHistoryEntity.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId))
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.EquipmentId, opt => opt.MapFrom(orderItemUnitWorkElementHistoryEntity => orderItemUnitWorkElementHistoryEntity.Equipment.EquipmentId))
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.WorkElementStatusId, opt => opt.MapFrom(orderItemUnitWorkElementHistoryEntity => orderItemUnitWorkElementHistoryEntity.WorkElementStatus.WorkElementStatusId))
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.UserId, opt => opt.MapFrom(orderItemUnitWorkElementHistoryEntity => orderItemUnitWorkElementHistoryEntity.User.UserId))
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.OrderItemUnit, opt => opt.Ignore())
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.BillOfProcessProcessWorkElement, opt => opt.Ignore())
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.Equipment, opt => opt.Ignore())
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.WorkElementStatus, opt => opt.Ignore())
                    .ForMember(orderItemUnitWorkElementHistoryModel => orderItemUnitWorkElementHistoryModel.User, opt => opt.Ignore());

            mapperConfig.CreateMap<Model.OrderItemUnit, OrderItemUnit>();
            mapperConfig.CreateMap<Model.OrderItem, OrderItem>();
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.Order, Order>();
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElement, BillOfProcessProcessWorkElement>();
            mapperConfig.CreateMap<Model.Equipment, Equipment>();
            mapperConfig.CreateMap<Model.User, User>();
            mapperConfig.CreateMap<Model.WorkElementStatus, WorkElementStatus>();
            mapperConfig.CreateMap<Model.OrderItemUnitScrap, OrderItemUnitScrap>();
            mapperConfig.CreateMap<Model.Defect, Defect>();
            mapperConfig.CreateMap<Model.DefectCategory, DefectCategory>()
                .ForPath(defectCategoryEntity => defectCategoryEntity.Defects, opt => opt.Ignore());
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public OrderItemUnitWorkElementHistoryData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistoryId">Unique ID for the <see cref="OrderItemUnitWorkElementHistory"/> record</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetOrderItemUnitWorkElementHistoryById(long orderItemUnitWorkElementHistoryId)
        {
            OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecord = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnitWorkElementHistoryId == orderItemUnitWorkElementHistoryId)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItemUnitScrap)
                        .ThenInclude(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.User)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.WorkElementStatus)
                    .FirstOrDefault();

                if (orderItemUnitWorkElementHistoryRecord != null)
                {
                    orderItemUnitWorkElementHistory = mapper.Map<OrderItemUnitWorkElementHistory>(orderItemUnitWorkElementHistoryRecord);
                }
            }

            return orderItemUnitWorkElementHistory;
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// </summary>
        /// <param name="equipmentId">Unique ID for the equipment to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentOrderItemUnitWorkElementHistoryByEquipmentId(int equipmentId)
        {
            OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecord = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.EquipmentId == equipmentId)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItemUnitScrap)
                        .ThenInclude(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.User)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.WorkElementStatus)
                    .OrderByDescending(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.StartDateUtc)
                    .FirstOrDefault();

                if (orderItemUnitWorkElementHistoryRecord != null)
                {
                    orderItemUnitWorkElementHistory = mapper.Map<OrderItemUnitWorkElementHistory>(orderItemUnitWorkElementHistoryRecord);
                }
            }

            return orderItemUnitWorkElementHistory;
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// </summary>
        /// <param name="serialNumber">Serial number to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentOrderItemUnitWorkElementHistoryBySerialNumber(string serialNumber)
        {
            OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecord = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber == serialNumber)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItemUnitScrap)
                        .ThenInclude(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.User)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.WorkElementStatus)
                    .OrderByDescending(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.StartDateUtc)
                    .FirstOrDefault();

                if (orderItemUnitWorkElementHistoryRecord != null)
                {
                    orderItemUnitWorkElementHistory = mapper.Map<OrderItemUnitWorkElementHistory>(orderItemUnitWorkElementHistoryRecord);
                }
            }

            return orderItemUnitWorkElementHistory;
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// Only return a value when the most recent record is "In Progress"
        /// </summary>
        /// <param name="equipmentId">Unique ID for the equipment to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentActiveOrderItemUnitWorkElementHistoryByEquipmentId(int equipmentId)
        {
            OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecord = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.EquipmentId == equipmentId
                        && orderItemUnitWorkElementHistory.WorkElementStatus.Name == "In Progress")
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItemUnitScrap)
                        .ThenInclude(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.User)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.WorkElementStatus)
                    .OrderByDescending(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.StartDateUtc)
                    .FirstOrDefault();

                if (orderItemUnitWorkElementHistoryRecord != null)
                {
                    orderItemUnitWorkElementHistory = mapper.Map<OrderItemUnitWorkElementHistory>(orderItemUnitWorkElementHistoryRecord);
                }
            }

            return orderItemUnitWorkElementHistory;
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// Only return a value when the most recent record is "In Progress"
        /// </summary>
        /// <param name="serialNumber">Serial number to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentActiveOrderItemUnitWorkElementHistoryBySerialNumber(string serialNumber)
        {
            OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecord = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber == serialNumber
                        && orderItemUnitWorkElementHistory.WorkElementStatus.Name == "In Progress")
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItemUnitScrap)
                        .ThenInclude(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.User)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.WorkElementStatus)
                    .OrderByDescending(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.StartDateUtc)
                    .FirstOrDefault();

                if (orderItemUnitWorkElementHistoryRecord != null)
                {
                    orderItemUnitWorkElementHistory = mapper.Map<OrderItemUnitWorkElementHistory>(orderItemUnitWorkElementHistoryRecord);
                }
            }

            return orderItemUnitWorkElementHistory;
        }

        /// <summary>
        /// Returns a <see cref="OrderItemUnitWorkElementHistory"/> entity only with no child entities included.
        /// The purpose of this method is to check if a work element has any production history associated with it.  
        /// If so, the logic for handling work element changes will be adjusted to handle these work elements.
        /// </summary>
        /// <param name="workElementId">Work element ID to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetAnyOrderItemUnitWorkElementHistoryByWorkElementId(int workElementId)
        {
            OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecord = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElementId == workElementId)
                    .FirstOrDefault();

                if (orderItemUnitWorkElementHistoryRecord != null)
                {
                    orderItemUnitWorkElementHistory = mapper.Map<OrderItemUnitWorkElementHistory>(orderItemUnitWorkElementHistoryRecord);
                }
            }

            return orderItemUnitWorkElementHistory;
        }

        /// <summary>
        /// Return the latest OrderItemUnitWorkElementHistory for an equipment by checking the equipment work element history
        /// </summary>
        /// <param name="equipmentName">Unique equipment name to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentOrderItemUnitByEquipment(string equipmentName)
        {
            OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecord = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment.Name == equipmentName)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItemUnitScrap)
                        .ThenInclude(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.User)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.WorkElementStatus)
                    .OrderByDescending(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.StartDateUtc)
                    .FirstOrDefault();

                if (orderItemUnitWorkElementHistoryRecord != null)
                {
                    orderItemUnitWorkElementHistory = mapper.Map<OrderItemUnitWorkElementHistory>(orderItemUnitWorkElementHistoryRecord);
                }
            }

            return orderItemUnitWorkElementHistory;
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// </summary>
        /// <param name="equipmentId">Unique ID for the equipment to search</param>
        /// <param name="serialNumber">Serial number to search</param>
        /// <returns></returns>
        public List<OrderItemUnitWorkElementHistory> GetOrderItemUnitWorkElementHistoryByEquipmentAndSerialNumber(int equipmentId, string serialNumber)
        {
            List<OrderItemUnitWorkElementHistory> orderItemUnitWorkElementHistories = new List<OrderItemUnitWorkElementHistory>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecords = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.EquipmentId == equipmentId
                        && orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber == serialNumber)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItemUnitScrap)
                        .ThenInclude(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.User)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.WorkElementStatus)
                    .OrderByDescending(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.StartDateUtc)
                    .ToList();

                if (orderItemUnitWorkElementHistoryRecords != null)
                {
                    orderItemUnitWorkElementHistories = mapper.Map<List<Model.OrderItemUnitWorkElementHistory>, List<OrderItemUnitWorkElementHistory>>(orderItemUnitWorkElementHistoryRecords);
                }
            }

            return orderItemUnitWorkElementHistories;
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// The where clause is looking for the natural key of the table.  If we get a result from this query, we should NOT be adding a new record, as it would be a duplicate
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory">The incoming <see cref="OrderItemUnitWorkElementHistory"/> record to complete the query</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetOrderItemUnitWorkElementHistoryByOrderItemUnitAndWorkElementAndEquipmentWithInProgressStatus(OrderItemUnitWorkElementHistory inputOrderItemUnitWorkElementHistory)
        {
            OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecord = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnitId == inputOrderItemUnitWorkElementHistory.OrderItemUnit.OrderItemUnitId
                        && orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElementId == inputOrderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId
                        && orderItemUnitWorkElementHistory.EquipmentId == inputOrderItemUnitWorkElementHistory.Equipment.EquipmentId
                        && orderItemUnitWorkElementHistory.WorkElementStatus.Name == "In Progress")
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItemUnitScrap)
                        .ThenInclude(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.User)
                    .Include(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.WorkElementStatus)
                    .FirstOrDefault();

                if (orderItemUnitWorkElementHistoryRecord != null)
                {
                    orderItemUnitWorkElementHistory = mapper.Map<OrderItemUnitWorkElementHistory>(orderItemUnitWorkElementHistoryRecord);
                }
            }

            return orderItemUnitWorkElementHistory;
        }

        /// <summary>
        /// Add a new order item unit work element history record.  This will typically take place when a work element is getting started, or needs reworked
        /// </summary>
        /// <param name="billOfMaterial"></param>
        public void AddOrderItemUnitWorkElementHistory(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            var orderItemUnitWorkElementHistoryRecord = mapper.Map<Model.OrderItemUnitWorkElementHistory>(orderItemUnitWorkElementHistory);
            if (orderItemUnitWorkElementHistoryRecord != null)
            {
                // Set the Last Modified By if present
                orderItemUnitWorkElementHistoryRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(orderItemUnitWorkElementHistory.LastModifiedBy) ? orderItemUnitWorkElementHistory.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.OrderItemUnitWorkElementHistories.Add(orderItemUnitWorkElementHistoryRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing order item unit work element history.  
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        public void UpdateOrderItemUnitWorkElementHistory(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            // Pull the existing record in the database so that we can compare properties for updates
            var existingOrderItemUnitWorkElementHistory = GetOrderItemUnitWorkElementHistoryByOrderItemUnitWorkElementHistoryId(orderItemUnitWorkElementHistory.OrderItemUnitWorkElementHistoryId);

            if (existingOrderItemUnitWorkElementHistory == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingOrderItemUnitWorkElementHistory.OrderItemUnitId != orderItemUnitWorkElementHistory.OrderItemUnit.OrderItemUnitId
                || existingOrderItemUnitWorkElementHistory.BillOfProcessProcessWorkElementId != orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId
                || existingOrderItemUnitWorkElementHistory.EquipmentId != orderItemUnitWorkElementHistory.Equipment.EquipmentId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingOrderItemUnitWorkElementHistory.WorkElementStatusId = orderItemUnitWorkElementHistory.WorkElementStatus.WorkElementStatusId;
                existingOrderItemUnitWorkElementHistory.UserId = orderItemUnitWorkElementHistory.User.UserId;
                existingOrderItemUnitWorkElementHistory.StartDate = orderItemUnitWorkElementHistory.StartDate;
                existingOrderItemUnitWorkElementHistory.StartDateUtc = orderItemUnitWorkElementHistory.StartDateUtc;
                existingOrderItemUnitWorkElementHistory.EndDate = orderItemUnitWorkElementHistory.EndDate;
                existingOrderItemUnitWorkElementHistory.EndDateUtc = orderItemUnitWorkElementHistory.EndDateUtc;
                existingOrderItemUnitWorkElementHistory.LastModifiedTime = DateTime.Now;
                existingOrderItemUnitWorkElementHistory.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingOrderItemUnitWorkElementHistory.LastModifiedBy = !string.IsNullOrWhiteSpace(orderItemUnitWorkElementHistory.LastModifiedBy) ? orderItemUnitWorkElementHistory.LastModifiedBy : existingOrderItemUnitWorkElementHistory.LastModifiedBy;

                mesProductionContext.Entry(existingOrderItemUnitWorkElementHistory).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        /// <summary>
        /// Update any existing order item unit work element history where status is "In Progress" and the work element ID is being replaced by a new value
        /// </summary>
        /// <param name="oldWorkElementId">Used to query any existing records</param>
        /// <param name="newWorkElementId">New BOP Work Element ID to apply to the existing records</param>
        public void UpdateOrderItemUnitWorkElementHistoryWorkElementIds(int oldWorkElementId, int newWorkElementId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitWorkElementHistoryRecordsToUpdate = mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElementId == oldWorkElementId
                        && (orderItemUnitWorkElementHistory.WorkElementStatus.Name == "In Progress" || orderItemUnitWorkElementHistory.WorkElementStatus.Name == "Paused"))
                    .ToList();

                if (orderItemUnitWorkElementHistoryRecordsToUpdate != null)
                {
                    orderItemUnitWorkElementHistoryRecordsToUpdate.ForEach(orderItemUnitWorkElementHistoryRecordToUpdate =>
                    {
                        orderItemUnitWorkElementHistoryRecordToUpdate.BillOfProcessProcessWorkElementId = newWorkElementId;
                    });

                    mesProductionContext.UpdateRange(orderItemUnitWorkElementHistoryRecordsToUpdate);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        #endregion

        #region Private Methods

        private Model.OrderItemUnitWorkElementHistory GetOrderItemUnitWorkElementHistoryByOrderItemUnitWorkElementHistoryId(long orderItemUnitWorkElementHistoryId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.OrderItemUnitWorkElementHistories
                    .Where(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.OrderItemUnitWorkElementHistoryId == orderItemUnitWorkElementHistoryId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
