using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using ue_mes_api.Dto;
using ue_mes_logic.Global;
using ue_mes_logic.dbo;
using ue_mes_entities.dbo;
using Microsoft.AspNetCore.SignalR;
using ue_mes_api.SignalR;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ScrapController : ControllerBase
    {
        #region Constructor

        ILogger Logger { get; }
        IInventoryItemScrapData InventoryItemScrapData { get; }
        IOrderItemUnitScrapData OrderItemUnitScrapData { get; }
        IHubContext<OrderItemUnitHub> OrderItemUnitHubContext { get; }

        public ScrapController(ILogger<ScrapController> logger, 
            IInventoryItemScrapData inventoryItemScrapData,
            IOrderItemUnitScrapData orderItemUnitScrapData,
            IHubContext<OrderItemUnitHub> orderItemUnitHubContext)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InventoryItemScrapData = inventoryItemScrapData ?? throw new ArgumentNullException(nameof(inventoryItemScrapData));
            OrderItemUnitScrapData = orderItemUnitScrapData ?? throw new ArgumentNullException(nameof(orderItemUnitScrapData));
            OrderItemUnitHubContext = orderItemUnitHubContext ?? throw new ArgumentNullException(nameof(orderItemUnitHubContext));
        }

        #endregion

        #region Public API Methods

        #region Inventory Item

        /// <summary>
        /// Returns a InventoryItemScrap entity that matches the given serial number
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(InventoryItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("InventoryItemScrap/GetBySerialNumber/{serialNumber}")]
        public IActionResult GetInventoryItemScrapBySerialNumber(string serialNumber)
        {
            using (var inventoryItemScrapLogic = new InventoryItemScrapLogic(InventoryItemScrapData))
            {
                var inventoryItemScrapForSerial = inventoryItemScrapLogic.GetInventoryItemScrapBySerialNumber(serialNumber);

                if (inventoryItemScrapForSerial == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("Serial number {0} is not found as an inventory item scrap.  Please verify entry or contact MES support for help.", serialNumber));

                return Ok(inventoryItemScrapForSerial);
            }
        }

        /// <summary>
        /// Scrap an inventory item
        /// This will be used to scrap inventory items with some quantity.  They will remain as inventory items as well, but the usable quantity will be reduced by any matching scrap quantity
        /// </summary>
        /// <param name="inventoryItemScrap"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("InventoryItemScrap/Add")]
        public IActionResult AddInventoryItemScrap([FromBody] InventoryItemScrap inventoryItemScrap)
        {
            using (var inventoryItemScrapLogic = new InventoryItemScrapLogic(InventoryItemScrapData))
            {
                try
                {
                    inventoryItemScrapLogic.AddInventoryItemScrap(inventoryItemScrap);
                    Logger.LogInformation("Added {0} quantity of inventory scrap for serial number: {1}", inventoryItemScrap.Quantity, inventoryItemScrap.InventoryItem.SerialNumber);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the inventory scrap.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the inventory scrap.  Please verify entry or contact MES support for help."));
                }
            }
        }

        #endregion

        #region Order Item Unit

        /// <summary>
        /// Returns a OrderItemUntScrap entity that matches the given serial number
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(OrderItemUnitScrap), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("OrderItemUnitScrap/GetBySerialNumber/{serialNumber}")]
        public IActionResult GetOrderItemUnitScrapBySerialNumber(string serialNumber)
        {
            using (var orderItemUnitScrapLogic = new OrderItemUnitScrapLogic(OrderItemUnitScrapData))
            {
                var orderItemUnitScrapForSerial = orderItemUnitScrapLogic.GetOrderItemUnitScrapBySerialNumber(serialNumber);

                if (orderItemUnitScrapForSerial == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("Serial number {0} is not found as scrap.  Please verify entry or contact MES support for help.", serialNumber));

                return Ok(orderItemUnitScrapForSerial);
            }
        }

        /// <summary>
        /// Scrap an order item unit
        /// This will be used to scrap order item units in assembly.  Depending on the defect assigned, they can either be reworked and sent back to the line, or recycled\trashed.
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemUnitScrap/Add")]
        public async Task<IActionResult> AddOrderItemUnitScrap([FromBody] OrderItemUnitScrap orderItemUnitScrap)
        {
            using (var orderItemUnitScrapLogic = new OrderItemUnitScrapLogic(OrderItemUnitScrapData))
            {
                try
                {
                    orderItemUnitScrapLogic.AddOrderItemUnitScrap(orderItemUnitScrap);

                    // Send SignalR message to any listening clients
                    await OrderItemUnitHubContext.Clients.Group(orderItemUnitScrap.Equipment.Name)
                        .SendAsync("ReceiveOrderItemUnitScrapped", orderItemUnitScrap.OrderItemUnit.SerialNumber);

                    Logger.LogInformation("Order item unit with serial number: {0} has been scrapped successfully." , orderItemUnitScrap.OrderItemUnit.SerialNumber);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the order item unit scrap.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the order item unit scrap.  Please verify entry or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Reclassify scrap is used when the defect is incorrect and needs changed.
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemUnitScrap/Reclassify")]
        public IActionResult ReclassifyOrderItemUnitScrap([FromBody] OrderItemUnitScrap orderItemUnitScrap)
        {
            using (var orderItemUnitScrapLogic = new OrderItemUnitScrapLogic(OrderItemUnitScrapData))
            {
                try
                {
                    orderItemUnitScrapLogic.ReclassifyOrderItemUnitScrap(orderItemUnitScrap);
                    Logger.LogInformation("Order item unit with serial number: {0} has been reclassified successfully.", orderItemUnitScrap.OrderItemUnit.SerialNumber);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when reclassifying the order item unit scrap.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when reclassifying the order item unit scrap.  Please verify entry or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Return an order item unit to production.  This is done if the scrapping was inadvertent, or discovered later that the unit is okay.
        /// If the unit has to be manually fixed, then the rework logic should be used instead.
        /// </summary>
        /// <param name="orderItemUnitScrap"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemUnitScrap/ReturnToProduction")]
        public async Task<IActionResult> ReturnOrderItemUnitToProduction([FromBody] OrderItemUnitScrap orderItemUnitScrap)
        {
            using (var orderItemUnitScrapLogic = new OrderItemUnitScrapLogic(OrderItemUnitScrapData))
            {
                try
                {
                    orderItemUnitScrapLogic.ReturnOrderItemUnitToProduction(orderItemUnitScrap);

                    // Send SignalR message to any listening clients
                    await OrderItemUnitHubContext.Clients.Group(orderItemUnitScrap.Equipment.Name)
                        .SendAsync("ReceiveOrderItemUnitReturnedToProduction", orderItemUnitScrap.OrderItemUnit.SerialNumber);

                    
                    Logger.LogInformation("Order item unit with serial number: {0} has been returned to production successfully.", orderItemUnitScrap.OrderItemUnit.SerialNumber);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when returning the order item unit to production.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when returning the order item unit to production.  Please verify entry or contact MES support for help."));
                }
            }
        }

        #endregion

        #endregion
    }
}
