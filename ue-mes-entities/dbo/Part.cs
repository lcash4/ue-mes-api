using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Authorization;
using ue_mes_entities.Enums;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// A part can be one of 3 types (raw material, sub assembly, finished good).  
    /// This object will be used to track all types throughout the production process and is used in many other objects, such as <see cref="Order"/>, <see cref="BillOfProcess"/> and BOM
    /// </summary>
    public class Part
    {
        /// <summary>
        /// Unique ID for the part
        /// </summary>
        public int PartId { get; set; }

        /// <summary>
        /// Type of part
        /// </summary>
        public PartTypeEnum PartType { get; set; }

        /// <summary>
        /// Alphanumeric unique part number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Revision of the part
        /// </summary>
        public string PartRevision { get; set; }

        /// <summary>
        /// A description of the part
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unit of measure for the part
        /// </summary>
        public UnitOfMeasure UnitOfMeasure { get; set; }

        /// <summary>
        /// Whether the part is active for use
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Part record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the part record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }

        /// <summary>
        /// List of item attribute types assigned to this part
        /// </summary>
        public List<PartItemAttributeType> PartItemAttributeTypes { get; set; }
    }
}
