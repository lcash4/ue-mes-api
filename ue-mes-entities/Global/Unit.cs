using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Global
{
    public class Unit
    {
        /// <summary>
        /// Unique ID for the unit
        /// </summary>
        public int UnitId { get; set; }

        /// <summary>
        /// Plant for the unit
        /// </summary>
        public Plant Plant { get; set; }

        /// <summary>
        /// Full name of the unit
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short display name for the unit
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Unit record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the unit record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// List of lines that are part of a <see cref="Unit"/>
        /// </summary>
        public List<Line> Lines { get; set; }
    }
}
