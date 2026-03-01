using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class DefectLogic : LogicBase
    {
        IDefectData DefectData { get; }

        #region Constructor

        public DefectLogic(IDefectData defectData)
        {
            DefectData = defectData ?? throw new ArgumentNullException(nameof(defectData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return all defects for a given category.  Only return inactive defects if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="defectCategoryId"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public List<Defect> GetDefectsByCategoryId(int defectCategoryId, bool includeInactive = false)
        {
            return DefectData.GetDefectsByCategoryId(defectCategoryId, includeInactive);
        }

        #endregion
    }
}
