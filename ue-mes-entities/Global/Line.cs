using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Global
{
    public class Line
    {
        /// <summary>
        /// Unique ID for the line
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// Unit for the line
        /// </summary>
        public Unit Unit { get; set; }

        /// <summary>
        /// Full name of the line
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Short display name for the line
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Line record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the line record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// List of equipment that are part of a <see cref="Line"/>
        /// </summary>
        public List<Equipment> Equipment { get; set; }
    }
}
