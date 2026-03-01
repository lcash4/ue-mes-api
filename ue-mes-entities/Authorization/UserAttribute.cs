using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Authorization
{
    /// <summary>
    /// User attributes are additional attributes about a user that may or may not be present.  For example, a phone number would be needed for someone on call, but not needed otherwise. 
    /// Each attribute is its own key\value pair where the <see cref="UserAttributeType"/> is the key.
    /// </summary>
    public class UserAttribute
    {
        /// <summary>
        /// Unique ID for the user attribute
        /// </summary>
        public int UserAttributeId { get; set; }

        /// <summary>
        /// User object for this attribute
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The attribute type (i.e. phone number, badge ID, etc.)
        /// </summary>
        public UserAttributeType UserAttributeType { get; set; }

        /// <summary>
        /// The value of the attribute
        /// </summary>
        public string UserAttributeValue { get; set; }

        /// <summary>
        /// UserAttribute record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the user attribute record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
