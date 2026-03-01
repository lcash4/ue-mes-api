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
    public interface IOrderItemUnitWorkElementHistoryData
    {
        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistoryId">Unique ID for the <see cref="OrderItemUnitWorkElementHistory"/> record</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetOrderItemUnitWorkElementHistoryById(long orderItemUnitWorkElementHistoryId);

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// </summary>
        /// <param name="equipmentId">Unique ID for the equipment to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentOrderItemUnitWorkElementHistoryByEquipmentId(int equipmentId);

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// </summary>
        /// <param name="serialNumber">Serial number to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentOrderItemUnitWorkElementHistoryBySerialNumber(string serialNumber);

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// Only return a value when the most recent record is "In Progress"
        /// </summary>
        /// <param name="equipmentId">Unique ID for the equipment to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentActiveOrderItemUnitWorkElementHistoryByEquipmentId(int equipmentId);

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// Only return a value when the most recent record is "In Progress"
        /// </summary>
        /// <param name="serialNumber">Serial number to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentActiveOrderItemUnitWorkElementHistoryBySerialNumber(string serialNumber);

        /// <summary>
        /// Returns a <see cref="OrderItemUnitWorkElementHistory"/> entity only with no child entities included.
        /// The purpose of this method is to check if a work element has any production history associated with it.  
        /// If so, the logic for handling work element changes will be adjusted to handle these work elements.
        /// </summary>
        /// <param name="workElementId">Work element ID to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetAnyOrderItemUnitWorkElementHistoryByWorkElementId(int workElementId);

        /// <summary>
        /// Return the latest OrderItemUnitWorkElementHistory for an equipment by checking the equipment work element history
        /// </summary>
        /// <param name="equipmentName">Unique equipment name to search</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetMostRecentOrderItemUnitByEquipment(string equipmentName);

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// </summary>
        /// <param name="equipmentId">Unique ID for the equipment to search</param>
        /// <param name="serialNumber">Serial number to search</param>
        /// <returns></returns>
        public List<OrderItemUnitWorkElementHistory> GetOrderItemUnitWorkElementHistoryByEquipmentAndSerialNumber(int equipmentId, string serialNumber);

        /// <summary>
        /// Returns a full <see cref="OrderItemUnitWorkElementHistory"/> entity with child entities included.
        /// The where clause is looking for the natural key of the table.  If we get a result from this query, we should NOT be adding a new record, as it would be a duplicate
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory">The incoming <see cref="OrderItemUnitWorkElementHistory"/> record to complete the query</param>
        /// <returns></returns>
        public OrderItemUnitWorkElementHistory GetOrderItemUnitWorkElementHistoryByOrderItemUnitAndWorkElementAndEquipmentWithInProgressStatus(OrderItemUnitWorkElementHistory inputOrderItemUnitWorkElementHistory);

        /// <summary>
        /// Add a new order item unit work element history record.  This will typically take place when a work element is getting started, or needs reworked
        /// </summary>
        /// <param name="billOfMaterial"></param>
        public void AddOrderItemUnitWorkElementHistory(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory);

        /// <summary>
        /// Update an existing order item unit work element history.  
        /// </summary>
        /// <param name="orderItemUnitWorkElementHistory"></param>
        public void UpdateOrderItemUnitWorkElementHistory(OrderItemUnitWorkElementHistory orderItemUnitWorkElementHistory);

        /// <summary>
        /// Update any existing order item unit work element history where status is "In Progress" and the work element ID is being replaced by a new value
        /// </summary>
        /// <param name="oldWorkElementId">Used to query any existing records</param>
        /// <param name="newWorkElementId">New BOP Work Element ID to apply to the existing records</param>
        public void UpdateOrderItemUnitWorkElementHistoryWorkElementIds(int oldWorkElementId, int newWorkElementId);
    }
}
