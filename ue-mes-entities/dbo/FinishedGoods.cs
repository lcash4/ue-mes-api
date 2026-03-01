using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Finished goods are containers of order item units that have been validated and are ready to ship.  It will also contain records for containers that are returned to production (which also requires validation)
    /// </summary>
    public class FinishedGoods
    {
        /// <summary>
        /// Unique ID of the finished goods record
        /// </summary>
        public long FinishedGoodsId { get; set; }

        /// <summary>
        /// Container of the finished goods.
        /// </summary>
        public Container Container { get; set; }

        /// <summary>
        /// The quantity at transaction date is specified because if a container moves in and out of finished goods, we need to know the quantity at that time.  This allows for proper accounting.
        /// </summary>
        public int ContainerQuantityAtTransactionDate { get; set; }

        /// <summary>
        /// Set to true if a container is returned back to production
        /// </summary>
        public bool IsReturnedToProduction { get; set; }

        /// <summary>
        /// Date of the transaction into finished goods.
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Date of the transaction into finished goods in UTC
        /// </summary>
        public DateTime TransactionDateUtc { get; set; }

        /// <summary>
        /// If a container is returned to production, this date will be set to that time.
        /// </summary>
        public DateTime? ReturnToProductionDate { get; set; }

        /// <summary>
        /// Return to production date in UTC
        /// </summary>
        public DateTime? ReturnToProductionDateUtc { get; set; }

        /// <summary>
        /// Finished goods record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the finished goods record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
