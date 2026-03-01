using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Authorization
{
    /// <summary>
    /// User Attribute Types describe the additional attributes that can be added to a user, such as phone number of their badge ID.
    /// </summary>
    public class UserAttributeType
    {
        // Unique ID for this user attribute type
        public int UserAttributeTypeId { get; set; }

        /// <summary>
        /// Unique name for this user attribute type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description this user attribute type
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User attribute type record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the user attribute type
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
