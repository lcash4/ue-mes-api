using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// An order item attribute is the value of an <see cref="OrderItemAttributeType"/>.  This would be the actual value captured for each item.  
    /// For example, if the <see cref="ItemAttributeType.Name"/> is "Length (mm)", then the AttributeValue should be a numeric result of that length.
    /// The data type of AttributeValue is set to string to allow it to be dynamic.  The business logic will ensure the correct values are stored.
    /// </summary>
    public class OrderItemAttribute
    {
        /// <summary>
        /// Unique ID for the order item attribute
        /// </summary>
        public int OrderItemAttributeId { get; set; }

        /// <summary>
        /// Main order item for the order item attribute
        /// </summary>
        public OrderItem OrderItem { get; set; }

        /// <summary>
        /// Order item attribute type
        /// </summary>
        public ItemAttributeType ItemAttributeType { get; set; }

        /// <summary>
        /// Value for the attribute, can be of any type.
        /// </summary>
        public string AttributeValue { get; set; }

        /// <summary>
        /// Order item attribute record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the order item attribute record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
