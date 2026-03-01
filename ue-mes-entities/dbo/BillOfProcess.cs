using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// A Bill of Process is a list of all processes needed to build the associated part.  Each part can have a unique set of processes, although in a manufacturing plant, they will change slowly
    /// </summary>
    public class BillOfProcess
    {
        /// <summary>
        /// Unique BOP Id
        /// </summary>
        public int BillOfProcessId { get; set; }

        /// <summary>
        /// BOP Name (includes a revision character at the end)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the BOP, typically what the part is and calling out any unique processing
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The part to be assembled through this BOP
        /// </summary>
        public Part Part { get; set; }

        /// <summary>
        /// Effective start date of the BOP
        /// </summary>
        public DateTime EffectiveStartDate { get; set; }

        /// <summary>
        /// Effective start date in UTC format
        /// </summary>
        public DateTime EffectiveStartDateUtc { get; set; }

        /// <summary>
        /// Effective end date of the BOP
        /// </summary>
        public DateTime? EffectiveEndDate { get; set; }

        /// <summary>
        /// Effective end date in UTC format
        /// </summary>
        public DateTime? EffectiveEndDateUtc { get; set; }

        /// <summary>
        /// Bill of process record last modified by user
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

        /// <summary>
        /// List of processes included in this BOP
        /// </summary>
        public List<BillOfProcessProcess>? BillOfProcessProcesses { get; set; }
    }
}
