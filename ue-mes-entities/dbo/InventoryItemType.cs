using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Inventory is stored in either lots or individual serial numbers.  This type object defines which one applies.
    /// </summary>
    public class InventoryItemType
    {
        // Unique ID for this item type
        public int InventoryItemTypeId { get; set; }

        /// <summary>
        /// Unique name for this item type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description this item type
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Inventory item type record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the inventory item type
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
