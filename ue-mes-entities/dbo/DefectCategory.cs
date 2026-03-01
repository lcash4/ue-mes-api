using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Defect categories are used to group defects for easier selection and analysis
    /// </summary>
    public class DefectCategory
    {
        /// <summary>
        /// Unique ID of the defect category
        /// </summary>
        public int DefectCategoryId { get; set; }

        /// <summary>
        /// Name of the defect category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the defect category
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Defect category record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the defect category record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// Defects assigned to the defect category
        /// </summary>
        public List<Defect> Defects { get; set; }
    }
}
