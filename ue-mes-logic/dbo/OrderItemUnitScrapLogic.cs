using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class OrderItemUnitScrapLogic : LogicBase
    {
        IOrderItemUnitScrapData OrderItemUnitScrapData { get; }

        #region Constructor

        public OrderItemUnitScrapLogic(IOrderItemUnitScrapData orderItemUnitScrapData)
        {
            OrderItemUnitScrapData = orderItemUnitScrapData ?? throw new ArgumentNullException(nameof(orderItemUnitScrapData));
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
            return OrderItemUnitScrapData.GetOrderItemUnitScrapBySerialNumber(serialNumber);
        }

        /// <summary>
        /// Add a new order item unit scrap.  Typically, this will be done from a quality screen
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        public void AddOrderItemUnitScrap(OrderItemUnitScrap orderItemUnitScrap)
        {
            // Need to check if the order item unit is already scrapped.  If so, return "unit already scrapped" exception
            var existingScrap = OrderItemUnitScrapData.GetOrderItemUnitScrapBySerialNumber(orderItemUnitScrap.OrderItemUnit.SerialNumber);
            if (existingScrap != null)
                throw new Exception(string.Format("Order Item Unit with Serial {0}, is already scrapped.", orderItemUnitScrap.OrderItemUnit.SerialNumber));

            OrderItemUnitScrapData.AddOrderItemUnitScrap(orderItemUnitScrap);
        }

        /// <summary>
        /// Reclassify scrap is used when the defect is incorrect and needs changed.
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        public void ReclassifyOrderItemUnitScrap(OrderItemUnitScrap orderItemUnitScrap)
        {
            var existingOrderItemUnitScrap = OrderItemUnitScrapData.GetOrderItemUnitScrapBySerialNumber(orderItemUnitScrap.OrderItemUnit.SerialNumber);

            if (existingOrderItemUnitScrap == null)
                throw new Exception(string.Format("Order Item Unit with Serial {0}, is not listed as scrap and cannot be reclassified.", orderItemUnitScrap.OrderItemUnit.SerialNumber));

            OrderItemUnitScrapData.UpdateOrderItemUnitScrap(orderItemUnitScrap);
        }

        /// <summary>
        /// Return an order item unit to production.  This is done if the scrapping was inadvertent, or discovered later that the unit is okay.
        /// If the unit has to be manually fixed, then the rework logic should be used instead.
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        public void ReturnOrderItemUnitToProduction(OrderItemUnitScrap orderItemUnitScrap)
        {
            // When coming from a client or caller, a comment will be required as to why the RTP is happening.
            // Therefore, this logic will first update the scrap record with the RTP comment and then delete it.
            // The history schema in the data model can be used to analyze these comments if needed.  
            var existingOrderItemUnitScrap = OrderItemUnitScrapData.GetOrderItemUnitScrapBySerialNumber(orderItemUnitScrap.OrderItemUnit.SerialNumber);

            if (existingOrderItemUnitScrap == null)
                throw new Exception(string.Format("Order Item Unit with Serial {0}, is not listed as scrap and cannot be returned to production.", orderItemUnitScrap.OrderItemUnit.SerialNumber));

            OrderItemUnitScrapData.UpdateOrderItemUnitScrap(orderItemUnitScrap);
            OrderItemUnitScrapData.DeleteOrderItemUnitScrap(existingOrderItemUnitScrap.OrderItemUnitScrapId);
        }

        #endregion
    }
}
