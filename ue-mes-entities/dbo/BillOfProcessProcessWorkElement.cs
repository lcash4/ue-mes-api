using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Each <see cref="Process"/> in a <see cref="BillOfProcess"/> contains 1 to many work elements.
    /// Work elements are the key steps that are needed to complete a process.  
    /// In an automated factory, these are typically processed behind the scenes.  
    /// In a manual environment, <see cref="WorkElementTypeAttribute"/> are used to guide a user on how to complete the work element.
    /// </summary>
    public class BillOfProcessProcessWorkElement
    {
        /// <summary>
        /// Unique ID of this BOP process work element
        /// </summary>
        public int BillOfProcessProcessWorkElementId { get; set; }

        /// <summary>
        /// BOP process parent record
        /// </summary>
        public BillOfProcessProcess BillOfProcessProcess { get; set; }

        /// <summary>
        /// Work element type for this record
        /// </summary>
        public WorkElementType WorkElementType { get; set; }

        /// <summary>
        /// Name is typically used to describe the process work element in brief
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sequence among the other work elements in this BOP process
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// If true, then the work element must be completed for the BOP process to be marked completed.  Otherwise, it can be skipped.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// If true, then the work element will be displayed and used.  Otherwise, it will be hidden and not used.  This is to preserve legacy data when BOP changes are made.
        /// </summary>
        public bool IsActive{ get; set; }

        /// <summary>
        /// BOP process work element record last modified by user
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
        /// List of work element attributes for this process work element
        /// </summary>
        public List<BillOfProcessProcessWorkElementAttribute>? BillOfProcessProcessWorkElementAttributes { get; set; }
    }
}
