using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    public class BillOfMaterialPart
    {
        /// <summary>
        /// Unique ID of the BOM Part
        /// </summary>
        public int BillOfMaterialPartId { get; set; }

        /// <summary>
        /// Bill of Material parent record
        /// </summary>
        public BillOfMaterial BillOfMaterial { get; set; }

        /// <summary>
        /// Part object for this record
        /// </summary>
        public Part Part { get; set; }

        /// <summary>
        /// Total part quantity.  Decimal precision needed for parts measured in length or other non-integer value.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Bill of material part record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC format
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
