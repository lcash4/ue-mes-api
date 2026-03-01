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
using ue_mes_entities.Enums;
using ue_mes_entities.Global;

namespace ue_mes_data.dbo
{
    public class OrderItemUnitData : DbContextBase, IOrderItemUnitData
    {
        #region AutoMapper config

        private MapperConfiguration mapperConfiguration = new MapperConfiguration(mapperConfig =>
        {
            mapperConfig.CreateMap<Model.OrderItemUnit, OrderItemUnit>()
                .ReverseMap()
                    .ForMember(orderItemUnitModel => orderItemUnitModel.OrderItemId, opt => opt.MapFrom(orderItemUnitEntity => orderItemUnitEntity.OrderItem.OrderItemId))
                    .ForMember(orderItemUnitModel => orderItemUnitModel.PartId, opt => opt.MapFrom(orderItemUnitEntity => orderItemUnitEntity.Part.PartId))
                    .ForMember(orderItemUnitModel => orderItemUnitModel.OrderItem, opt => opt.Ignore())
                    .ForMember(orderItemUnitModel => orderItemUnitModel.Part, opt => opt.Ignore());
            mapperConfig.CreateMap<Model.OrderItemUnitWorkElementHistory, OrderItemUnitWorkElementHistory>();
            mapperConfig.CreateMap<Model.OrderItem, OrderItem>();
            mapperConfig.CreateMap<Model.Order, Order>();
            mapperConfig.CreateMap<Model.Part, Part>()
                .ForMember(partEntity => partEntity.PartType, opt => opt.MapFrom(partData => (int)partData.PartTypeId));
            mapperConfig.CreateMap<Model.BillOfProcessProcessWorkElement, BillOfProcessProcessWorkElement>();
            mapperConfig.CreateMap<Model.Equipment, Equipment>();
            mapperConfig.CreateMap<Model.Process, Process>();
            mapperConfig.CreateMap<Model.WorkElementStatus, WorkElementStatus>();
            mapperConfig.CreateMap<Model.User, User>();
            mapperConfig.CreateMap<Model.OrderItemUnitDataCollection, OrderItemUnitDataCollection>()
                .ForPath(orderItemUnitDataCollectionEntity => orderItemUnitDataCollectionEntity.BillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId, opt => opt.MapFrom(orderItemUnitDataCollectionRecord => orderItemUnitDataCollectionRecord.BillOfProcessProcessWorkElementAttributeId));
            mapperConfig.CreateMap<Model.OrderItemUnitConsumption, OrderItemUnitConsumption>()
                .ForPath(orderItemUnitConsumptionEntity => orderItemUnitConsumptionEntity.BillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId, opt => opt.MapFrom(orderItemUnitConsumptionRecord => orderItemUnitConsumptionRecord.BillOfProcessProcessWorkElementAttributeId));
            mapperConfig.CreateMap<Model.OrderItemUnitScrap, OrderItemUnitScrap>();
            mapperConfig.CreateMap<Model.Defect, Defect>();
            mapperConfig.CreateMap<Model.DefectCategory, DefectCategory>()
                .ForPath(defectCategoryEntity => defectCategoryEntity.Defects, opt => opt.Ignore());
        });
        private IMapper mapper;

        #endregion

        #region Constructor

