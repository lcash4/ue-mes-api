using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.dbo
{
    /// <summary>
    /// Unit of measures are needed as manufacturing environments have many part types.  These units of measure can be associated to <see cref="Part"/> in the MES.
    /// </summary>
    public class UnitOfMeasure
    {
        /// <summary>
        /// Unique ID for the unit of measure
        /// </summary>
        public byte UnitOfMeasureId { get; set; }

        /// <summary>
        /// Full name of the unit of measure
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Short display name for the unit of measure
        /// </summary>
        public string? ShortName { get; set; }

        /// <summary>
        /// Unit of measure record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the unit of measure record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
