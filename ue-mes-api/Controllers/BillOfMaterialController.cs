using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_logic.dbo;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BillOfMaterialController : ControllerBase
    {
        #region Constructor

        ILogger Logger { get; }
        IBillOfMaterialData BillOfMaterialData { get; }
        IBillOfMaterialPartData BillOfMaterialPartData { get; }

        public BillOfMaterialController(ILogger<BillOfMaterialController> logger, 
            IBillOfMaterialData billOfMaterialData,
            IBillOfMaterialPartData billOfMaterialPartData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            BillOfMaterialData = billOfMaterialData ?? throw new ArgumentNullException(nameof(billOfMaterialData));
            BillOfMaterialPartData = billOfMaterialPartData ?? throw new ArgumentNullException(nameof(billOfMaterialPartData));
        }

        #endregion

        #region Public API Methods

        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="partId">Unique Part ID that is built from this <see cref="BillOfMaterial"/></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BillOfMaterial), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetByPartID/{partId}")]
        public BillOfMaterial GetBillOfMaterialByPartId(int partId)
        {
            using (var billOfMaterialLogic = new BillOfMaterialLogic(BillOfMaterialData))
            {
                return billOfMaterialLogic.GetBillOfMaterialByPartId(partId);
            }
        }

        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="billOfMaterialId">Unique Bill of Material ID for the <see cref="BillOfMaterial"/></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BillOfMaterial), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetByID/{billOfMaterialId}")]
        public BillOfMaterial GetBillOfProcessByBillOfProcessId(int billOfMaterialId)
        {
            using (var billOfMaterialLogic = new BillOfMaterialLogic(BillOfMaterialData))
            {
                return billOfMaterialLogic.GetBillOfMaterialById(billOfMaterialId);
            }
        }

        /// <summary>
        /// Add a new bill of material
        /// </summary>
        /// <param name="billOfMaterial"></param>
        [ProducesResponseType(typeof(BillOfMaterial), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("Add")]
        public IActionResult AddBillOfMaterial([FromBody] BillOfMaterial billOfMaterial)
        {
            using (var billOfMaterialLogic = new BillOfMaterialLogic(BillOfMaterialData))
            {
                try
                {
                    billOfMaterialLogic.AddBillOfMaterial(billOfMaterial);
                    Logger.LogInformation("New bill of material was added: {@billOfMaterial}", billOfMaterial);
                    return Created(billOfMaterial.Name, billOfMaterial);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the new bill of material.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the new bill of material.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Update an existing bill of material.  
        /// Note, the bill of material name column is NOT mutable per our own logic.  It is an auto-generated string.
        /// If the caller somehow changes it before calling this method, an exception will be thrown.
        /// </summary>
        /// <param name="billOfMaterial"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("{billOfMaterialId}")]
        public IActionResult UpdateBillOfMaterial(BillOfMaterial billOfMaterial)
        {
            using (var billOfMaterialLogic = new BillOfMaterialLogic(BillOfMaterialData))
            {
                try
                {
                    billOfMaterialLogic.UpdateBillOfMaterial(billOfMaterial);
                    Logger.LogInformation("Bill Of Material was updated: {@billOfMaterial}", billOfMaterial);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when updating the bill of material.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when updating the bill of material.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Replace all existing bill of material parts with a new set.
        /// </summary>
        /// <param name="billOfMaterialId"></param>
        /// <param name="billOfMaterialParts">List of new parts to save</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Parts/Replace/{billOfMaterialId}")]
        public IActionResult UpdateBillOfMaterialPartsList(int billOfMaterialId, [FromBody] List<BillOfMaterialPart> billOfMaterialParts)
        {
            using (var billOfMaterialPartLogic = new BillOfMaterialPartLogic(BillOfMaterialPartData))
            {
                try
                {
                    billOfMaterialPartLogic.UpdateBillOfMaterialPartsList(billOfMaterialId, billOfMaterialParts);
                    Logger.LogInformation("BOM Parts were replaced for Bill of Material ID: {@billOfMaterialId}", billOfMaterialId);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when updating the bill of material parts list.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when updating the bill of material parts list.  Please try again or contact MES support for help."));
                }
            }
        }

        #endregion
    }
}
