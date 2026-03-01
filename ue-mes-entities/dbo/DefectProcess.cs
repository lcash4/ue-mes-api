using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Global;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Defect Process is a mapping of Defects to Global Processes.  This is needed to ensure only valid defects are available at each entry point.
    /// For example, you cannot log a defect that is specific to a process that hasn't even completed (coating deposition for example).
    /// </summary>
    public class DefectProcess
    {
        /// <summary>
        /// Unique defect process ID
        /// </summary>
        public int DefectProcessId { get; set; }

        /// <summary>
        /// Defect assigned to this defect process mapping
        /// </summary>
        public Defect Defect{ get; set; }

        /// <summary>
        /// Process assigned to this defect process mapping
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        /// Defect process record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the defect process
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
