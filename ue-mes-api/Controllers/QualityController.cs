using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_logic.dbo;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class QualityController : ControllerBase
    {
        #region Constructor

        ILogger Logger { get; }
        IDefectCategoryData DefectCategoryData { get; }
        IDefectData DefectData { get; }

        public QualityController(ILogger<QualityController> logger, IDefectCategoryData defectCategoryData, IDefectData defectData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            DefectCategoryData = defectCategoryData ?? throw new ArgumentNullException(nameof(defectCategoryData));
            DefectData = defectData ?? throw new ArgumentNullException(nameof(defectData));
        }

        #endregion

        #region Public API Methods

        #region Defects

        /// <summary>
        /// Return all defects for a given category.  Only return inactive defects if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="defectCategoryId"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Defect>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("Defect/GetByCategoryId/{defectCategoryId}/{includeInactive}")]
        public List<Defect> GetDefectsByCategoryId(int defectCategoryId, bool includeInactive = false)
        {
            using (var defectLogic = new DefectLogic(DefectData))
            {
                return defectLogic.GetDefectsByCategoryId(defectCategoryId, includeInactive);
            }
        }

        /// <summary>
        /// Return all defect categories, as well as their child defects and defect processes.  
        /// The amount of unique defects should not exceed a large amount, so returning the fully hydrated list will allow the caller to have full flexibility.
        /// Only include inactive defects when <paramref name="includeInactive"/> is true.
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<DefectCategory>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("DefectCategory/GetAll/{includeInactive}")]
        public List<DefectCategory> GetDefectCategoriesWithDefects(bool includeInactive = false)
        {
            using (var defectCategoryLogic = new DefectCategoryLogic(DefectCategoryData))
            {
                return defectCategoryLogic.GetDefectCategoriesWithDefects(includeInactive);
            }
        }

        #endregion

        #endregion
    }
}
