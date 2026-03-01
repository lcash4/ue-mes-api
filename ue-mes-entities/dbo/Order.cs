using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Enums;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// An order will be received when a customer commits to a purchase.  Along with the <see cref="Customer"/> information, the order will progress through a series of <see cref="OrderStateEnum"/> until it is completed.
    /// The MES business logic will control the order state as it progresses to completion.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Unique order ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Unique order number, used for labeling and tracking
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Current state of the order
        /// </summary>
        public OrderStateEnum OrderState { get; set; }

        /// <summary>
        /// Customer receiving the order
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Amont of total finished parts to make for the order.  Note, the OrderItem entity drives the quantity, so this is just a sum of those items.
        /// </summary>
        public int TotalQuantity
        {
            get { return OrderItems != null && OrderItems.Count > 0 ? OrderItems.Sum(orderItem => orderItem.Quantity) : 0; }
        }

        /// <summary>
        /// Order record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the order
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
