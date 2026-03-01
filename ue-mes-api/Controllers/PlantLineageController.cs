
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using ue_mes_data.Global;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;
using ue_mes_logic.Global;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PlantLineageController : ControllerBase
    {
        #region Constructor

        ILogger Logger { get; }
        ISiteData SiteData { get; }
        IEquipmentData EquipmentData { get; }

        public PlantLineageController(ILogger<PlantLineageController> logger, ISiteData siteData, IEquipmentData equipmentData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SiteData = siteData;
            EquipmentData = equipmentData;
        }

        #endregion

        #region Public API Methods

        /// <summary>
        /// Get a site by its display name
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Site), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("site/GetByDisplayName/{displayName}")]
        public Site GetSiteByDisplayName(string displayName)
        {
            using (var siteLogic = new SiteLogic(SiteData))
            {
                var siteObject = siteLogic.GetSiteByDisplayName(displayName);
                return siteObject;
            }
        }

        /// <summary>
        /// Returns a list of equipment for a given site.
        /// </summary>
        /// <param name="siteDisplayName">Unique site display name</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Equipment>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("equipment/GetBySiteDisplayName/{siteDisplayName}")]
        public List<Equipment> GetEquipmentBySiteDisplayName(string siteDisplayName)
        {
            using (var equipmentLogic = new EquipmentLogic(EquipmentData))
            {
                return equipmentLogic.GetEquipmentBySiteDisplayName(siteDisplayName);
            }
        }

        #endregion
    }
}