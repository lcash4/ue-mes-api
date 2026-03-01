using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using ue_mes_logic.dbo;
using ue_mes_entities.dbo;
using System.Linq;
using ue_mes_api.Dto;
using ue_mes_logic.Global;
using Microsoft.AspNetCore.SignalR;
using ue_mes_api.SignalR;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net.Sockets;
using ue_mes_data.Global.Interface;
using ue_mes_data.Global;
using ue_mes_data.dbo.Interface;
using ue_mes_data.dbo;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderItemUnitController : Controller
    {
        #region Constructor

        ILogger Logger { get; }
        IBillOfMaterialData BillOfMaterialData { get; }
        IBillOfProcessData BillOfProcessData { get; }
        ICoaterData CoaterData { get; }
        IEquipmentData EquipmentData { get; }
        IInventoryItemData InventoryItemData { get; }
        IInventoryItemTypeData InventoryItemTypeData { get; }
        IInventoryLocationData InventoryLocationData { get; }
        IOrderItemData OrderItemData { get; }
        IOrderItemUnitConsumptionData OrderItemUnitConsumptionData { get; }
        IOrderItemUnitData OrderItemUnitData { get; }
        IOrderItemUnitDataCollectionData OrderItemUnitDataCollectionData { get; }
        IOrderItemUnitWorkElementHistoryData OrderItemUnitWorkElementHistoryData { get; }
        IPartData PartData { get; }
        IPartItemAttributeTypeData PartItemAttributeTypeData { get; }
        IWorkElementStatusData WorkElementStatusData { get; }

        IHubContext<OrderItemUnitHub> OrderItemUnitHubContext {get;}

        public OrderItemUnitController(ILogger<OrderItemUnitController> logger,
            IBillOfMaterialData billOfMaterialData,
            IBillOfProcessData billOfProcessData,
            ICoaterData coaterData,
            IEquipmentData equipmentData,
            IInventoryItemData inventoryItemData,
            IInventoryItemTypeData inventoryItemTypeData,
            IInventoryLocationData inventoryLocationData,
            IOrderItemData orderItemData,
            IOrderItemUnitConsumptionData orderItemUnitConsumptionData,
            IOrderItemUnitData orderItemUnitData,
            IOrderItemUnitDataCollectionData orderItemUnitDataCollectionData,
            IOrderItemUnitWorkElementHistoryData orderItemUnitWorkElementHistoryData,
            IPartData partData,
            IPartItemAttributeTypeData partItemAttributeTypeData,
            IWorkElementStatusData workElementStatusData,
            IHubContext<OrderItemUnitHub> orderItemUnitHubContext)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            BillOfMaterialData = billOfMaterialData ?? throw new ArgumentNullException(nameof(billOfMaterialData));
            BillOfProcessData = billOfProcessData ?? throw new ArgumentNullException(nameof(billOfProcessData));
            CoaterData = coaterData ?? throw new ArgumentNullException(nameof(coaterData));
            EquipmentData = equipmentData ?? throw new ArgumentNullException(nameof(equipmentData));
            InventoryItemData = inventoryItemData ?? throw new ArgumentNullException(nameof(inventoryItemData));
            InventoryItemTypeData = inventoryItemTypeData ?? throw new ArgumentNullException(nameof(inventoryItemTypeData));
            InventoryLocationData = inventoryLocationData ?? throw new ArgumentNullException(nameof(inventoryLocationData));
            OrderItemData = orderItemData ?? throw new ArgumentNullException(nameof(orderItemData));
            OrderItemUnitConsumptionData = orderItemUnitConsumptionData ?? throw new ArgumentNullException(nameof(orderItemUnitConsumptionData));
            OrderItemUnitData = orderItemUnitData ?? throw new ArgumentNullException(nameof(orderItemUnitData));
            OrderItemUnitDataCollectionData = orderItemUnitDataCollectionData ?? throw new ArgumentNullException(nameof(orderItemUnitDataCollectionData));
            OrderItemUnitWorkElementHistoryData = orderItemUnitWorkElementHistoryData ?? throw new ArgumentNullException(nameof(orderItemUnitWorkElementHistoryData));
            PartData = partData ?? throw new ArgumentNullException(nameof(partData));
            PartItemAttributeTypeData = partItemAttributeTypeData ?? throw new ArgumentNullException(nameof(partItemAttributeTypeData));
            WorkElementStatusData = workElementStatusData ?? throw new ArgumentNullException(nameof(workElementStatusData));

            OrderItemUnitHubContext = orderItemUnitHubContext ?? throw new ArgumentNullException(nameof(orderItemUnitHubContext));
        }

        #endregion

        #region Public API Methods

        #region Order Item Unit

        /// <summary>
        /// Returns all order item units by searching the provided order item IDs
        /// </summary>
        /// <param name="orderItemIds">list of <see cref="OrderItem"/> IDs to search</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("OrderItemUnit/GetByOrderItemIds")]
        public IActionResult GetOrderItemUnitsForOrderItems([FromBody] List<int> orderItemIds)
        {
            using (var orderItemUnitLogic = new OrderItemUnitLogic(BillOfProcessData, CoaterData, EquipmentData, InventoryItemData, InventoryItemTypeData, InventoryLocationData, OrderItemUnitData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                return Ok(orderItemUnitLogic.GetOrderItemUnitsForOrderItems(orderItemIds));
            }
        }

        /// <summary>
        /// Starts a new order item unit
        /// Starting a new order item unit involves:
        ///  - generating a serial number based on the part and current date.
        ///  - Adding the new serial number as an OrderItemUnit for the OrderItem and Part
        ///  - Adding the new serial number as a production inventory item
        ///  - Adding the first work element history 
        /// If all transactions succeed, the serial number is returned to the caller so that they can refresh the client with the new order item unit.
        /// </summary>
        /// <param name="startOrderItemUnitViewModel"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemUnit/Start")]
        public async Task<IActionResult> StartOrderItemUnit([FromBody] StartOrderItemUnitViewModel startOrderItemUnitViewModel)
        {
            using (var orderItemUnitLogic = new OrderItemUnitLogic(BillOfProcessData, CoaterData, EquipmentData, InventoryItemData, InventoryItemTypeData, InventoryLocationData, OrderItemUnitData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                try
                {
                    var startedSerialNumber = orderItemUnitLogic.StartOrderItemUnit(startOrderItemUnitViewModel.OrderItem, startOrderItemUnitViewModel.Part, startOrderItemUnitViewModel.Equipment, startOrderItemUnitViewModel.User);

                    // Send SignalR message to any listening clients
                    await OrderItemUnitHubContext.Clients.Group(startOrderItemUnitViewModel.Equipment.Name)
                        .SendAsync("ReceiveOrderItemUnitStarted", startedSerialNumber);

                    Logger.LogInformation("Started new serial number {0} for part: {1}({2}) and order {3}", startedSerialNumber, startOrderItemUnitViewModel.Part.PartNumber, startOrderItemUnitViewModel.Part.PartRevision, startOrderItemUnitViewModel.OrderItem.Order.Number);
                    return Ok(startedSerialNumber);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when starting the order item unit.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when starting the order item unit.  Please contact MES support for help."));
                }                
            }
        }

        #endregion

        #region Order Item Unit, Equipment and User View Model

        /// <summary>
        /// This method is used to load a OrderItemUnitEquipmentAndUserViewModel by serial number.
        /// First, it will ensure the OrderItemUnit exists.  
        ///     If not, return a server error with not found reply.
        ///     If found, attempt to hydrate the view model with latest equipment and its work element history
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(OrderItemUnitEquipmentAndUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("OrderItemUnitEquipmentAndUserViewModel/GetBySerialNumber/{serialNumber}")]
        public IActionResult GetOrderItemUnitEquipmentAndUserViewModelBySerialNumber(string serialNumber)
        {
            using(var orderItemUnitLogic = new OrderItemUnitLogic(BillOfProcessData, CoaterData, EquipmentData, InventoryItemData, InventoryItemTypeData, InventoryLocationData, OrderItemUnitData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            using (var equipmentLogic = new EquipmentLogic(EquipmentData))
            {
                var orderItemUnitForSerial = orderItemUnitLogic.GetOrderItemUnitBySerialNumber(serialNumber);

                if (orderItemUnitForSerial == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("Serial number {0} is not found as an order item unit.  Please verify entry or contact MES support for help.", serialNumber));

                var orderItemUnitEquipmentAndUserViewModelForSerial = new OrderItemUnitEquipmentAndUserViewModel()
                {
                    OrderItemUnit = orderItemUnitForSerial
                };

                var mostRecentWorkElementHistory = orderItemUnitForSerial.OrderItemUnitWorkElementHistories.OrderByDescending(workElementHistory => workElementHistory.StartDateUtc).FirstOrDefault();
                if (mostRecentWorkElementHistory != null)
                {
                    orderItemUnitEquipmentAndUserViewModelForSerial.Equipment = equipmentLogic.GetEquipmentByName(mostRecentWorkElementHistory.Equipment.Name);
                    orderItemUnitEquipmentAndUserViewModelForSerial.User = mostRecentWorkElementHistory.User;
                }
                
                return Ok(orderItemUnitEquipmentAndUserViewModelForSerial);
            }
        }

        /// <summary>
        /// This method is used to load a OrderItemUnitEquipmentAndUserViewModel by equipment.
        /// First, it will pull the most recent work element history for the given equipment.
        ///     If not found, there simply isn't an order item here and the view model can be returned with equipment hydrated, but no order item.  The client will take over from there.
        ///     If found, attempt to hydrate the view model with latest equipment and its work element history
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(OrderItemUnitEquipmentAndUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("OrderItemUnitEquipmentAndUserViewModel/GetByEquipment/{equipmentName}")]
        public IActionResult GetOrderItemUnitEquipmentAndUserViewModelByEquipment(string equipmentName)
        {
            using (var equipmentLogic = new EquipmentLogic(EquipmentData))
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                var equipment = equipmentLogic.GetEquipmentByName(equipmentName);

                var orderItemUnitEquipmentAndUserViewModelForEquipment = new OrderItemUnitEquipmentAndUserViewModel()
                {
                    Equipment = equipment
                };

                var orderItemUnitWorkElementHistoryForEquipment = orderItemUnitWorkElementHistoryLogic.GetMostRecentOrderItemUnitByEquipment(equipmentName);
                
                // If no work element history for this equipment, simply return with the object only having the equipment set.
                if (orderItemUnitWorkElementHistoryForEquipment == null)
                    return Ok(orderItemUnitEquipmentAndUserViewModelForEquipment);

                // reset equipment with fully hydrated lineage.
                orderItemUnitWorkElementHistoryForEquipment.Equipment = equipment;

                // If work element history is found, we need to first make sure the serial is still present at this equipment.  If it has moved on to a downstream process, then we will not load this item, and instead offer a "start unit" if the client logic allows.
                var mostRecentOrderItemUnitWorkElementHistories = orderItemUnitWorkElementHistoryLogic.GetOrderItemUnitWorkElementHistoryBySerialNumber(orderItemUnitWorkElementHistoryForEquipment.OrderItemUnit.SerialNumber);
                if(mostRecentOrderItemUnitWorkElementHistories != null && mostRecentOrderItemUnitWorkElementHistories.FirstOrDefault().Equipment.Name != equipmentName)
                    return Ok(orderItemUnitEquipmentAndUserViewModelForEquipment);

                // If the active history matches the input equipmentName, then we can load the OrderItemUnit with its history and User
                orderItemUnitEquipmentAndUserViewModelForEquipment.OrderItemUnit = orderItemUnitWorkElementHistoryForEquipment.OrderItemUnit;
                orderItemUnitEquipmentAndUserViewModelForEquipment.User = orderItemUnitWorkElementHistoryForEquipment.User;
                orderItemUnitEquipmentAndUserViewModelForEquipment.Equipment = equipment;

                // With an object found, we need to load the full work element history for the current BOP process so that it can be presented in the client.
                orderItemUnitEquipmentAndUserViewModelForEquipment.OrderItemUnit.OrderItemUnitWorkElementHistories = mostRecentOrderItemUnitWorkElementHistories;

                return Ok(orderItemUnitEquipmentAndUserViewModelForEquipment);
            }
        }

        #endregion

        #region Part and OrderItems view model

        /// <summary>
        /// This method is used to load a list of PartAndOrderItemsViewModel by equipment name.
        /// First, it will ensure the equipment is a starting equipment by returning any parts that have a BOP sequence = 1 that matches the equipment sequence
        ///     If not, simply return an empty list
        ///     If found, we need to find all order items that could consume the parts found and then map them to each part.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<PartAndOrderItemsViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("PartAndOrderItemsViewModel/GetByEquipmentName/{equipmentName}")]
        public IActionResult GetPartAndOrderItemsViewModelToStartAtEquipment(string equipmentName)
        {
            using (var partLogic = new PartLogic(BillOfMaterialData, EquipmentData, PartData, PartItemAttributeTypeData))
            using (var orderItemLogic = new OrderItemLogic(BillOfMaterialData, EquipmentData, OrderItemData, PartData, PartItemAttributeTypeData))
            {
                var partAndOrderItemsViewModelsForEquipment = new List<PartAndOrderItemsViewModel>();
                
                var partsToStartAssemblyAtEquipment = partLogic.GetPartsForBillOfProcessAtStartingEquipment(equipmentName);
                
                // simply return an empty, but successful, result if no parts are found.  It likely means that this equipment is not a starting equipment.
                if (partsToStartAssemblyAtEquipment == null)
                    return Ok(partAndOrderItemsViewModelsForEquipment);

                partsToStartAssemblyAtEquipment.ForEach(part =>
                {
                    var partAndOrderItemsViewModelForEquipment = new PartAndOrderItemsViewModel()
                    {
                        PartToAssemble = part,
                        ActiveOrderItems = orderItemLogic.GetActiveOrderItemsByPartId(part)
                    };

                    if (partAndOrderItemsViewModelForEquipment.ActiveOrderItems.Count > 0 )
                        partAndOrderItemsViewModelsForEquipment.Add(partAndOrderItemsViewModelForEquipment);
                });

                return Ok(partAndOrderItemsViewModelsForEquipment);
            }
        }

        #endregion

        #region Work Elements

        /// <summary>
        /// This method is used to load an equipment's active work element history.  
        /// It will first gather the active serial number and then query the remaining history.
        /// If no result is returned, it simply means the equipment is empty.  Further logic will direct the client user on next steps.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<OrderItemUnitWorkElementHistory>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("OrderItemWorkElementHistory/GetByEquipmentId/{equipmentId}")]
        public List<OrderItemUnitWorkElementHistory> GetOrderItemUnitWorkElementHistoryByEquipment(int equipmentId)
        {
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                return orderItemUnitWorkElementHistoryLogic.GetActiveOrderItemUnitWorkElementHistoryByEquipment(equipmentId);
            }
        }

        /// <summary>
        /// This method is used to load a serial number's latest work element history.
        /// The method will first get the serial number's most recent record.  Then, it will pull all records for that equipment, so that they can be presented to the client.
        /// If no result is returned, it means the serial number either does not exist, or has already completed all steps of its Bill of Process. Further logic will direct the client user on next steps.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<OrderItemUnitWorkElementHistory>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("OrderItemWorkElementHistory/GetBySerialNumber/{serialNumber}")]
        public List<OrderItemUnitWorkElementHistory> GetOrderItemUnitWorkElementHistoryBySerialNumber(string serialNumber)
        {
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                return orderItemUnitWorkElementHistoryLogic.GetActiveOrderItemUnitWorkElementHistoryBySerialNumber(serialNumber);
            }
        }

        /// <summary>
        /// Adds a work element history
        /// This will be used to add a new history in scnearios where an order item unit is started, but does not have its first work element history.
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemWorkElementHistory/Add")]
        public IActionResult AddOrderItemUnitWorkElementHistory([FromBody] OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                try
                {
                    orderItemUnitWorkElementHistoryLogic.AddOrderItemUnitWorkElementHistory(orderItemUnitWorkElementHistory);
                    Logger.LogInformation("Added Work Element history {0} for serial number: {1}", orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.Name, orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the work element history.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the work element history.  Please verify entry or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Completes a work element.
        /// If there is another work element that proceeds the completed, in the same equipment, then it will be started
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemWorkElementHistory/Complete")]
        public async Task<IActionResult> CompleteOrderItemUnitWorkElement([FromBody] OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                try
                {
                    orderItemUnitWorkElementHistoryLogic.CompleteOrderItemUnitWorkElement(orderItemUnitWorkElementHistory);
                    Logger.LogInformation("Completed Work Element {0} for serial number: {1}", orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.Name, orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);

                    // Send SignalR message to any listening clients
                    await OrderItemUnitHubContext.Clients.Group(orderItemUnitWorkElementHistory.Equipment.Name)
                        .SendAsync("ReceiveCompletedOrderItemUnitWorkElement", orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);

                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when completing the work element.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when completing the work element.  Please verify entry or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Bypasses a work element.
        /// In some cases, a user will need to bypass a work element.  Either the work cannot be done, or something else is preventing them from successfully completing the element.
        /// If there is another work element that proceeds the bypassed element, in the same equipment, then it will be started
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemWorkElementHistory/Bypass")]
        public async Task<IActionResult> BypassOrderItemUnitWorkElement([FromBody] OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                try
                {
                    orderItemUnitWorkElementHistoryLogic.BypassOrderItemUnitWorkElement(orderItemUnitWorkElementHistory);
                    Logger.LogInformation("Bypassed Work Element {0} for serial number: {1}", orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.Name, orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);

                    // Send SignalR message to any listening clients
                    await OrderItemUnitHubContext.Clients.Group(orderItemUnitWorkElementHistory.Equipment.Name)
                        .SendAsync("ReceiveBypassedOrderItemUnitWorkElement", orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);

                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when bypassing the work element.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when bypassing the work element.  Please verify entry or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Fails a work element.
        /// The failed work element will typically only be seen for automated elements.  Some problem with the integration has caused the step to fail instead of just completing or bypassing.
        /// If there is another work element that proceeds the failed element, in the same equipment, then it will be started
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemWorkElementHistory/Fail")]
        public async Task<IActionResult> FailOrderItemUnitWorkElement([FromBody] OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                try
                {
                    orderItemUnitWorkElementHistoryLogic.FailOrderItemUnitWorkElement(orderItemUnitWorkElementHistory);
                    Logger.LogInformation("Failed Work Element {0} for serial number: {1}", orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.Name, orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);

                    // Send SignalR message to any listening clients
                    await OrderItemUnitHubContext.Clients.Group(orderItemUnitWorkElementHistory.Equipment.Name)
                        .SendAsync("ReceiveFailedOrderItemUnitWorkElement", orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);

                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when failing the work element.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when failing the work element.  Please contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Pauses a work element.
        /// If the user clears the order item unit when it is still in progress, it will be paused.
        /// A pause could come from a browser session close as well
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemWorkElementHistory/Pause")]
        public async Task<IActionResult> PauseOrderItemUnitWorkElement([FromBody] OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                try
                {
                    orderItemUnitWorkElementHistoryLogic.PauseOrderItemUnitWorkElement(orderItemUnitWorkElementHistory);
                    Logger.LogInformation("Paused Work Element {0} for serial number: {1}", orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.Name, orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);

                    // Send SignalR message to any listening clients
                    await OrderItemUnitHubContext.Clients.Group(orderItemUnitWorkElementHistory.Equipment.Name)
                        .SendAsync("ReceivePausedOrderItemUnitWorkElement", orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);

                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when pausing the work element.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when pausing the work element.  Please verify entry or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Resumes a work element.
        /// If the user had previously cleared the order item unit before it had finished all work elements, then a "Paused" record will be present.
        /// This call will add a new In Progress record to resume the production
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("OrderItemWorkElementHistory/Resume")]
        public async Task<IActionResult> ResumeOrderItemUnitWorkElement([FromBody] OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                try
                {
                    orderItemUnitWorkElementHistoryLogic.ResumeOrderItemUnitWorkElement(orderItemUnitWorkElementHistory);

                    // Send SignalR message to any listening clients
                    await OrderItemUnitHubContext.Clients.Group(orderItemUnitWorkElementHistory.Equipment.Name)
                        .SendAsync("ReceiveResumedOrderItemUnitWorkElement", orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);

                    Logger.LogInformation("Resumed Work Element {0} for serial number: {1}", orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.Name, orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when resuming the work element.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when resuming the work element.  Please verify entry or contact MES support for help."));
                }
            }
        }

        #endregion

        #region Data Collection

        /// <summary>
        /// This method will pull existing data collection records for the given work element and serial number.  
        /// It will only return those that have collected values and may not be the full list of data collection attributes
        /// </summary>
        /// <param name="workElementId">The specific work element from the caller</param>
        /// <param name="serialNumber">The serial number of the assembly being worked on</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<OrderItemUnitDataCollection>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("DataCollection/GetByWorkElementAndSerial/{workElementId}/{serialNumber}")]
        public List<OrderItemUnitDataCollection> GetOrderItemDataCollectionByWorkElementAndSerialNumber(int workElementId, string serialNumber)
        {
            using (var orderItemUnitDataCollectionLogic = new OrderItemUnitDataCollectionLogic(OrderItemUnitDataCollectionData))
            {
                return orderItemUnitDataCollectionLogic.GetOrderItemDataCollectionByWorkElementAndSerialNumber(workElementId, serialNumber);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("DataCollection/Save")]
        public IActionResult SaveOrderItemUnitDataCollections([FromBody] List<OrderItemUnitDataCollection> orderItemUnitDataCollections)
        {
            using (var orderItemUnitDataCollectionLogic = new OrderItemUnitDataCollectionLogic(OrderItemUnitDataCollectionData))
            {
                var topOrderItemForLoggic = orderItemUnitDataCollections.First();

                if (topOrderItemForLoggic != null && topOrderItemForLoggic.OrderItemUnit != null)
                {
                    try
                    {
                        orderItemUnitDataCollectionLogic.SaveOrderItemUnitDataCollections(orderItemUnitDataCollections);
                        Logger.LogInformation("{0} data collection values saved for serial number: {1} and work element attribute ID : {2}", orderItemUnitDataCollections.Count, topOrderItemForLoggic.OrderItemUnit.SerialNumber, topOrderItemForLoggic.BillOfProcessProcessWorkElementAttribute.BillOfProcessProcessWorkElementAttributeId);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "An error occurred when saving the data collection values.");
                        return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when saving the data collection values.  Please verify entry or contact MES support for help."));
                    }
                }
                else
                {
                    Logger.LogInformation("The data collection list was empty, so no values were saved.");
                    return Ok();
                }
            }
        }

        #endregion

        #region Consumption

        /// <summary>
        /// This method will pull existing consumption records for the given work element and serial number.  
        /// It will only return those that have values and not include any where isRemoved = true
        /// </summary>
        /// <param name="workElementId">The specific work element from the caller</param>
        /// <param name="serialNumber">The serial number of the assembly being worked on</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<OrderItemUnitConsumption>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("Consumption/GetByWorkElementAndSerial/{workElementId}/{serialNumber}")]
        public List<OrderItemUnitConsumption> GetOrderItemUnitConsumptionByWorkElementAndSerialNumber(int workElementId, string serialNumber)
        {
            using (var orderItemUnitConsumptionLogic = new OrderItemUnitConsumptionLogic(OrderItemUnitConsumptionData))
            {
                return orderItemUnitConsumptionLogic.GetOrderItemUnitConsumptionByWorkElementAndSerialNumber(workElementId, serialNumber);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("Consumption/Save")]
        public IActionResult SaveOrderItemUnitConsumptions([FromBody] List<OrderItemUnitConsumption> orderItemUnitConsumptions)
        {
            using (var orderItemUnitConsumptionLogic = new OrderItemUnitConsumptionLogic(OrderItemUnitConsumptionData))
            {
                var topOrderItemForLoggic = orderItemUnitConsumptions.First();

                if (topOrderItemForLoggic != null && topOrderItemForLoggic.OrderItemUnit != null)
                {
                    try
                    {
                        var existingConsumptionCount = orderItemUnitConsumptions.Where(consumption => consumption.OrderItemUnitConsumptionId > 0).Count();
                        orderItemUnitConsumptionLogic.SaveOrderItemUnitConsumptions(orderItemUnitConsumptions);
                        Logger.LogInformation("{0} consumed parts saved for serial number: {1} and work element : {2}.  {3} existing consumed parts where removed.", orderItemUnitConsumptions.Count, topOrderItemForLoggic.OrderItemUnit.SerialNumber, topOrderItemForLoggic.BillOfProcessProcessWorkElementAttribute.WorkElementTypeAttribute.Name, existingConsumptionCount);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "An error occurred when consuming the work element parts.");
                        return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when consuming the work element parts.  Please verify entry or contact MES support for help."));
                    }
                }
                else
                {
                    Logger.LogInformation("The consumption list was empty, so no values were saved.");
                    return Ok();
                }
            }
        }

        #endregion

        #endregion
    }
}
