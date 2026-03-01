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
    public class OrderItemUnitConsumptionLogic : LogicBase
    {
        IOrderItemUnitConsumptionData OrderItemUnitConsumptionData { get; }

        #region Constructor

        public OrderItemUnitConsumptionLogic(IOrderItemUnitConsumptionData orderItemUnitConsumptionData)
        {
            OrderItemUnitConsumptionData = orderItemUnitConsumptionData ?? throw new ArgumentNullException(nameof(orderItemUnitConsumptionData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method will pull existing consumption records for the given work element and serial number.  
        /// It will only return those that have values and IsConsumed = true (IsConsumed = false will not be used in the client)
        /// </summary>
        /// <param name="workElementId">The specific work element from the caller</param>
        /// <param name="serialNumber">The serial number of the assembly being worked on</param>
        /// <returns></returns>
        public List<OrderItemUnitConsumption> GetOrderItemUnitConsumptionByWorkElementAndSerialNumber(int workElementId, string serialNumber)
        {
            return OrderItemUnitConsumptionData.GetOrderItemUnitConsumptionByWorkElementAndSerialNumber(workElementId, serialNumber);
        }

        /// <summary>
        /// Add a range of new order item unit consumption values
        /// If any of the consumption attributes already have a value, it will be removed and then replaced with the new one.
        /// </summary>
        /// <param name="orderItemUnitConsumptions"></param>
        public void SaveOrderItemUnitConsumptions(List<OrderItemUnitConsumption> orderItemUnitConsumptions)
        {
            List<OrderItemUnitConsumption> newOrderItemUnitConsumptionsToAdd = orderItemUnitConsumptions.Where(orderItemUnitConsumption => orderItemUnitConsumption.OrderItemUnitConsumptionId == 0).ToList();
            List<OrderItemUnitConsumption> existingOrderItemUnitConsumptionsToUpdate = orderItemUnitConsumptions.Where(orderItemUnitConsumption => orderItemUnitConsumption.OrderItemUnitConsumptionId > 0).ToList();

            // First, update the existing to remove them from the assembly (set isRemoved to false)
            existingOrderItemUnitConsumptionsToUpdate.ForEach(orderItemUnitConsumption =>
            {
                OrderItemUnitConsumptionData.RemoveOrderItemUnitConsumption(orderItemUnitConsumption);
            });

            // Then, add the new items
            OrderItemUnitConsumptionData.AddOrderItemUnitConsumptions(newOrderItemUnitConsumptionsToAdd);

        }

        #endregion
    }
}
