using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// The type of a container.  Some examples are "A" racks, "L" racks and Harp Racks.
    /// </summary>
    public class ContainerType
    {
        /// <summary>
        /// Unique ID of the container type
        /// </summary>
        public int ContainerTypeId { get; set; }

        /// <summary>
        /// Name of the container type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the container type
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Container type record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the container type record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
