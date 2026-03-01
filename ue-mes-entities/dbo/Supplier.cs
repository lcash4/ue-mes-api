using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Supplier information that is tied to an <see cref="InventoryItem"/>.  This information will typically be fed through a CRM or ERP platform, but the MES UI will also support entry.
    /// </summary>
    public class Supplier
    {
        /// <summary>
        /// Unique supplier ID
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// Supplier name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Supplier mailing address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Supplier mailing postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Supplier mailing city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Supplier mailing state or province
        /// </summary>
        public string StateOrProvince { get; set; }

        /// <summary>
        /// Supplier mailing country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Supplier main phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Supplier main email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Supplier main contact
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Supplier is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Supplier record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Supplier record last modified time
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
