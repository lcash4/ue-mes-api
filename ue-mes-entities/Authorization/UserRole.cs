using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Authorization
{
    /// <summary>
    /// The UserRole is a mapping of a user and a role.  In order for some functionality to work, the role assigned to the feature must also be mapped to the user.
    /// A user can have many roles based on their job description.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Auto generated unique ID for the record
        /// </summary>
        public int UserRoleId { get; set; }

        /// <summary>
        /// User object for this mapping
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Role object for this mapping
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// User role record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the user role record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
