using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Authorization;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;
using ue_mes_logic.Global;

namespace ue_mes_logic.dbo
{
    public class OrderItemUnitLogic : LogicBase
    {
        ICoaterData CoaterData { get; }
        IBillOfProcessData BillOfProcessData { get; }
        IEquipmentData EquipmentData { get; }
        IInventoryItemData InventoryItemData { get; }
        IInventoryItemTypeData InventoryItemTypeData { get; }
        IInventoryLocationData InventoryLocationData { get; }
        IOrderItemUnitData OrderItemUnitData { get; }
        IOrderItemUnitWorkElementHistoryData OrderItemUnitWorkElementHistoryData { get; }
        IWorkElementStatusData WorkElementStatusData { get; }


        #region Constructor

        public OrderItemUnitLogic(IBillOfProcessData billOfProcessData, 
            ICoaterData coaterData,
            IEquipmentData equipmentData,
            IInventoryItemData inventoryItemData,
            IInventoryItemTypeData inventoryItemTypeData,
            IInventoryLocationData inventoryLocationData,
            IOrderItemUnitData orderItemUnitData,
            IOrderItemUnitWorkElementHistoryData orderItemUnitWorkElementHistoryData,
            IWorkElementStatusData workElementStatusData)
        {   
            CoaterData = coaterData ?? throw new ArgumentNullException(nameof(coaterData));
            BillOfProcessData = billOfProcessData ?? throw new ArgumentNullException(nameof(billOfProcessData));
            EquipmentData = equipmentData ?? throw new ArgumentNullException(nameof(equipmentData));
            InventoryItemData = inventoryItemData ?? throw new ArgumentNullException(nameof(inventoryItemData));
            InventoryItemTypeData = inventoryItemTypeData ?? throw new ArgumentNullException(nameof(inventoryItemTypeData));
            InventoryLocationData = inventoryLocationData ?? throw new ArgumentNullException(nameof(inventoryLocationData));
            OrderItemUnitData = orderItemUnitData ?? throw new ArgumentNullException(nameof(orderItemUnitData));
            OrderItemUnitWorkElementHistoryData = orderItemUnitWorkElementHistoryData ?? throw new ArgumentNullException(nameof(orderItemUnitWorkElementHistoryData));
            WorkElementStatusData = workElementStatusData ?? throw new ArgumentNullException(nameof(workElementStatusData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return an OrderItemUnit object by its serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number to search</param>
        /// <returns></returns>
        public OrderItemUnit GetOrderItemUnitBySerialNumber(string serialNumber)
        {
            return OrderItemUnitData.GetOrderItemUnitBySerialNumber(serialNumber);
        }

        /// <summary>
        /// Returns all order item units by searching the provided order item IDs
        /// </summary>
        /// <param name="orderItemIds">list of <see cref="OrderItem"/> IDs to search</param>
        /// <returns></returns>
        public List<OrderItemUnit> GetOrderItemUnitsForOrderItems(List<int> orderItemIds)
        {
            return OrderItemUnitData.GetOrderItemUnitsForOrderItems(orderItemIds);
        }

        /// <summary>
        /// Starting a new order item unit involves:
        ///  - generating a serial number based on the part and current date.
        ///  - Adding the new serial number as an OrderItemUnit for the OrderItem and Part
        ///  - Adding the new serial number as a production inventory item
        ///  - Adding the first work element history 
        /// If all transactions succeed, the serial number is returned to the caller so that they can refresh the client with the new order item unit.
        /// </summary>
        /// <param name="orderItemUnit"></param>
        /// <param name="part"></param>
        /// <param name="equipment"></param>
        public string StartOrderItemUnit(OrderItem orderItem, Part part, Equipment equipment, User user)
        {
            using (var inventoryItemLogic = new InventoryItemLogic(InventoryItemData))
            using (var orderItemUnitWorkElementHistoryLogic = new OrderItemUnitWorkElementHistoryLogic(BillOfProcessData, EquipmentData, OrderItemUnitWorkElementHistoryData, WorkElementStatusData))
            {
                var newOrderItemUnit = GenerateNewOrderItemUnitForPartAndOrderItem(orderItem, part, equipment);
                OrderItemUnitData.AddOrderItemUnit(newOrderItemUnit);

                //var newInventoryItem = GenerateNewInventoryItemForSerial(newOrderItemUnit.SerialNumber, part);
                //inventoryItemLogic.AddInventoryItem(newInventoryItem);

                var newOrderItemUnitWorkElementHistory = GenerateNewOrderItemUnitWorkElementHistoryForOrderItemUnitEquipmentAndUser(newOrderItemUnit, part, equipment, user);
                orderItemUnitWorkElementHistoryLogic.AddOrderItemUnitWorkElementHistory(newOrderItemUnitWorkElementHistory);

                return newOrderItemUnit.SerialNumber;
            }
        }

        #endregion

        #region Private Methods

        private string GenerateSerialNumberForPart(Part part, Equipment equipment)
        {
            using (var coaterLogic = new CoaterLogic(CoaterData))
            {
                var currentDate = DateTime.Now;
                string currentJulianDate = string.Format("{0:yy}{1:D3}", currentDate, currentDate.DayOfYear);

                var lastOrderItemUnitGeneratedForCurrentDate = OrderItemUnitData.GetMostRecentOrderItemUnitByJulianDate(currentJulianDate);
                int latestSerialNumberSequence = lastOrderItemUnitGeneratedForCurrentDate == null ? 1 : int.Parse(lastOrderItemUnitGeneratedForCurrentDate.SerialNumber.Substring(lastOrderItemUnitGeneratedForCurrentDate.SerialNumber.Length - 5)) + 1;
                string partTypeString = ((int)part.PartType).ToString("00");
                var activeLayerCoater = coaterLogic.GetActiveLayerCoaterBySiteName(equipment.Line.Unit.Plant.Site.DisplayName);
                string coaterNumber = activeLayerCoater.Number.ToString("00");

                string newSerialNumber = string.Format("{0}{1}{2}{3}", currentJulianDate, partTypeString, coaterNumber, latestSerialNumberSequence.ToString("00000"));
                return newSerialNumber;
            }
        }
        private OrderItemUnit GenerateNewOrderItemUnitForPartAndOrderItem(OrderItem orderItem, Part part, Equipment equipment)
        {
            var newOrderItemUnit = new OrderItemUnit();
            newOrderItemUnit.SerialNumber = GenerateSerialNumberForPart(part, equipment);
            newOrderItemUnit.OrderItem = orderItem;
            newOrderItemUnit.Part = part;

            return newOrderItemUnit;
        }

        private InventoryItem GenerateNewInventoryItemForSerial(string serialNumber, Part part)
        {
            using (var inventoryItemTypeLogic = new InventoryItemTypeLogic(InventoryItemTypeData))
            using (var inventoryLocationLogic = new InventoryLocationLogic(InventoryLocationData))
            {
                var newInventoryItem = new InventoryItem();
                newInventoryItem.SerialNumber = serialNumber;
                newInventoryItem.InventoryItemType = inventoryItemTypeLogic.GetInventoryItemTypeByName("Serial");
                newInventoryItem.InventoryLocation = inventoryLocationLogic.GetInventoryLocationByName("Production");
                newInventoryItem.Part = part;
                newInventoryItem.Quantity = 1;

                return newInventoryItem;
            }
        }

        private OrderItemUnitWorkElementHistory GenerateNewOrderItemUnitWorkElementHistoryForOrderItemUnitEquipmentAndUser(OrderItemUnit orderItemUnit, Part part, Equipment equipment, User user)
        {
            using (var billOfProcessLogic = new BillOfProcessLogic(BillOfProcessData, EquipmentData))
            {
                // re-hydrate the order item unit, so that we have the ID as well.
                var newOrderItemUnit = GetOrderItemUnitBySerialNumber(orderItemUnit.SerialNumber);

                var activeBillOfProcessForPart = billOfProcessLogic.GetBillOfProcessByPartId(part.PartId);
                var firstBillOfProcessProcessForEquipment = activeBillOfProcessForPart.BillOfProcessProcesses.Where(billOfProcessProcess => billOfProcessProcess.Process.ProcessId == equipment.Process.ProcessId && billOfProcessProcess.Sequence == 1).FirstOrDefault();
                var firstBillOfProcessWorkElement = firstBillOfProcessProcessForEquipment.BillOfProcessProcessWorkElements.Where(workElement => workElement.Sequence == 1 && workElement.IsActive).FirstOrDefault();
                var newOrderItemWorkElementHistory = new OrderItemUnitWorkElementHistory()
                {
                    OrderItemUnit = newOrderItemUnit,
                    BillOfProcessProcessWorkElement = firstBillOfProcessWorkElement,
                    Equipment = equipment,
                    User = user
                };

                return newOrderItemWorkElementHistory;
            }
        }

        #endregion
    }
}
