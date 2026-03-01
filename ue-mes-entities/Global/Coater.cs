using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Global
{
    public class Coater
    {
        /// <summary>
        /// Unique ID for the coater
        /// </summary>
        public int CoaterId { get; set; }

        /// <summary>
        /// Equipment record for the coater
        /// </summary>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// Unique coater number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Coater record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the coater record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
