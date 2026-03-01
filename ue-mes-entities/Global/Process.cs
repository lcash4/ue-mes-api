using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_entities.Global
{
    /// <summary>
    /// The physical process that is performed in the manufacturing assembly.
    /// </summary>
    public class Process
    {
        /// <summary>
        /// Unique ID of the process
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Numeric representation of the process.  While the values are numeric, the data type is string to support leading zeros and other domain specific formats.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Short name of the process such as "EDGE_GRIND" or "LASER_CUT"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The loop is a UE specific item to identify the processing loop on the factory floor.  It is also the leading digit of the Number property.
        /// </summary>
        public int? LoopNumber { get; set; }

        /// <summary>
        /// Process record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC format
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
