using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Enums;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// An order item is an individual part and quantity that is added to a full <see cref="Order"/>.  Often times, one <see cref="Order"/> will contain many line items.  
    /// This class represents the individual line item as a <see cref="Part"/>, as well as the quantity needed to fulfill the order.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Unique order item ID
        /// </summary>
        public int OrderItemId { get; set; }

        /// <summary>
        /// Main order unique ID for the order item
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Finished part object of the order item
        /// </summary>
        public Part FinishedPart { get; set; }

        /// <summary>
        /// Amont of finished parts to make for the order item
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Order item record last modified by user
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

        /// <summary>
        /// List of order item attributes
        /// </summary>
        public List<OrderItemAttribute> OrderItemAttributes { get; set; }
    }
}
