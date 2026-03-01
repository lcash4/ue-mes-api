using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Authorization
{
    /// <summary>
    /// A user of the system, including standard attributes that apply to every one (name, email, login)
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique ID for the user (auto generated, not the user's login)
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Login name of the user
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// First name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email Address of the user
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// User's job title
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Whether the user is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// User record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the user record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// List of user attributes assigned to this user
        /// </summary>
        public List<UserAttribute> UserAttributes { get; set; }

        /// <summary>
        /// List of roles assigned to this user
        /// </summary>
        public List<UserRole> UserRoles { get; set; }
    }
}
