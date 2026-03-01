using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Global
{
    public class Equipment
    {
        /// <summary>
        /// Unique ID for the equipment
        /// </summary>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Line for the equipment
        /// </summary>
        public Line Line { get; set; }

        /// <summary>
        /// Process for the equipment
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        /// Name for the equipment (combination of site, plant, unit, line and process)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Equipment record last modified by user
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// LastModifiedTime of the equipment record
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// LastModifiedTime in UTC
        /// </summary>
        public DateTime LastModifiedTimeUtc { get; set; }
    }
}
