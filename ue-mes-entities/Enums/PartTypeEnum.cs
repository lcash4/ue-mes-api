using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ue_mes_entities.Enums
{
    public enum PartTypeEnum
    {
        /// <summary>
        /// Default if unknown
        /// </summary>
        Unknown = 0,
        
        /// <summary>
        /// Any raw material or part that has not been processed
        /// </summary>
        RawMaterial = 1,
        
        /// <summary>
        /// Combination of one or more parts with processing
        /// </summary>
        SubAssembly = 2,
        
        /// <summary>
        /// Finished part at the end of line
        /// </summary>
        FinishedGood = 3
    }
}
