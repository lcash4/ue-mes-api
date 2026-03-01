using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Authorization;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Record of an inventory item that was scrapped and why
    /// </summary>
    public class InventoryItemScrap
    {
        /// <summary>
        /// Unique ID for the inventory item scrap
        /// </summary>
        public int InventoryItemScrapId { get; set; }

        /// <summary>
        /// Inventory item that is scrapped
        /// </summary>
        public InventoryItem InventoryItem { get; set; }

        /// <summary>
        /// Defect associated to this inventory scrap record
        /// </summary>
        public Defect Defect { get; set; }
        /// <summary>
        /// The logged in user when the inventory was scrapped.  If manual entry, this would be the person that entered the data.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The quantity of the inventory item that is scrap.  A lot can have more than 1, but only some of it scrap, so quantity is needed.  
        /// </summary>
        public decimal Quantity { get; set; }

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
        /// Inventory item scrap record last modified by user
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
