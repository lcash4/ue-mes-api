using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Global
{
    public class Plant
    {
        /// <summary>
        /// Unique ID for the plant
        /// </summary>
        public int PlantId { get; set; }

        /// <summary>
        /// Site for the plant
        /// </summary>
        public Site Site { get; set; }

        /// <summary>
        /// Full name of the plant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short display name for the plant
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Plant record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the plant record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// List of units that are part of a <see cref="Plant"/>
        /// </summary>
        public List<Unit> Units { get; set; }
    }
}
