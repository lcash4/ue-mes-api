using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Customer information that is tied to an <see cref="Order"/>.  This information will typically be fed through a CRM or ERP platform, but the MES UI will also support entry.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Unique customer ID
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Customer mailing address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Customer mailing postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Customer mailing city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Customer mailing state or province
        /// </summary>
        public string StateOrProvince { get; set; }

        /// <summary>
        /// Customer mailing country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Customer main phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Customer main email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Customer main contact
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Customer is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Customer record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Customer record last modified time
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