        public OrderItemUnitData()
        {
            mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return an OrderItemUnit object by its serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number to search</param>
        /// <returns></returns>
        public OrderItemUnit GetOrderItemUnitBySerialNumber(string serialNumber)
        {
            OrderItemUnit orderItemUnit = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitRecord = mesProductionContext.OrderItemUnits
                    .Where(orderItemUnit => orderItemUnit.SerialNumber == serialNumber)
                    .Include(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.Order)
                    .Include(orderItemUnit => orderItemUnit.OrderItem)
                        .ThenInclude(orderItem => orderItem.FinishedPart)
                    .Include(orderItemUnit => orderItemUnit.Part)
                    .Include(orderItemUnit => orderItemUnit.OrderItemUnitWorkElementHistories)
                        .ThenInclude(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement)
                    .Include(orderItemUnit => orderItemUnit.OrderItemUnitWorkElementHistories)
                        .ThenInclude(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.Equipment)
                        .ThenInclude(equipment => equipment.Process)
                    .Include(orderItemUnit => orderItemUnit.OrderItemUnitWorkElementHistories)
                        .ThenInclude(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.WorkElementStatus)
                    .Include(orderItemUnit => orderItemUnit.OrderItemUnitWorkElementHistories)
                        .ThenInclude(orderItemUnitWorkElementHistory => orderItemUnitWorkElementHistory.User)
                    .Include(orderItemUnit => orderItemUnit.OrderItemUnitDataCollections)
                    .Include(orderItemUnit => orderItemUnit.OrderItemUnitConsumptions)
                    .Include(orderItemUnit => orderItemUnit.OrderItemUnitScrap)
                        .ThenInclude(orderItemUnitScrap => orderItemUnitScrap.Defect)
                        .ThenInclude(defect => defect.DefectCategory)
                    .FirstOrDefault();

                if (orderItemUnitRecord != null)
                {
                    orderItemUnit = mapper.Map<Model.OrderItemUnit, OrderItemUnit>(orderItemUnitRecord);
                }
            }

            return orderItemUnit;
        }

        /// <summary>
        /// Return the most recent order item unit for the given julian date.
        /// </summary>
        /// <param name="julianDate">julian date to search</param>
        /// <returns></returns>
        public OrderItemUnit GetMostRecentOrderItemUnitByJulianDate(string julianDate)
        {
            OrderItemUnit orderItemUnit = null;

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitRecord = mesProductionContext.OrderItemUnits
                    .Where(orderItemUnit => orderItemUnit.SerialNumber.StartsWith(julianDate))
                    .OrderByDescending(orderItemUnit => orderItemUnit.SerialNumber)
                    .FirstOrDefault();

                if (orderItemUnitRecord != null)
                {
                    orderItemUnit = mapper.Map<Model.OrderItemUnit, OrderItemUnit>(orderItemUnitRecord);
                }
            }

            return orderItemUnit;
        }

        /// <summary>
        /// Returns all order item units by searching the provided order item IDs
        /// </summary>
        /// <param name="orderItemIds">list of <see cref="OrderItem"/> IDs to search</param>
        /// <returns></returns>
        public List<OrderItemUnit> GetOrderItemUnitsForOrderItems(List<int> orderItemIds)
        {
            List<OrderItemUnit> orderItemUnits = new List<OrderItemUnit>();

            using (var mesProductionContext = new Model.MesProductionContext())
            {
                var orderItemUnitRecords = mesProductionContext.OrderItemUnits
                    .Where(orderItemUnit => orderItemIds.Contains(orderItemUnit.OrderItemId))
                    .Include(orderItemUnit => orderItemUnit.OrderItem)
                    .ToList();

                if (orderItemUnitRecords != null)
                {
                    orderItemUnits = mapper.Map<List<Model.OrderItemUnit>, List<OrderItemUnit>>(orderItemUnitRecords);
                }
            }

            return orderItemUnits;
        }

        /// <summary>
        /// Add a new order item unit.  Typically, this will be done from an order management screen
        /// </summary>
        /// <param name="orderItemUnit"></param>
        public void AddOrderItemUnit(OrderItemUnit orderItemUnit)
        {
            var orderItemUnitRecord = mapper.Map<Model.OrderItemUnit>(orderItemUnit);
            if (orderItemUnitRecord != null)
            {
                // Set the Last Modified By if present
                orderItemUnitRecord.LastModifiedBy = !string.IsNullOrWhiteSpace(orderItemUnit.LastModifiedBy) ? orderItemUnit.LastModifiedBy : null;

                using (var mesProductionContext = new Model.MesProductionContext())
                {
                    mesProductionContext.OrderItemUnits.Add(orderItemUnitRecord);
                    mesProductionContext.SaveChanges();
                }
            }
        }
        #endregion
    }
}
