using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.Authorization;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;

namespace ue_mes_data.dbo
{
    public class OrderItemUnitScrapData : DbContextBase, IOrderItemUnitScrapData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.OrderItemUnitScrap, OrderItemUnitScrap>()
                .ReverseMap()
                    .ForMember(orderItemUnitScrapModel => orderItemUnitScrapModel.OrderItemUnitId, opt => opt.MapFrom(orderItemUnitScrapEntity => orderItemUnitScrapEntity.OrderItemUnit.OrderItemUnitId))
                    .ForMember(orderItemUnitScrapModel => orderItemUnitScrapModel.EquipmentId, opt => opt.MapFrom(orderItemUnitScrapEntity => orderItemUnitScrapEntity.Equipment.EquipmentId))
                    .ForMember(orderItemUnitScrapModel => orderItemUnitScrapModel.DefectId, opt => opt.MapFrom(orderItemUnitScrapEntity => orderItemUnitScrapEntity.Defect.DefectId))
                    .ForMember(orderItemUnitScrapModel => orderItemUnitScrapModel.UserId, opt => opt.MapFrom(orderItemUnitScrapEntity => orderItemUnitScrapEntity.User.UserId))
                    .ForMember(orderItemUnitScrapModel => orderItemUnitScrapModel.OrderItemUnit, opt => opt.Ignore())
                    .ForMember(orderItemUnitScrapModel => orderItemUnitScrapModel.Equipment, opt => opt.Ignore())
                    .ForMember(orderItemUnitScrapModel => orderItemUnitScrapModel.Defect, opt => opt.Ignore())
                    .ForMember(orderItemUnitScrapModel => orderItemUnitScrapModel.User, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.OrderItemUnit, OrderItemUnit>();
            mapperConfig.CreateMap<Model.OrderItem, OrderItem>();
            mapperConfig.CreateMap<Model.Order, Order>();
            mapperConfig.CreateMap<Model.Equipment, Equipment>();
            mapperConfig.CreateMap<Model.Process, Process>();
            mapperConfig.CreateMap<Model.Defect, Defect>();
            mapperConfig.CreateMap<Model.DefectCategory, DefectCategory>();
            mapperConfig.CreateMap<Model.User, User>();
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public OrderItemUnitScrapData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return an OrderItemUnitScrap object by its serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number to search</param>
        /// <returns></returns>
        public OrderItemUnitScrap GetOrderItemUnitScrapBySerialNumber(string serialNumber)
        {
            OrderItemUnitScrap orderItemUnitScrap = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitScrapRecord = mesProductionContext.OrderItemUnitScraps
                    .Where(orderItemUnitScrap => orderItemUnitScrap.OrderItemUnit.SerialNumber == serialNumber)
                    .Include(orderItemUnitScrap => orderItemUnitScrap.OrderItemUnit)
                        .ThenInclude(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnitScrap => orderItemUnitScrap.Equipment)
                        .ThenInclude(equipment => equipment.Process)
                    .Include(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .Include(orderItemUnitScrap => orderItemUnitScrap.User)
                    .FirstOrDefault();

                if (orderItemUnitScrapRecord != null)
                {
                    orderItemUnitScrap = mapper.Map<Model.OrderItemUnitScrap, OrderItemUnitScrap>(orderItemUnitScrapRecord);
                }
            }

            return orderItemUnitScrap;
        }

        /// <summary>
        /// Add a new order item unit scrap.  Typically, this will be done from a quality screen
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        public void AddOrderItemUnitScrap(OrderItemUnitScrap orderItemUnitScrap)
        {
            var orderItemUnitScrapRecord = mapper.Map<Model.OrderItemUnitScrap>(orderItemUnitScrap);
            if (orderItemUnitScrapRecord != null)
            {
                // Force the scrap date to current time, rather than make the caller provide one
                orderItemUnitScrapRecord.ScrapDate = DateTime.Now;
                orderItemUnitScrapRecord.ScrapDateUtc = DateTime.UtcNow;

                // Set the Last Modified By if present
                orderItemUnitScrapRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(orderItemUnitScrap.LastModifiedBy) ? orderItemUnitScrap.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.OrderItemUnitScraps.Add(orderItemUnitScrapRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update an existing orderItemUnitScrap.  Typically done when changing the comment or reclassifying to a different defect code
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        public void UpdateOrderItemUnitScrap(OrderItemUnitScrap orderItemUnitScrap)
        {
            var existingOrderItemUnitScrap = GetOrderItemUnitScrapByOrderItemUnitScrapId(orderItemUnitScrap.OrderItemUnitScrapId);

            if (existingOrderItemUnitScrap == null)
                throw new Exception("Object does not exist: There was an attempt to update an object that does not exist in the database.");

            if (existingOrderItemUnitScrap.OrderItemUnitId != orderItemUnitScrap.OrderItemUnit.OrderItemUnitId)
                throw new Exception("Invalid object: The object's immutable properties do not match the existing database record.");

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                existingOrderItemUnitScrap.EquipmentId = orderItemUnitScrap.Equipment.EquipmentId;
                existingOrderItemUnitScrap.DefectId = orderItemUnitScrap.Defect.DefectId;
                existingOrderItemUnitScrap.UserId = orderItemUnitScrap.User.UserId;
                existingOrderItemUnitScrap.Comment = orderItemUnitScrap.Comment;
                existingOrderItemUnitScrap.ScrapDate = orderItemUnitScrap.ScrapDate;
                existingOrderItemUnitScrap.ScrapDateUtc = orderItemUnitScrap.ScrapDateUtc;
                existingOrderItemUnitScrap.LastModifiedTime = DateTime.Now;
                existingOrderItemUnitScrap.LastModifiedTimeUtc = DateTime.UtcNow;

                // Set the Last Modified By if present, otherwise, 
                existingOrderItemUnitScrap.LastModifiedBy = !string.IsNullOrWhiteSpace(orderItemUnitScrap.LastModifiedBy) ? orderItemUnitScrap.LastModifiedBy : existingOrderItemUnitScrap.LastModifiedBy;

                mesProductionContext.Entry(existingOrderItemUnitScrap).State = EntityState.Modified;
                mesProductionContext.SaveChanges();
            }
        }

        /// <summary>
        /// Delete an existing order item unit scrap record. Typically this is done when a scrapped unit is reworked successfully.
        /// </summary>
        /// <param name="orderItemUnitScrapId"></param>
        public void DeleteOrderItemUnitScrap(long orderItemUnitScrapId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitScrapToDelete = mesProductionContext.OrderItemUnitScraps
                    .Where(orderItemUnitScrap => orderItemUnitScrap.OrderItemUnitScrapId == orderItemUnitScrapId);
                    
                if (orderItemUnitScrapToDelete != null)
                {
                    mesProductionContext.RemoveRange(orderItemUnitScrapToDelete);
                    mesProductionContext.SaveChanges();
                }
            }
        }

        #endregion

        #region Private Methods

        private Model.OrderItemUnitScrap GetOrderItemUnitScrapByOrderItemUnitScrapId(long orderItemUnitScrapId)
        {
            using (var mesProductionContext = new Model.MesProductionContext())
            {
                return mesProductionContext.OrderItemUnitScraps
                    .Where(orderItemUnitScrap => orderItemUnitScrap.OrderItemUnitScrapId == orderItemUnitScrapId)
                    .FirstOrDefault();
            }
        }

        #endregion
    }
}
