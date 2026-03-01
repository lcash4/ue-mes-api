using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using ue_mes_entities.dbo;
using ue_mes_logic.dbo;
using ue_mes_data.dbo.Interface;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UnitsOfMeasureController : ControllerBase
    {
        ILogger Logger { get; }
        IUnitOfMeasureData UnitOfMeasureData { get; }

        public UnitsOfMeasureController(ILogger<UnitsOfMeasureController> logger, IUnitOfMeasureData unitOfMeasureData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UnitOfMeasureData = unitOfMeasureData ?? throw new ArgumentNullException(nameof(unitOfMeasureData));
        }

        #region Public Controller Methods

        /// <summary>
        /// Simply return all units of measure for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<UnitOfMeasure>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public List<UnitOfMeasure> GetUnitsOfMeasure()
        {
            using (var unitOfMeasureLogic = new UnitOfMeasureLogic(UnitOfMeasureData))
            {
                return unitOfMeasureLogic.GetUnitsOfMeasure();
            }
        }

        #endregion
    }
}
