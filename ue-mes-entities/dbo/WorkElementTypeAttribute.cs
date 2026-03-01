using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Each <see cref="WorkElementType"/> can have 1 to many attributes.  These are used to help build the front end and are coupled with the UI.  Any additions to this table are typically paired with new development.
    /// A couple examples would be "Work Instruction Text" which means the <see cref="BillOfProcessProcessWorkElementAttribute.AttributeValue"/> would contain the instruction text that the user sees on the UI
    /// Another example would be "Work Element Image" which means the <see cref="BillOfProcessProcessWorkElementAttribute.AttributeValue"/> would contain the file name that was uploaded for displaying an image to the user.
    /// The schema allows for additional attributes to be added, but they are still coupled with the UI and may require additional front end development
    /// </summary>
    public class WorkElementTypeAttribute
    {
        /// <summary>
        /// Unique ID for the work element type attribute
        /// </summary>
        public int WorkElementTypeAttributeId { get; set; }

        /// <summary>
        /// Work element type parent record
        /// </summary>
        public WorkElementType WorkElementType { get; set; }

        /// <summary>
        /// Name of the work element type attribute.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If IsRequiredAtSetup is true, then this attribute must be added to the list of work element attributes.  In the UI, this will be done by default.  The optional (IsRequiredAtSetup = false) can be added by the user
        /// </summary>
        public bool IsRequiredAtSetup { get; set; }

        /// <summary>
        /// If IsRequiredAtRun is true, then this attribute must get a value when assembling a part.  The optional (IsRequiredAtRun = false) attributes will present a warning, but can be bypassed
        /// </summary>
        public bool IsRequiredAtRun { get; set; }

        /// <summary>
        /// Work element type attribute record last modified by user
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

        public List<WorkElementTypeAttributeListItem> WorkElementTypeAttributeListItems { get; set; }
    }
}
