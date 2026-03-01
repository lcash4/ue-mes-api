using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using ue_mes_logic.dbo;
using ue_mes_entities.Enums;
using System.Linq;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PartTypesController : ControllerBase
    {
        ILogger Logger { get; }
        public PartTypesController(ILogger<PartTypesController> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Public Controller Methods

        /// <summary>
        /// Simply return all part types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(KeyValuePair<int, string>[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public KeyValuePair<int,string>[] GetPartTypes()
        {
            return Enum.GetValues(typeof(PartTypeEnum))
                .Cast<int>()
                .ToDictionary(enumValue => enumValue, enumValue => Enum.GetName(typeof(PartTypeEnum), enumValue)).ToArray();
        }

        #endregion
    }
}
