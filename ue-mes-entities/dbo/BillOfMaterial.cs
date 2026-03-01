using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// The Bill of Material (BOM) is a full list of parts that are consumed in a finished part.  For example, an IGU would contain two lites of glass, a spacer and some external wiring.
    /// </summary>
    public class BillOfMaterial
    {
        /// <summary>
        /// Unique BOM Id
        /// </summary>
        public int BillOfMaterialId { get; set; }

        /// <summary>
        /// BOM Name (includes a revision character at the end)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the BOM, typically what the part is and calling out any unique processing
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The finished part to be assembled through this BOM
        /// </summary>
        public Part Part { get; set; }

        /// <summary>
        /// Effective start date of the BOM
        /// </summary>
        public DateTime EffectiveStartDate { get; set; }

        /// <summary>
        /// Effective start date in UTC format
        /// </summary>
        public DateTime EffectiveStartDateUtc { get; set; }

        /// <summary>
        /// Effective end date of the BOM
        /// </summary>
        public DateTime? EffectiveEndDate { get; set; }

        /// <summary>
        /// Effective end date in UTC format
        /// </summary>
        public DateTime? EffectiveEndDateUtc { get; set; }

        /// <summary>
        /// Bill of material record last modified by user
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
        /// List of parts included in this BOM
        /// </summary>
        public List<BillOfMaterialPart>? BillOfMaterialParts { get; set; }
    }
}
