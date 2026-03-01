using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IOrderItemUnitData
    {
        /// <summary>
        /// Return an OrderItemUnit object by its serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number to search</param>
        /// <returns></returns>
        public OrderItemUnit GetOrderItemUnitBySerialNumber(string serialNumber);

        /// <summary>
        /// Return the most recent order item unit for the given julian date.
        /// </summary>
        /// <param name="julianDate">julian date to search</param>
        /// <returns></returns>
        public OrderItemUnit GetMostRecentOrderItemUnitByJulianDate(string julianDate);

        /// <summary>
        /// Returns all order item units by searching the provided order item IDs
        /// </summary>
        /// <param name="orderItemIds">list of <see cref="OrderItem"/> IDs to search</param>
        /// <returns></returns>
        public List<OrderItemUnit> GetOrderItemUnitsForOrderItems(List<int> orderItemIds);

        /// <summary>
        /// Add a new order item unit.  Typically, this will be done from an order management screen
        /// </summary>
        /// <param name="orderItemUnit"></param>
        public void AddOrderItemUnit(OrderItemUnit orderItemUnit);
    }
}
