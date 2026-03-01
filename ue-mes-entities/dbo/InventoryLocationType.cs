using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// The location type is used to determine how this inventory can be moved.
    /// For example, parts in the receiving area (type = receiving) can only be moved into a warehouse, and not directly to production.
    /// </summary>
    public class InventoryLocationType
    {
        // Unique ID for this location type
        public int InventoryLocationTypeId { get; set; }

        /// <summary>
        /// Unique name for this location type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description this location type
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Inventory location type record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the inventory location type
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
