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
    public interface IOrderItemUnitScrapData
    {
        /// <summary>
        /// Return an OrderItemUnitScrap object by its serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number to search</param>
        /// <returns></returns>
        public OrderItemUnitScrap GetOrderItemUnitScrapBySerialNumber(string serialNumber);

        /// <summary>
        /// Add a new order item unit scrap.  Typically, this will be done from a quality screen
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        public void AddOrderItemUnitScrap(OrderItemUnitScrap orderItemUnitScrap);

        /// <summary>
        /// Update an existing orderItemUnitScrap.  Typically done when changing the comment or reclassifying to a different defect code
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        public void UpdateOrderItemUnitScrap(OrderItemUnitScrap orderItemUnitScrap);

        /// <summary>
        /// Delete an existing order item unit scrap record. Typically this is done when a scrapped unit is reworked successfully.
        /// </summary>
        /// <param name="orderItemUnitScrapId"></param>
        public void DeleteOrderItemUnitScrap(long orderItemUnitScrapId);
    }
}
