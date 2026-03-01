using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;

namespace ue_mes_data.dbo.Interface
{
    public interface IOrderData
    {
        /// <summary>
        /// Returns all orders that have a matching state to the list provided
        /// </summary>
        /// <param name="orderStates"></param>
        /// <returns></returns>
        public List<Order> GetOrdersByStates(List<OrderStateEnum> orderStates);
    }
}
