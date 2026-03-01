using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IDefectData
    {
        /// <summary>
        /// Return all defects for a given category.  Only return inactive defects if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="defectCategoryId"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<Defect> GetDefectsByCategoryId(int defectCategoryId, bool includeInactive = false);
    }
}
