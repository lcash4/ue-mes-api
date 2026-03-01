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
    public interface IOrderItemUnitConsumptionData
    {
        /// <summary>
        /// This method will pull existing consumption records for the given serial number.  
        /// It will only return those that have values and IsConsumed = true (IsConsumed = false will not be used in the client or validation logic as it is similar to not existing at all)
        /// </summary>
        /// <param name="serialNumber">The consumed serial number</param>
        /// <returns>Can return a list as a lot serial number can be consumed in many assemblies</returns>
        public List<OrderItemUnitConsumption> GetOrderItemUnitConsumptionsByConsumedSerialNumber(string serialNumber);

        /// <summary>
        /// This method will pull existing consumption records for the given work element and serial number.  
        /// It will only return those that have values and IsConsumed = true (IsConsumed = false will not be used in the client)
        /// </summary>
        /// <param name="workElementId">The specific work element from the caller</param>
        /// <param name="serialNumber">The serial number of the assembly being worked on</param>
        /// <returns></returns>
        public List<OrderItemUnitConsumption> GetOrderItemUnitConsumptionByWorkElementAndSerialNumber(int workElementId, string serialNumber);

        /// <summary>
        /// Add a range of new order item unit consumption values
        /// </summary>
        /// <param name="orderItemUnitConsumptions"></param>
        public void AddOrderItemUnitConsumptions(List<OrderItemUnitConsumption> orderItemUnitConsumptions);

        /// <summary>
        /// Remove an existing order item unit consumption value.
        /// Note: "Remove" will actually just update the existing record with IsConsumed = false and not actually delete the record.
        /// </summary>
        /// <param name="orderItemUnitConsumption"></param>
        public void RemoveOrderItemUnitConsumption(OrderItemUnitConsumption orderItemUnitConsumption);
    }
}
