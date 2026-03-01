using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Used to map\unmap an order item unit to a container when the physical item has been added\removed.
    /// </summary>
    public class ContainerOrderItemUnit
    {
        /// <summary>
        /// Unique ID of the container order item unit record
        /// </summary>
        public long ContainerOrderItemUnitId { get; set; }

        /// <summary>
        /// Container that the order item was added to
        /// </summary>
        public Container Container { get; set; }

        /// <summary>
        /// Order item unit tha was added to the container
        /// </summary>
        public OrderItemUnit OrderItemUnit { get; set; }

        /// <summary>
        /// The position is of string type to support non-standard packing.  For example, an A rack has two sides and could be stored as L1,L2,R1,R2 etc.
        /// </summary>
        public string PositionOnContainer { get; set; }

        /// <summary>
        /// Container order item unit record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the container order item unit record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
