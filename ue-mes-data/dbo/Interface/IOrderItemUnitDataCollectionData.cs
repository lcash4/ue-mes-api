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
    public interface IOrderItemUnitDataCollectionData
    {
        /// <summary>
        /// This method will pull existing data collection records for the given work element and serial number.  
        /// It will only return those that have collected values and may not be the full list of data collection attributes
        /// </summary>
        /// <param name="workElementId">The specific work element from the caller</param>
        /// <param name="serialNumber">The serial number of the assembly being worked on</param>
        /// <returns></returns>
        public List<OrderItemUnitDataCollection> GetOrderItemUnitDataCollectionByWorkElementAndSerialNumber(int workElementId, string serialNumber);

        /// <summary>
        /// Add a range of new order item unit data collection values
        /// </summary>
        /// <param name="orderItemUnitDataCollections"></param>
        public void AddOrderItemUnitDataCollections(List<OrderItemUnitDataCollection> orderItemUnitDataCollections);

        /// <summary>
        /// Update an existing order item unit data collection value.  
        /// </summary>
        /// <param name="orderItemUnitDataCollection"></param>
        public void UpdateOrderItemUnitDataCollection(OrderItemUnitDataCollection orderItemUnitDataCollection);
    }
}
