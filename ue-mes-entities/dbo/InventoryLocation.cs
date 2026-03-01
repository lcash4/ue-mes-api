using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Each inventory location should represent a physical location at the factory.  The type of location will be defined by the <see cref="InventoryLocationType"/> property
    /// For example, you can have 1 to many warehouse inventory locations that would all have the same type of "warehouse"
    /// </summary>
    public class InventoryLocation
    {
        // Unique ID for this location
        public int InventoryLocationId { get; set; }

        /// <summary>
        /// Inventory location type 
        /// </summary>
        public InventoryLocationType InventoryLocationType { get; set; }

        /// <summary>
        /// Unique name for this location
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Inventory location record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the inventory location
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
