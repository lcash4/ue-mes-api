using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Authorization;
using ue_mes_entities.Global;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// This entity represents one to many work element executions as it captures the specific work element, the order item unit it was executed against, and then the start and end times.
    /// A single order item unit can pass through the same work element multiple times.  This could happen if the work was paused and had to be restarted, or if the element had to be redone to fix an issue.
    /// </summary>
    public class OrderItemUnitWorkElementHistory
    {
        /// <summary>
        /// Unique order item unit work element history ID
        /// </summary>
        public long OrderItemUnitWorkElementHistoryId { get; set; }

        /// <summary>
        /// Main order item unit for this history record
        /// </summary>
        public OrderItemUnit OrderItemUnit { get; set; }

        /// <summary>
        /// Unique BOP Work Element for this history record
        /// </summary>
        public BillOfProcessProcessWorkElement BillOfProcessProcessWorkElement { get; set; }

        /// <summary>
        /// Unique global equipment for this history record
        /// </summary>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// The current status for this work element
        /// </summary>
        public WorkElementStatus WorkElementStatus { get; set; }

        /// <summary>
        /// The user that worked on this order item work element.
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Start date for the history record
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// StartDate in UTC
        /// </summary>
        public DateTime StartDateUtc { get; set; }

        /// <summary>
        /// End date for the history record
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// EndDate in UTC
        /// </summary>
        public DateTime? EndDateUtc { get; set; }

        /// <summary>
        /// Order item unit work element history record last modified by user
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
    }
}
