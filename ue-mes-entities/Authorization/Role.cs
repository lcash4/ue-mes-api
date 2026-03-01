using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Authorization
{
    /// <summary>
    /// A role is used to define the work done by a feature (i.e. operator, scrap user, incoming quality check, etc.).  Users will be assigned to these roles based on their job description.
    /// The MES application will control access to features using these roles.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Unique ID for the role
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Full name of the role
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the role
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Role record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the role record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
