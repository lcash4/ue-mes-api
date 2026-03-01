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
    public class OrderItemUnitDataCollectionData : DbContextBase, IOrderItemUnitDataCollectionData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.OrderItemUnitDataCollection, OrderItemUnitDataCollection>()
                .ReverseMap()
                    .ForMember(orderItemUnitDataCollectionModel => orderItemUnitDataCollectionModel.OrderItemUnitId, opt => opt.MapFrom(orderItemUnitDataCollectionEntity => orderItemUnitDataCollectionEntity.OrderItemUnit.OrderItemUnitId))
                    .ForMember(orderItemUnitDataCollectionModel => orderItemUnitDataCollectionModel.EquipmentId, opt => opt.MapFrom(orderItemUnitDataCollectionEntity => orderItemUnitDataCollectionEntity.Equipment.EquipmentId))
                    .ForMember(orderItemUnitDataCollectionModel => orderItemUnitDataCollectionModel.UserId, opt => opt.MapFrom(orderItemUnitDataCollectionEntity => orderItemUnitDataCollectionEntity.User.UserId))
                    .ForMember(orderItemUnitDataCollectionModel => orderItemUnitDataCollectionModel.BillOfProcessProcessWorkElementAttributeId, opt => opt.MapFrom(orderItemUnitDataCollectionEntity => orderItemUnitDataCollectionEntity.BillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId))
                    .ForMember(orderItemUnitDataCollectionModel => orderItemUnitDataCollectionModel.OrderItemUnit, opt => opt.Ignore())
                    .ForMember(orderItemUnitDataCollectionModel => orderItemUnitDataCollectionModel.Equipment, opt => opt.Ignore())
                    .ForMember(orderItemUnitDataCollectionModel => orderItemUnitDataCollectionModel.User, opt => opt.Ignore())
                    .ForMember(orderItemUnitDataCollectionModel => orderItemUnitDataCollectionModel.BillOfProcessProcessWorkElementAttribute, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.OrderItemUnit, OrderItemUnit>();
            mapperConfig.CreateMap<Model.OrderItem, OrderItem>();
            mapperConfig.CreateMap<Model.Order, Order>();
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.Equipment, Equipment>();
            mapperConfig.CreateMap<Model.User, User>();
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElementAttribute, BillOfProcessProcessWorkElementAttribute>();
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElement, BillOfProcessProcessWorkElement>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public OrderItemUnitDataCollectionData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method will pull existing data collection records for the given work element and serial number.  
        /// It will only return those that have collected values and may not be the full list of data collection attributes
        /// </summary>
        /// <param name="workElementId">The specific work element from the caller</param>
        /// <param name="serialNumber">The serial number of the assembly being worked on</param>
        /// <returns></returns>
        public List<OrderItemUnitDataCollection> GetOrderItemUnitDataCollectionByWorkElementAndSerialNumber(int workElementId, string serialNumber)
        {
            List<OrderItemUnitDataCollection> orderItemUnitDataCollections = new List<OrderItemUnitDataCollection>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitDataCollectionRecords = mesProductionContext.OrderItemUnitDataCollections
                    .Where(orderItemUnitDataCollection => orderItemUnitDataCollection.OrderItemUnit.SerialNumber == serialNumber
                        && orderItemUnitDataCollection.BillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementId == workElementId)
                    .Include(orderItemUnitDataCollection => orderItemUnitDataCollection.OrderItemUnit)
                    .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                    .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitDataCollection => orderItemUnitDataCollection.OrderItemUnit)
                    .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitDataCollection => orderItemUnitDataCollection.Equipment)
                    .Include(orderItemUnitDataCollection => orderItemUnitDataCollection.User)
                    .Include(orderItemUnitDataCollection => orderItemUnitDataCollection.BillOfProcessProcessWorkElementAttribute)
                    .ThenInclude(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElement)
                    .ToList();

                if (orderItemUnitDataCollectionRecords != null)
                {
                    orderItemUnitDataCollections = mapper.Map<List<Model.OrderItemUnitDataCollection>, List<OrderItemUnitDataCollection>>(orderItemUnitDataCollectionRecords);
                }
            }

            return orderItemUnitDataCollections;
        }

        /// <summary>
        /// Add a range of new order item unit data collection values
        /// </summary>
        /// <param name="orderItemUnitDataCollections"></param>
        public void AddOrderItemUnitDataCollections(List<OrderItemUnitDataCollection> orderItemUnitDataCollections)
        {
            List<Model.OrderItemUnitDataCollection> orderItemUnitDataCollectionRecords = new List<Model.OrderItemUnitDataCollection>();

            orderItemUnitDataCollections.ForEach(orderItemUnitDataCollection =>
            {
                var orderItemUnitDataCollectionRecord = mapper.Map<Model.OrderItemUnitDataCollection>(orderItemUnitDataCollection);
                orderItemUnitDataCollectionRecord.OrderItemUnitDataCollectionId = 0;
                orderItemUnitDataCollectionRecord.CollectedDate = DateTime.Now;
                orderItemUnitDataCollectionRecord.CollectedDateUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                orderItemUnitDataCollectionRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(orderItemUnitDataCollection.LastModifiedBy) ? orderItemUnitDataCollection.LastModifiedBy : null;

                orderItemUnitDataCollectionRecords.Add(orderItemUnitDataCollectionRecord);
            });

            if (orderItemUnitDataCollectionRecords != null && orderItemUnitDataCollectionRecords.Count > 0)
            {
                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.OrderItemUnitDataCollections.AddRange(orderItemUnitDataCollectionRecords);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing order item unit data collection value.  
        /// </summary>
        /// <param name="orderItemUnitDataCollection"></param>
        public void UpdateOrderItemUnitDataCollection(OrderItemUnitDataCollection orderItemUnitDataCollection)
        {
            var existingOrderItemUnitDataCollection = GetOrderItemUnitDataCollectionByOrderItemUnitDataCollectionId(orderItemUnitDataCollection.OrderItemUnitDataCollectionId);

            if (existingOrderItemUnitDataCollection == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingOrderItemUnitDataCollection.OrderItemUnitId != orderItemUnitDataCollection.OrderItemUnit.OrderItemUnitId
                || existingOrderItemUnitDataCollection.BillOfProcessProcessWorkElementAttributeId != orderItemUnitDataCollection.BillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId
                || existingOrderItemUnitDataCollection.EquipmentId != orderItemUnitDataCollection.Equipment.EquipmentId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingOrderItemUnitDataCollection.CollectedValue = orderItemUnitDataCollection.CollectedValue;
                existingOrderItemUnitDataCollection.UserId = orderItemUnitDataCollection.User.UserId;
                existingOrderItemUnitDataCollection.CollectedDate = DateTime.Now;
                existingOrderItemUnitDataCollection.CollectedDateUtc = DateTime.UtcNow;
                existingOrderItemUnitDataCollection.LastModifiedTime = DateTime.Now;
                existingOrderItemUnitDataCollection.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingOrderItemUnitDataCollection.LastModifiedBy = !string.IsNullOrWhiteSpace(orderItemUnitDataCollection.LastModifiedBy) ? orderItemUnitDataCollection.LastModifiedBy : existingOrderItemUnitDataCollection.LastModifiedBy;

                mesProductionContext.Entry(existingOrderItemUnitDataCollection).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        #endregion

        #region Private Methods

        private Model.OrderItemUnitDataCollection GetOrderItemUnitDataCollectionByOrderItemUnitDataCollectionId(long orderItemUnitDataCollectionId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.OrderItemUnitDataCollections
                    .Where(orderItemUnitDataCollection => orderItemUnitDataCollection.OrderItemUnitDataCollectionId == orderItemUnitDataCollectionId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
