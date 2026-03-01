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
    public class OrderItemUnitConsumptionData : DbContextBase, IOrderItemUnitConsumptionData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.OrderItemUnitConsumption, OrderItemUnitConsumption>()
                .ReverseMap()
                    .ForMember(orderItemUnitConsumptionModel => orderItemUnitConsumptionModel.OrderItemUnitId, opt => opt.MapFrom(orderItemUnitConsumptionEntity => orderItemUnitConsumptionEntity.OrderItemUnit.OrderItemUnitId))
                    .ForMember(orderItemUnitConsumptionModel => orderItemUnitConsumptionModel.EquipmentId, opt => opt.MapFrom(orderItemUnitConsumptionEntity => orderItemUnitConsumptionEntity.Equipment.EquipmentId))
                    .ForMember(orderItemUnitConsumptionModel => orderItemUnitConsumptionModel.UserId, opt => opt.MapFrom(orderItemUnitConsumptionEntity => orderItemUnitConsumptionEntity.User.UserId))
                    .ForMember(orderItemUnitConsumptionModel => orderItemUnitConsumptionModel.BillOfProcessProcessWorkElementAttributeId, opt => opt.MapFrom(orderItemUnitConsumptionEntity => orderItemUnitConsumptionEntity.BillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId))
                    .ForMember(orderItemUnitConsumptionModel => orderItemUnitConsumptionModel.OrderItemUnit, opt => opt.Ignore())
                    .ForMember(orderItemUnitConsumptionModel => orderItemUnitConsumptionModel.Equipment, opt => opt.Ignore())
                    .ForMember(orderItemUnitConsumptionModel => orderItemUnitConsumptionModel.User, opt => opt.Ignore())
                    .ForMember(orderItemUnitConsumptionModel => orderItemUnitConsumptionModel.BillOfProcessProcessWorkElementAttribute, opt => opt.Ignore());
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

        public OrderItemUnitConsumptionData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method will pull existing consumption records for the given serial number.  
        /// It will only return those that have values and IsConsumed = true (IsConsumed = false will not be used in the client or validation logic as it is similar to not existing at all)
        /// </summary>
        /// <param name="serialNumber">The consumed serial number</param>
        /// <returns>Can return a list as a lot serial number can be consumed in many assemblies</returns>
        public List<OrderItemUnitConsumption> GetOrderItemUnitConsumptionsByConsumedSerialNumber(string serialNumber)
        {
            List<OrderItemUnitConsumption> orderItemUnitConsumptions = new List<OrderItemUnitConsumption>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitConsumptionRecords = mesProductionContext.OrderItemUnitConsumptions
                    .Where(orderItemUnitConsumption => orderItemUnitConsumption.ConsumedSerialNumber == serialNumber)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.OrderItemUnit)
                    .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                    .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.OrderItemUnit)
                    .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.Equipment)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.User)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.BillOfProcessProcessWorkElementAttribute)
                    .ThenInclude(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElement)
                    .ToList();

                if (orderItemUnitConsumptionRecords != null)
                {
                    orderItemUnitConsumptions = mapper.Map<List<Model.OrderItemUnitConsumption>, List<OrderItemUnitConsumption>>(orderItemUnitConsumptionRecords);
                }
            }

            return orderItemUnitConsumptions;
        }

        /// <summary>
        /// This method will pull existing consumption records for the given work element and serial number.  
        /// It will only return those that have values and IsConsumed = true (IsConsumed = false will not be used in the client)
        /// </summary>
        /// <param name="workElementId">The specific work element from the caller</param>
        /// <param name="serialNumber">The serial number of the assembly being worked on</param>
        /// <returns></returns>
        public List<OrderItemUnitConsumption> GetOrderItemUnitConsumptionByWorkElementAndSerialNumber(int workElementId, string serialNumber)
        {
            List<OrderItemUnitConsumption> orderItemUnitConsumptions = new List<OrderItemUnitConsumption>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitConsumptionRecords = mesProductionContext.OrderItemUnitConsumptions
                    .Where(orderItemUnitConsumption => orderItemUnitConsumption.OrderItemUnit.SerialNumber == serialNumber
                        && orderItemUnitConsumption.BillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementId == workElementId)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.OrderItemUnit)
                    .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                    .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.OrderItemUnit)
                    .ThenInclude(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.Equipment)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.User)
                    .Include(orderItemUnitConsumption => orderItemUnitConsumption.BillOfProcessProcessWorkElementAttribute)
                    .ThenInclude(billOfProcessProcessWorkElementAttribute => billOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElement)
                    .ToList();

                if (orderItemUnitConsumptionRecords != null)
                {
                    orderItemUnitConsumptions = mapper.Map<List<Model.OrderItemUnitConsumption>, List<OrderItemUnitConsumption>>(orderItemUnitConsumptionRecords);
                }
            }

            return orderItemUnitConsumptions;
        }

        /// <summary>
        /// Add a range of new order item unit consumption values
        /// </summary>
        /// <param name="orderItemUnitConsumptions"></param>
        public void AddOrderItemUnitConsumptions(List<OrderItemUnitConsumption> orderItemUnitConsumptions)
        {
            List<Model.OrderItemUnitConsumption> orderItemUnitConsumptionRecords = new List<Model.OrderItemUnitConsumption>();

            orderItemUnitConsumptions.ForEach(orderItemUnitConsumption =>
            {
                var orderItemUnitConsumptionRecord = mapper.Map<Model.OrderItemUnitConsumption>(orderItemUnitConsumption);
                orderItemUnitConsumptionRecord.OrderItemUnitConsumptionId = 0;
                orderItemUnitConsumptionRecord.IsConsumed = true;
                orderItemUnitConsumptionRecord.ConsumedDate = DateTime.Now;
                orderItemUnitConsumptionRecord.ConsumedDateUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                orderItemUnitConsumptionRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(orderItemUnitConsumption.LastModifiedBy) ? orderItemUnitConsumption.LastModifiedBy : null;
                
                orderItemUnitConsumptionRecords.Add(orderItemUnitConsumptionRecord);
            });

            if (orderItemUnitConsumptionRecords != null && orderItemUnitConsumptionRecords.Count > 0)
            {
                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.OrderItemUnitConsumptions.AddRange(orderItemUnitConsumptionRecords);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Remove an existing order item unit consumption value.
        /// Note: "Remove" will actually just update the existing record with IsConsumed = false and not actually delete the record.
        /// </summary>
        /// <param name="orderItemUnitConsumption"></param>
        public void RemoveOrderItemUnitConsumption(OrderItemUnitConsumption orderItemUnitConsumption)
        {
            var existingOrderItemUnitConsumption = GetOrderItemUnitConsumptionByOrderItemUnitConsumptionId(orderItemUnitConsumption.OrderItemUnitConsumptionId);

            if (existingOrderItemUnitConsumption == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingOrderItemUnitConsumption.OrderItemUnitId != orderItemUnitConsumption.OrderItemUnit.OrderItemUnitId
                || existingOrderItemUnitConsumption.BillOfProcessProcessWorkElementAttributeId != orderItemUnitConsumption.BillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId
                || existingOrderItemUnitConsumption.EquipmentId != orderItemUnitConsumption.Equipment.EquipmentId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingOrderItemUnitConsumption.IsConsumed = false;
                existingOrderItemUnitConsumption.UserId = orderItemUnitConsumption.User.UserId;
                existingOrderItemUnitConsumption.RemovedDate = DateTime.Now;
                existingOrderItemUnitConsumption.RemovedDateUtc = DateTime.UtcNow;
                existingOrderItemUnitConsumption.LastModifiedTime = DateTime.Now;
                existingOrderItemUnitConsumption.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                existingOrderItemUnitConsumption.LastModifiedBy = !string.IsNullOrWhiteSpace(orderItemUnitConsumption.LastModifiedBy) ? orderItemUnitConsumption.LastModifiedBy : existingOrderItemUnitConsumption.LastModifiedBy;

                mesProductionContext.Entry(existingOrderItemUnitConsumption).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        #endregion

        #region Private Methods

        private Model.OrderItemUnitConsumption GetOrderItemUnitConsumptionByOrderItemUnitConsumptionId(long orderItemUnitConsumptionId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.OrderItemUnitConsumptions
                    .Where(orderItemUnitConsumption => orderItemUnitConsumption.OrderItemUnitConsumptionId == orderItemUnitConsumptionId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
