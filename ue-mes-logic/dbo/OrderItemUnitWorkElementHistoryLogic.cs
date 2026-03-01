using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class OrderItemUnitWorkElementHistoryLogic : LogicBase
    {
        IBillOfProcessData BillOfProcessData { get; }
        IEquipmentData EquipmentData { get; }
        IOrderItemUnitWorkElementHistoryData OrderItemUnitWorkElementHistoryData { get; }
        IWorkElementStatusData WorkElementStatusData { get; }

        #region Constructor

        public OrderItemUnitWorkElementHistoryLogic(IBillOfProcessData billOfProcessData, 
            IEquipmentData equipmentData,
            IOrderItemUnitWorkElementHistoryData orderItemUnitWorkElementHistoryData,
            IWorkElementStatusData workElementStatusData)
        {
            BillOfProcessData = billOfProcessData ?? throw new ArgumentNullException(nameof(billOfProcessData));
            EquipmentData = equipmentData ?? throw new ArgumentNullException(nameof(equipmentData));
            OrderItemUnitWorkElementHistoryData = orderItemUnitWorkElementHistoryData ?? throw new ArgumentNullException(nameof(orderItemUnitWorkElementHistoryData));
            WorkElementStatusData = workElementStatusData ?? throw new ArgumentNullException(nameof(workElementStatusData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistoryId">Unique ID for the <see cref="OrderItemUnitWorkElementHistory"/> record</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetOrderItemUnitWorkElementHistoryById(int orderItemUnitWorkElementHistoryId)
        {
            return OrderItemUnitWorkElementHistoryData.GetOrderItemUnitWorkElementHistoryById(orderItemUnitWorkElementHistoryId);
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// When this method is called, it needs to first find the most recent serial number for a given equipment, with "In Progress" status.
        /// If found, then it will return the full history for that serial number at the given equipment.
        /// If not found, then simply return no result (null).  Further logic will dictate the next steps from there.
        /// </summary>
        /// <param name="equipmentId">Unique ID for the equipment to search</param>
        /// <returns></returns>
        public List<OrderItemUnitWorkElementHistory> GetActiveOrderItemUnitWorkElementHistoryByEquipment(int equipmentId)
        {
            var activeOrderItemUnitWorkElementHistory = OrderItemUnitWorkElementHistoryData.GetMostRecentActiveOrderItemUnitWorkElementHistoryByEquipmentId(equipmentId);

            if (activeOrderItemUnitWorkElementHistory == null)
                return null;

            // If the active history item is not null, then we will get the full history for this equipment so that we can return it to the caller.
            return OrderItemUnitWorkElementHistoryData.GetOrderItemUnitWorkElementHistoryByEquipmentAndSerialNumber(equipmentId, activeOrderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// If not found, then simply return no result (null).  Further logic will dictate the next steps from there.
        /// </summary>
        /// <param name="equipmentId">Unique ID for the equipment to search</param>
        /// <returns></returns>
        public List<OrderItemUnitWorkElementHistory> GetOrderItemUnitWorkElementHistoryByEquipment(int equipmentId)
        {
            var activeOrderItemUnitWorkElementHistory = OrderItemUnitWorkElementHistoryData.GetMostRecentOrderItemUnitWorkElementHistoryByEquipmentId(equipmentId);

            if (activeOrderItemUnitWorkElementHistory == null)
                return null;

            // If the active history item is not null, then we will get the full history for this equipment so that we can return it to the caller.
            return OrderItemUnitWorkElementHistoryData.GetOrderItemUnitWorkElementHistoryByEquipmentAndSerialNumber(equipmentId, activeOrderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber);
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// When this method is called, it needs to first find the most recent record for the given serial number, with "In Progress" status.
        /// If found, then it will return the full history for that serial number at the equipment where it is currently located.
        /// If not found, then simply return no result (null).  Further logic will dictate the next steps from there.
        /// </summary>
        /// <param name="serialNumber">Serial number to search</param>
        /// <returns></returns>
        public List<OrderItemUnitWorkElementHistory> GetActiveOrderItemUnitWorkElementHistoryBySerialNumber(string serialNumber)
        {
            var activeOrderItemUnitWorkElementHistory = OrderItemUnitWorkElementHistoryData.GetMostRecentActiveOrderItemUnitWorkElementHistoryBySerialNumber(serialNumber);

            if (activeOrderItemUnitWorkElementHistory == null)
                return null;

            // If the active history item is not null, then we will get the full history for this equipment so that we can return it to the caller.
            return OrderItemUnitWorkElementHistoryData.GetOrderItemUnitWorkElementHistoryByEquipmentAndSerialNumber(activeOrderItemUnitWorkElementHistory.Equipment.EquipmentId, serialNumber);
        }

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// If not found, then simply return no result (null).  Further logic will dictate the next steps from there.
        /// </summary>
        /// <param name="serialNumber">Serial number to search</param>
        /// <returns></returns>
        public List<OrderItemUnitWorkElementHistory> GetOrderItemUnitWorkElementHistoryBySerialNumber(string serialNumber)
        {
            var activeOrderItemUnitWorkElementHistory = OrderItemUnitWorkElementHistoryData.GetMostRecentOrderItemUnitWorkElementHistoryBySerialNumber(serialNumber);

            if (activeOrderItemUnitWorkElementHistory == null)
                return null;

            // If the active history item is not null, then we will get the full history for this equipment so that we can return it to the caller.
            return OrderItemUnitWorkElementHistoryData.GetOrderItemUnitWorkElementHistoryByEquipmentAndSerialNumber(activeOrderItemUnitWorkElementHistory.Equipment.EquipmentId, serialNumber);
        }

        /// <summary>
        /// Return the latest OrderItemUnitWorkElementHistory for an equipment by checking the equipment work element history
        /// </summary>
        /// <param name="equipmentName">Unique equipment name to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentOrderItemUnitByEquipment(string equipmentName)
        {
            return OrderItemUnitWorkElementHistoryData.GetMostRecentOrderItemUnitByEquipment(equipmentName);
        }

        /// <summary>
        /// Add a new order item unit work element history record.  This will typically take place when a work element is getting started, or needs reworked
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        public void AddOrderItemUnitWorkElementHistory(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            // Need to check if bill of material already exists.  If so, return "Bill Of Material already exists" exception
            var existingOrderItemUnitWorkElementHistory = OrderItemUnitWorkElementHistoryData.GetOrderItemUnitWorkElementHistoryByOrderItemUnitAndWorkElementAndEquipmentWithInProgressStatus(orderItemUnitWorkElementHistory);
            if (existingOrderItemUnitWorkElementHistory != null)
                throw new Exception(string.Format("In Progress Work Element Histroy for {0} already exists at equipment {1} and work element {2}", orderItemUnitWorkElementHistory.OrderItemUnit.SerialNumber, orderItemUnitWorkElementHistory.Equipment.Name, orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.Name));

            var workElementStatuses = WorkElementStatusData.GetWorkElementStatuses();

            // When adding new, we should set the start date to current time, and ensure the status is "In Progress".
            // At this time, all added records should be coming in with In Progress status, and all other statuses are achieved through an update.
            orderItemUnitWorkElementHistory.WorkElementStatus = workElementStatuses.Where(workElementStatus => workElementStatus.Name == "In Progress").FirstOrDefault();
            orderItemUnitWorkElementHistory.StartDate = DateTime.Now;
            orderItemUnitWorkElementHistory.StartDateUtc = DateTime.UtcNow;

            // Add the record
            OrderItemUnitWorkElementHistoryData.AddOrderItemUnitWorkElementHistory(orderItemUnitWorkElementHistory);
        }

        /// <summary>
        /// Update an existing order item unit work element history.  
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        public void UpdateOrderItemUnitWorkElementHistory(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            var existingOrderItemUnitWorkElementHistory = OrderItemUnitWorkElementHistoryData.GetOrderItemUnitWorkElementHistoryById(orderItemUnitWorkElementHistory.OrderItemUnitWorkElementHistoryId);
            if (existingOrderItemUnitWorkElementHistory == null)
                throw new Exception(string.Format("Order Item Unit Work Element History with ID {0} does not exists and cannot be updated", orderItemUnitWorkElementHistory.OrderItemUnitWorkElementHistoryId));

            // When updating, if the status is changing to a completed status, then we will update the end times as well.
            orderItemUnitWorkElementHistory.EndDate = IsWorkElementPausingOrCompleting(existingOrderItemUnitWorkElementHistory, orderItemUnitWorkElementHistory) ? DateTime.Now : existingOrderItemUnitWorkElementHistory.EndDate;
            orderItemUnitWorkElementHistory.EndDateUtc = IsWorkElementPausingOrCompleting(existingOrderItemUnitWorkElementHistory, orderItemUnitWorkElementHistory) ? DateTime.UtcNow : existingOrderItemUnitWorkElementHistory.EndDateUtc;

            // Update the record
            OrderItemUnitWorkElementHistoryData.UpdateOrderItemUnitWorkElementHistory(orderItemUnitWorkElementHistory);
        }

        /// <summary>
        /// Updates the work element with complete status.  
        /// If a next work element in sequence is available, it will then write a new "In Progress" record for the next element
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        public void CompleteOrderItemUnitWorkElement(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            UpdateOrderItemUnitWorkElementStatusAndMoveToNextElement(orderItemUnitWorkElementHistory, "Complete");
        }

        /// <summary>
        /// Updates the work element with bypassed status.  
        /// If a next work element in sequence is available, it will then write a new "In Progress" record for the next element
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        public void BypassOrderItemUnitWorkElement(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            UpdateOrderItemUnitWorkElementStatusAndMoveToNextElement(orderItemUnitWorkElementHistory, "Bypassed");
        }

        /// <summary>
        /// Updates the work element with failed status.  
        /// If a next work element in sequence is available, it will then write a new "In Progress" record for the next element
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        public void FailOrderItemUnitWorkElement(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            UpdateOrderItemUnitWorkElementStatusAndMoveToNextElement(orderItemUnitWorkElementHistory, "Failed");
        }

        /// <summary>
        /// Updates the work element with a provided status.  
        /// If a next work element in sequence is available, it will then write a new "In Progress" record for the next element
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        /// <param name="workElementStatusToUpdate"></param>
        public void UpdateOrderItemUnitWorkElementStatusAndMoveToNextElement(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory, string workElementStatusToUpdate)
        {
            using (var billOfProcessLogic = new BillOfProcessLogic(BillOfProcessData, EquipmentData))
            {
                var workElementStatuses = WorkElementStatusData.GetWorkElementStatuses();

                // Force the work element status to the provided workElementStatusToUpdate
                orderItemUnitWorkElementHistory.WorkElementStatus = workElementStatuses.Where(workElementStatus => workElementStatus.Name == workElementStatusToUpdate).FirstOrDefault();

                // Update the current orderItemUnitWorkElementHistory
                UpdateOrderItemUnitWorkElementHistory(orderItemUnitWorkElementHistory);

                // Now, we need to get the BOP based on the Order Item Unit Part ID.  This will give us the full list of work elements so that we can see if a new orderItemUnitWorkElementHistory record needs added with In Progress status.
                var currentBillOfProcess = billOfProcessLogic.GetBillOfProcessByPartId(orderItemUnitWorkElementHistory.OrderItemUnit.Part.PartId);
                if (currentBillOfProcess != null)
                {
                    // Extract the next work element in line
                    var currentBillOfProcessProcess = currentBillOfProcess.BillOfProcessProcesses.Where(billOfProcessProcess => billOfProcessProcess.BillOfProcessProcessId == orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.BillOfProcessProcess.BillOfProcessProcessId).FirstOrDefault();
                    var nextBillOfProcessWorkElement = currentBillOfProcessProcess.BillOfProcessProcessWorkElements.OrderBy(billOfProcessworkElement => billOfProcessworkElement.Sequence).SkipWhile(billOfProcessworkElement => billOfProcessworkElement.BillOfProcessProcessWorkElementId != orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId).Skip(1).FirstOrDefault();

                    // If the next work element in line is not null, and the work element ID is not the same as completed (happens in a 1 work element setup), add a new history record with the correct properties.
                    if (nextBillOfProcessWorkElement != null && nextBillOfProcessWorkElement.BillOfProcessProcessWorkElementId != orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement.BillOfProcessProcessWorkElementId)
                    {
                        var newOrderItemUnitWorkElementHistory = new OrderItemUnitWorkElementHistory()
                        {
                            OrderItemUnitWorkElementHistoryId = 0,
                            OrderItemUnit = orderItemUnitWorkElementHistory.OrderItemUnit,
                            BillOfProcessProcessWorkElement = nextBillOfProcessWorkElement,
                            Equipment = orderItemUnitWorkElementHistory.Equipment,
                            WorkElementStatus = workElementStatuses.Where(workElementStatus => workElementStatus.Name == "In Progress").FirstOrDefault(),
                            User = orderItemUnitWorkElementHistory.User,
                            StartDate = DateTime.Now,
                            StartDateUtc = DateTime.UtcNow
                        };

                        AddOrderItemUnitWorkElementHistory(newOrderItemUnitWorkElementHistory);
                    }
                }
            }
        }

        /// <summary>
        /// If the operations run screen is cleared in any way, while the work element is "In Progress", then we will update it to "Paused" status.
        /// This allows us to track how often the work is paused, and for how long.
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        public void PauseOrderItemUnitWorkElement(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            using (var billOfProcessLogic = new BillOfProcessLogic(BillOfProcessData, EquipmentData))
            {
                var workElementStatuses = WorkElementStatusData.GetWorkElementStatuses();

                // Force the work element status to paused
                orderItemUnitWorkElementHistory.WorkElementStatus = workElementStatuses.Where(workElementStatus => workElementStatus.Name == "Paused").FirstOrDefault();

                // Update the current orderItemUnitWorkElementHistory
                UpdateOrderItemUnitWorkElementHistory(orderItemUnitWorkElementHistory);
            }
        }

        /// <summary>
        /// If the operations run screen is loaded with an order and that order item unit has a "Paused" work element history, we will then write a new "In Progress" record for the same work element as operations has resumed.
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        public void ResumeOrderItemUnitWorkElement(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory)
        {
            using (var billOfProcessLogic = new BillOfProcessLogic(BillOfProcessData, EquipmentData))
            {
                var workElementStatuses = WorkElementStatusData.GetWorkElementStatuses();

                var newOrderItemUnitWorkElementHistory = new OrderItemUnitWorkElementHistory()
                {
                    OrderItemUnitWorkElementHistoryId = 0,
                    OrderItemUnit = orderItemUnitWorkElementHistory.OrderItemUnit,
                    BillOfProcessProcessWorkElement = orderItemUnitWorkElementHistory.BillOfProcessProcessWorkElement,
                    Equipment = orderItemUnitWorkElementHistory.Equipment,
                    WorkElementStatus = workElementStatuses.Where(workElementStatus => workElementStatus.Name == "In Progress").FirstOrDefault(),
                    User = orderItemUnitWorkElementHistory.User,
                    StartDate = DateTime.Now,
                    StartDateUtc = DateTime.UtcNow
                };

                AddOrderItemUnitWorkElementHistory(newOrderItemUnitWorkElementHistory);
            }
        }

        #endregion

        #region Private Methods

        private bool IsWorkElementPausingOrCompleting(OrderItemUnitWorkElementHistory currentWorkElementHistory, OrderItemUnitWorkElementHistory newWorkElementHistory)
        {
            // arrays of states for comparison
            string[] workingStates = new string[1] { "In Progress" };
            string[] completedStates = new string[4] { "Paused", "Bypassed", "Complete", "Failed" };

            // If moving from In Progress to either Bypassed, Complete or Failed, then this is a completing work element
            return workingStates.Contains(currentWorkElementHistory.WorkElementStatus.Name) && completedStates.Contains(newWorkElementHistory.WorkElementStatus.Name);
        }

        #endregion
    }
}
