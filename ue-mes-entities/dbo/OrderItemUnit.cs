using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// An Order Item Unit is where the unique serial number for an <see cref="OrderItem"/> is captured.  
    /// The logic could be different for when a serial number is assigned based on the type of <see cref="Part"/>, but the format will be the same for all.
    ///   14 total characters
    ///   Julian Date (5) + Type Id (2) + Coater Number (2) + Sequence Number (5)
    ///    - The Type Id is a 2 digit representation of the Order Item Type, (coming soon).
    ///    - The coater number comes from the global coater record as it is the unique process identified at Ubiquitous energy.  
    ///    - The sequence number will be a running count and will reset based on the Julian Date + OrderItemType.  
    /// </summary>
    public class OrderItemUnit
    {
        /// <summary>
        /// Unique order item unit ID
        /// </summary>
        public int OrderItemUnitId { get; set; }

        /// <summary>
        /// Main order item for the order item unit
        /// </summary>
        public OrderItem OrderItem { get; set; }

        /// <summary>
        /// Part being assembled for this order item unit.  With multi-tier BOM structure, an Order Item Unit can be the finished part, or one of its consumed assemblies.
        /// </summary>
        public Part Part { get; set; }

        /// <summary>
        /// Unique serial number of the order item unit
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Order item unit record last modified by user
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
        /// List of work element history for this order item unit
        /// </summary>
        public List<OrderItemUnitWorkElementHistory>? OrderItemUnitWorkElementHistories { get; set; }

        /// <summary>
        /// List of data collection values for this order item unit
        /// </summary>
        public List<OrderItemUnitDataCollection>? OrderItemUnitDataCollections { get; set; }

        /// <summary>
        /// List of consumption values for this order item unit
        /// </summary>
        public List<OrderItemUnitConsumption>? OrderItemUnitConsumptions { get; set; }

        /// <summary>
        /// If the order item unit is scrapped, this property will be hydrated with the information.  It will be null otherwise.
        /// </summary>
        public OrderItemUnitScrap OrderItemUnitScrap { get; set; }
    }
}
