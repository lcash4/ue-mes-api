using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;

namespace ue_mes_data.dbo
{
    public class OrderItemData : DbContextBase, IOrderItemData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.OrderItem, OrderItem>()
                .ReverseMap()
                    .ForMember(orderItemModel => orderItemModel.OrderId, opt => opt.MapFrom(orderItemEntity => orderItemEntity.Order.OrderId))
                    .ForMember(orderItemModel => orderItemModel.FinishedPartId, opt => opt.MapFrom(orderItemEntity => orderItemEntity.FinishedPart.PartId))
                    .ForMember(orderItemModel => orderItemModel.Order, opt => opt.Ignore())
                    .ForMember(orderItemModel => orderItemModel.FinishedPart, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.Order, Order>()
                .ForMember(orderEntity => orderEntity.OrderState, opt => opt.MapFrom(orderData => (int)orderData.OrderStateId))
                .ForMember(orderEntity => orderEntity.OrderItems, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.Customer, Customer>();
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.OrderItemAttribute, OrderItemAttribute>();
            mapperConfig.CreateMap<Model.ItemAttributeType, ItemAttributeType>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public OrderItemData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Constants

        private static ReadOnlyCollection<OrderStateEnum> activeOrderStates = new ReadOnlyCollection<OrderStateEnum>(new List<OrderStateEnum> { OrderStateEnum.Started, OrderStateEnum.Released });

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all active (order state = started\released) order items given the provided part IDs.
        /// This call will typically used in order management screens, such as starting orders
        /// </summary>
        /// <param name="partIds">list of part IDs to search</param>
        /// <returns></returns>
        public List<OrderItem> GetActiveOrderItemsForParts(List<int> partIds)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemRecords = mesProductionContext.OrderItems
                    .Where(orderItem => partIds.Contains(orderItem.FinishedPart.PartId)
                        && activeOrderStates.Contains((OrderStateEnum)orderItem.Order.OrderStateId))
                    .Include(orderItem => orderItem.Order)
                        .ThenInclude(order => order.Customer)
                    .Include(orderItem => orderItem.FinishedPart)
                    .Include(orderItem => orderItem.OrderItemAttributes)
                        .ThenInclude(orderItemAttribute => orderItemAttribute.ItemAttributeType)
                    .ToList();

                if (orderItemRecords != null)
                {
                    orderItems = mapper.Map<List<Model.OrderItem>, List<OrderItem>>(orderItemRecords);
                }
            }

            return orderItems;
        }

        #endregion
    }
}
