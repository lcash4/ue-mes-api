using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;

namespace ue_mes_data.dbo
{
    public class OrderData : DbContextBase, IOrderData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.Order, Order>()
                .ForMember(orderEntity => orderEntity.OrderState, opt => opt.MapFrom(orderData => (int)orderData.OrderStateId))
                .ReverseMap()
                    .ForMember(orderModel => orderModel.OrderStateId, opt => opt.MapFrom(orderEntity => (byte)orderEntity.OrderState))
                    .ForMember(orderModel => orderModel.Customer, opt => opt.Ignore())
                    .ForMember(orderModel => orderModel.OrderState, opt => opt.Ignore());

            mapperConfig.CreateMap<Model.Customer, Customer>();
            mapperConfig.CreateMap<Model.OrderItem, OrderItem>();
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.OrderItemAttribute, OrderItemAttribute>();
            mapperConfig.CreateMap<Model.ItemAttributeType, ItemAttributeType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public OrderData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all orders that have a matching state to the list provided
        /// </summary>
        /// <param name="orderStates"></param>
        /// <returns></returns>
        public List<Order> GetOrdersByStates(List<OrderStateEnum> orderStates)
        {
            List<Order> orders = new List<Order>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderRecords = mesProductionContext.Orders
                    .Where(order => orderStates.Contains((OrderStateEnum)order.OrderStateId))
                    .Include(order => order.Customer)
                    .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(order => order.OrderItems)
                    .ThenInclude(orderItem => orderItem.OrderItemAttributes)
                    .ThenInclude(orderItemAttribute => orderItemAttribute.ItemAttributeType)
                    .ToList();

                if (orderRecords != null)
                {
                    orders = mapper.Map<List<Model.Order>, List<Order>>(orderRecords);
                }
            }

            return orders;
        }
        #endregion
    }
}
