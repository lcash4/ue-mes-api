using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// A defect identifies a quality concern on an assembly unit or inventory item that needs to be addressed.  Users will be able to assign defects to Order Item Units and Inventory Items
    /// </summary>
    public class Defect
    {
        /// <summary>
        /// Unique defect ID
        /// </summary>
        public int DefectId { get; set; }

        /// <summary>
        /// Defect category assigned to this defect
        /// </summary>
        public DefectCategory DefectCategory { get; set; }

        /// <summary>
        /// Name of the defect
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the defect
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Defect record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the defect
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// Processes assigned to the defect
        /// </summary>
        public List<DefectProcess> DefectProcesses { get; set; }
    }
}
