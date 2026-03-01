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
    /// Order Item Unit Consumption is a record of adding a part to the main assembly of the BOP.  It will include the serial number of the consumed part, and the part information is derived from the BOP Work Element Attribute.
    /// The part to scan in will be presented in MES to enforce proper error proofing logic.
    /// </summary>
    public class OrderItemUnitConsumption
    {
        /// <summary>
        /// Unique ID of the Order Item Unit Consumption record
        /// </summary>
        public long OrderItemUnitConsumptionId { get; set; }

        /// <summary>
        /// The main assembly unit that is consuming the part
        /// </summary>
        public OrderItemUnit OrderItemUnit { get; set; }

        /// <summary>
        /// The equipment where the consumption took place
        /// </summary>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// The user who scanned in the consumed part
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The BOP Work Element attribute that ensures the correct part is scanned
        /// </summary>
        public BillOfProcessProcessWorkElementAttribute BillOfProcessProcessWorkElementAttribute { get; set; }

        /// <summary>
        /// Serial number of the consumed part
        /// </summary>
        public string ConsumedSerialNumber { get; set; }

        /// <summary>
        /// When consumed, this will be true.  If the part has to be removed from rework, this will be set to false, but the record will still remain.
        /// This allows the MES to see history of changes, but also ensure that parts cannot be scanned twice.
        /// </summary>
        public bool IsConsumed { get; set; }

        /// <summary>
        /// Date the part was consumed
        /// </summary>
        public DateTime ConsumedDate { get; set; }

        /// <summary>
        /// Consumption date in UTC
        /// </summary>
        public DateTime ConsumedDateUtc { get; set; }

        /// <summary>
        /// If a part is removed, this proerty will be hydrated with the date it was removed
        /// </summary>
        public DateTime? RemovedDate { get; set; }

        /// <summary>
        /// Removed date in UTC
        /// </summary>
        public DateTime? RemovedDateUtc { get; set; }

        /// <summary>
        /// Order item unit consumption last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the consumption record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
