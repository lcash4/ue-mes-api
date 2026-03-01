using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IDefectCategoryData
    {
        /// <summary>
        /// Return all defect categories, as well as their child defects and defect processes.  
        /// The amount of unique defects should not exceed a large amount, so returning the fully hydrated list will allow the caller to have full flexibility.
        /// Only include inactive defects when <paramref name="includeInactive"/> is true.
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<DefectCategory> GetDefectCategoriesWithDefects(bool includeInactive = false);
    }
}
