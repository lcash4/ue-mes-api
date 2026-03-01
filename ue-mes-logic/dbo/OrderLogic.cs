using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;

namespace ue_mes_logic.dbo
{
    public class OrderLogic : LogicBase
    {
        IOrderData OrderData { get; }

        #region Constructor

        public OrderLogic(IOrderData orderData)
        {
            OrderData = orderData ?? throw new ArgumentNullException(nameof(orderData));
        }

        #endregion

        #region Public Methods

        public List<Order> GetOrdersByStates(List<OrderStateEnum> orderStates)
        {
            return OrderData.GetOrdersByStates(orderStates);
        }

        #endregion

    }
}
