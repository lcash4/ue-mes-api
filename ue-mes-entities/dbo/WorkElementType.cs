using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// A work element type is a fixed set of work element instructions that are useful in manufacturing.  
    /// A couple examples would be "Text", which is just a text based instruction on how to complete the work.  Another would be "Image" where instructions are displayed along with a helpful image.
    /// To simplify the front end development, the Name will be directly related to a component.  Any additions to the <see cref="WorkElementType"/> table will be useless without also developing the front end.
    /// </summary>
    public class WorkElementType
    {
        /// <summary>
        /// Unique ID of the work element type
        /// </summary>
        public int WorkElementTypeId { get; set; }

        /// <summary>
        /// Name of the work element type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the work elemenet type
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Work element type record last modified by user
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
        /// List of work element type attributes associated with this work element
        /// </summary>
        public List<WorkElementTypeAttribute>? WorkElementTypeAttributes { get; set; }
    }
}
