using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// The WorkElementTypeAttributeListItem class is used for work element attributes that have a set list of options.  These are typically used for Data Collection type attributes.
    /// </summary>
    public class WorkElementTypeAttributeListItem
    {
        // Unique ID for this work element type attribute list item
        public int WorkElementTypeAttributeListItemId { get; set; }

        /// <summary>
        /// The work element type attribute that this list item belongs to.
        /// </summary>
        public WorkElementTypeAttribute WorkElementTypeAttribute { get; set; }

        /// <summary>
        /// Unique name for this work element type attribute list item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description for this work element type attribute list item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Work element type attribute list item record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Last modified time of the work element type attribute list item
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// Last modified time UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
