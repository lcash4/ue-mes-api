using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// A BOP Process Work Element Attribute contains the details that are presented to a user when completing a <see cref="BillOfProcessProcessWorkElement"/>
    /// To allow customization of each work element, the attributes are dynamic and ultimately result in key value pairs where the key is a <see cref="WorkElementTypeAttribute"/> and the value is the AttributeValue of this object.
    /// </summary>
    public class BillOfProcessProcessWorkElementAttribute
    {
        /// <summary>
        /// Unique ID of the BOP Process work element attribute
        /// </summary>
        public int BillOfProcessProcessWorkElementAttributeId { get; set; }

        /// <summary>
        /// Parent BOP process work element
        /// </summary>
        public BillOfProcessProcessWorkElement BillOfProcessProcessWorkElement { get; set; }

        /// <summary>
        /// Work element attribute type assigned to this attribute
        /// </summary>
        public WorkElementTypeAttribute WorkElementTypeAttribute { get; set; }

        /// <summary>
        /// Attribute value is dynamic.  It can be an image path, instruction text, parameter name, etc.
        /// </summary>
        public string AttributeValue { get; set; }

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
    }
}
