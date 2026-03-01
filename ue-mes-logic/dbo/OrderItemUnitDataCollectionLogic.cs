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
    public class OrderItemUnitDataCollectionLogic : LogicBase
    {
        IOrderItemUnitDataCollectionData OrderItemUnitDataCollectionData { get; }

        #region Constructor

        public OrderItemUnitDataCollectionLogic(IOrderItemUnitDataCollectionData orderItemUnitDataCollectionData)
        {
            OrderItemUnitDataCollectionData = orderItemUnitDataCollectionData ?? throw new ArgumentNullException(nameof(orderItemUnitDataCollectionData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method will pull existing data collection records for the given work element and serial number.  
        /// It will only return those that have collected values and may not be the full list of data collection attributes
        /// </summary>
        /// <param name="workElementId">The specific work element from the caller</param>
        /// <param name="serialNumber">The serial number of the assembly being worked on</param>
        /// <returns></returns>
        public List<OrderItemUnitDataCollection> GetOrderItemDataCollectionByWorkElementAndSerialNumber(int workElementId, string serialNumber)
        {
            return OrderItemUnitDataCollectionData.GetOrderItemUnitDataCollectionByWorkElementAndSerialNumber(workElementId, serialNumber);
        }

        /// <summary>
        /// This method will take a list of order item unit data collection objects and add them if new (ID = 0), or update if existing (Id > 0)
        /// </summary>
        /// <param name="orderItemUnitDataCollections">List of <see cref="OrderItemUnitDataCollection"/>from the caller</param>
        public void SaveOrderItemUnitDataCollections(List<OrderItemUnitDataCollection> orderItemUnitDataCollections)
        {
            List<OrderItemUnitDataCollection> newOrderItemUnitDataCollectionsToAdd = orderItemUnitDataCollections.Where(orderItemUnitDataCollection => orderItemUnitDataCollection.OrderItemUnitDataCollectionId == 0).ToList();
            List<OrderItemUnitDataCollection> existingOrderItemUnitDataCollectionsToUpdate = orderItemUnitDataCollections.Where(orderItemUnitDataCollection => orderItemUnitDataCollection.OrderItemUnitDataCollectionId > 0).ToList();

            // Add the new items
            OrderItemUnitDataCollectionData.AddOrderItemUnitDataCollections(newOrderItemUnitDataCollectionsToAdd);

            // Then update any existing items
            existingOrderItemUnitDataCollectionsToUpdate.ForEach(orderItemUnitDataCollection =>
            {
                OrderItemUnitDataCollectionData.UpdateOrderItemUnitDataCollection(orderItemUnitDataCollection);
            });
        }

        #endregion
    }
}
