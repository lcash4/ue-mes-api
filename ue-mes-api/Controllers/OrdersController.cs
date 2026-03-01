using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;
using ue_mes_logic.dbo;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        #region Constants

        private IReadOnlyList<OrderStateEnum> createdOrderState = new List<OrderStateEnum>() { OrderStateEnum.Created };
        private IReadOnlyList<OrderStateEnum> holdOrderState = new List<OrderStateEnum>() { OrderStateEnum.Hold };
        private IReadOnlyList<OrderStateEnum> releasedOrderState = new List<OrderStateEnum>() { OrderStateEnum.Released };
        private IReadOnlyList<OrderStateEnum> inProgressOrderState = new List<OrderStateEnum>() { OrderStateEnum.Started, OrderStateEnum.Complete };
        private IReadOnlyList<OrderStateEnum> closedOrderState = new List<OrderStateEnum>() { OrderStateEnum.Closed };

        #endregion

        #region Constructor

        ILogger Logger { get; }
        IOrderData OrderData { get; }

        public OrdersController(ILogger<PartsController> logger, IOrderData orderData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            OrderData = orderData ?? throw new ArgumentNullException(nameof(orderData));
        }

        #endregion

        #region Public Controller Methods
        
        /// <summary>
        /// Return all created orders
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("created")]
        public List<Order> GetCreatedOrders()
        {
            using (var orderLogic = new OrderLogic(OrderData))
            {
                return orderLogic.GetOrdersByStates(createdOrderState.ToList());
            }
        }

        /// <summary>
        /// Return all on hold orders
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("hold")]
        public List<Order> GetOnHoldOrders()
        {
            using (var orderLogic = new OrderLogic(OrderData))
            {
                return orderLogic.GetOrdersByStates(holdOrderState.ToList());
            }
        }

        /// <summary>
        /// Return all orders released and ready for production
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("released")]
        public List<Order> GetReleasedOrders()
        {
            using (var orderLogic = new OrderLogic(OrderData))
            {
                return orderLogic.GetOrdersByStates(releasedOrderState.ToList());
            }
        }

        /// <summary>
        /// Return all orders that have started or have completed production, but not yet closed.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("inprogress")]
        public List<Order> GetInProgressOrders()
        {
            using (var orderLogic = new OrderLogic(OrderData))
            {
                return orderLogic.GetOrdersByStates(inProgressOrderState.ToList());
            }
        }

        /// <summary>
        /// Return all closed orders, meaning they have completed and been shipped to the customer
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("closed")]
        public List<Order> GetClosedOrders()
        {
            using (var orderLogic = new OrderLogic(OrderData))
            {
                return orderLogic.GetOrdersByStates(closedOrderState.ToList());
            }
        }

        /// <summary>
        /// Return all orders
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("all")]
        public List<Order> GetAllOrders()
        {
            var allOrderStates = createdOrderState.Concat(holdOrderState).Concat(releasedOrderState).Concat(inProgressOrderState).Concat(closedOrderState);

            using (var orderLogic = new OrderLogic(OrderData))
            {
                return orderLogic.GetOrdersByStates(allOrderStates.ToList());
            }
        }

        #endregion
    }
}
