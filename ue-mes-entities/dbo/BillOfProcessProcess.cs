
using ue_mes_entities.Global;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// One of many processes included in a <see cref="BillOfProcess"/>.
    /// The process links to a global <see cref="Process"/> record and also includes its sequence among the other processes
    /// </summary>
    public class BillOfProcessProcess
    {
        /// <summary>
        /// Unique ID of the BOP Process 
        /// </summary>
        public int BillOfProcessProcessId { get; set; }

        /// <summary>
        /// Bill of Process parent record
        /// </summary>
        public BillOfProcess BillOfProcess { get; set; }

        /// <summary>
        /// Global Process step
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        /// Sequence for which the process should be run among this BOP
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// A BOP process will be set to IsActive = false if we need to retain work element history when the process is removed from a BOP.  Of coure it will be marked true if it is actively part of the BOP process list.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// BOP process record last modified by user
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
        /// All work elements associated to this process
        /// </summary>
        public List<BillOfProcessProcessWorkElement>? BillOfProcessProcessWorkElements { get; set; }
    }
}
