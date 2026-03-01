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
    /// These are data collection key\value pairs that are specifically associated to the assembly work element.  They are gathered through MES and not a data collection pipeline.
    /// </summary>
    public class OrderItemUnitDataCollection
    {
        /// <summary>
        /// Unique ID of the order item data collection record
        /// </summary>
        public long OrderItemUnitDataCollectionId { get; set; }

        /// <summary>
        /// Order Item Unit for this record
        /// </summary>
        public OrderItemUnit OrderItemUnit { get; set; }

        /// <summary>
        /// Equipment where the data was collected
        /// </summary>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// The logged in user when the data was collected.  If manual entry, this would be the person that entered the data.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The work element attribute for this data collection record
        /// </summary>
        public BillOfProcessProcessWorkElementAttribute BillOfProcessProcessWorkElementAttribute { get; set; }

        /// <summary>
        /// The value of the data collection parameter.  Type is string to support all types of data.
        /// </summary>
        public string CollectedValue { get; set; }

        /// <summary>
        /// Date the record was saved
        /// </summary>
        public DateTime CollectedDate { get; set; }

        /// <summary>
        /// Date saved in UTC
        /// </summary>
        public DateTime CollectedDateUtc { get; set; }

        /// <summary>
        /// Order item unit data collection record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
