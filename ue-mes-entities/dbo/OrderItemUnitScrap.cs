using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Authorization;
using ue_mes_entities.Global;

namespace ue_mes_entities.dbo
{
    public class OrderItemUnitScrap
    {
        /// <summary>
        /// Unique ID of the order item unit scrap record
        /// </summary>
        public long OrderItemUnitScrapId { get; set; }

        /// <summary>
        /// Order Item Unit for this record
        /// </summary>
        public OrderItemUnit OrderItemUnit { get; set; }

        /// <summary>
        /// Equipment where the unit was scrapped
        /// </summary>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// Defect associated to this unit scrap record
        /// </summary>
        public Defect Defect { get; set; }
        /// <summary>
        /// The logged in user when the unit was scrapped.  If manual entry, this would be the person that entered the data.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// A comment that provides more details on the scrap record
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Date the record was scrapped
        /// </summary>
        public DateTime ScrapDate { get; set; }

        /// <summary>
        /// Date scrapped in UTC
        /// </summary>
        public DateTime ScrapDateUtc { get; set; }

        /// <summary>
        /// Order item unit scrap record last modified by user
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
