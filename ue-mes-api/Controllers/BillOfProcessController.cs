using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global.Interface;
using ue_mes_entities.dbo;
using ue_mes_logic.dbo;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BillOfProcessController : ControllerBase
    {
        #region Constructor

        ILogger Logger { get; }
        IBillOfProcessData BillOfProcessData { get; }
        IBillOfProcessProcessData BillOfProcessProcessData { get; }
        IBillOfProcessProcessWorkElementData BillOfProcessProcessWorkElementData { get; }
        IEquipmentData EquipmentData { get; }
        IOptionsSnapshot<Settings.WorkElements> WorkElementSettings { get; }

        public BillOfProcessController(ILogger<BillOfProcessController> logger, 
            IBillOfProcessData billOfProcessData,
            IBillOfProcessProcessData billOfProcessProcessData,
            IBillOfProcessProcessWorkElementData billOfProcessProcessWorkElementData,
            IEquipmentData equipmentData,
            IOptionsSnapshot<Settings.WorkElements> workElementOptions)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            BillOfProcessData = billOfProcessData ?? throw new ArgumentNullException(nameof(billOfProcessData));
            BillOfProcessProcessData = billOfProcessProcessData ?? throw new ArgumentNullException(nameof(billOfProcessProcessData));
            BillOfProcessProcessWorkElementData = billOfProcessProcessWorkElementData ?? throw new ArgumentNullException(nameof(billOfProcessProcessWorkElementData));
            EquipmentData = equipmentData ?? throw new ArgumentNullException(nameof(equipmentData));
            WorkElementSettings = workElementOptions ?? throw new ArgumentNullException(nameof(workElementOptions));
        }

        #endregion

        #region Public API Methods

        /// <summary>
        /// Returns a full <see cref="BillOfProcess"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="partId">Unique Part ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BillOfProcess), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetByPartID/{partId}")]
        public BillOfProcess GetBillOfProcessByPartId(int partId)
        {
            using (var billOfProcessLogic = new BillOfProcessLogic(BillOfProcessData, EquipmentData))
            {
                return billOfProcessLogic.GetBillOfProcessByPartId(partId);
            }
        }

        /// <summary>
        /// Returns a full <see cref="BillOfProcess"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="billOfProcessId">Unique Bill of Process ID for the <see cref="BillOfProcess"/></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BillOfProcess), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetByID/{billOfProcessId}")]
        public BillOfProcess GetBillOfProcessByBillOfProcessId(int billOfProcessId)
        {
            using (var billOfProcessLogic = new BillOfProcessLogic(BillOfProcessData, EquipmentData))
            {
                return billOfProcessLogic.GetBillOfProcessById(billOfProcessId);
            }
        }

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcess"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessId">Unique Bill of Process Process ID for the <see cref="BillOfProcessProcess"/></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BillOfProcessProcess), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("Process/GetByID/{billOfProcessProcessId}")]
        public BillOfProcessProcess GetBillOfProcessProcessById(int billOfProcessProcessId)
        {
            using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(BillOfProcessProcessData, BillOfProcessProcessWorkElementData))
            {
                return billOfProcessProcessLogic.GetBillOfProcessProcessById(billOfProcessProcessId);
            }
        }

        /// <summary>
        /// Returns a full <see cref="BillOfProcessProcessWorkElement"/> entity with child entities included.
        /// </summary>
        /// <param name="billOfProcessProcessWorkElementId">Unique Bill of Process Process Work Element ID for the <see cref="BillOfProcessProcessWorkElement"/></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(BillOfProcessProcessWorkElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("Process/WorkElement/GetByID/{billOfProcessProcessWorkElementId}")]
        public BillOfProcessProcessWorkElement GetBillOfProcessProcessWorkElementById(int billOfProcessProcessWorkElementId)
        {
            using (var billOfProcessProcessWorkElementLogic = new BillOfProcessProcessWorkElementLogic(BillOfProcessProcessWorkElementData))
            {
                return billOfProcessProcessWorkElementLogic.GetBillOfProcessProcessWorkElementById(billOfProcessProcessWorkElementId);
            }
        }

        /// <summary>
        /// Add a new bill of process
        /// </summary>
        /// <param name="billOfProcess"></param>
        [ProducesResponseType(typeof(BillOfProcess), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("Add")]
        public IActionResult AddBillOfProcess([FromBody] BillOfProcess billOfProcess)
        {
            using (var billOfProcessLogic = new BillOfProcessLogic(BillOfProcessData, EquipmentData))
            {
                try
                {
                    billOfProcessLogic.AddBillOfProcess(billOfProcess);
                    Logger.LogInformation("New bill of process was added: {@billOfProcess}", billOfProcess);
                    return Created(billOfProcess.Name, billOfProcess);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the new bill of process.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the new bill of process.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Add a new bill of process process
        /// </summary>
        /// <param name="billOfProcessProcess"></param>
        [ProducesResponseType(typeof(BillOfProcessProcess), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("Process/Add")]
        public IActionResult AddBillOfProcessProcess(BillOfProcessProcess billOfProcessProcess)
        {
            using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(BillOfProcessProcessData, BillOfProcessProcessWorkElementData))
            {
                try
                {
                    billOfProcessProcessLogic.AddBillOfProcessProcess(billOfProcessProcess);
                    Logger.LogInformation("New BOP process was added: {@billOfProcessProcess}", billOfProcessProcess);
                    return Created(billOfProcessProcess.Process.Name, billOfProcessProcess);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the new BOP process.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the new BOP process.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Add a new bill of process process work element
        /// </summary>
        /// <param name="billOfProcessProcessWorkElement"></param>
        [ProducesResponseType(typeof(BillOfProcessProcessWorkElement), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("Process/WorkElement/Add")]
        public IActionResult AddBillOfProcessProcessWorkElement(BillOfProcessProcessWorkElement billOfProcessProcessWorkElement)
        {
            using (var billOfProcessProcessWorkElementLogic = new BillOfProcessProcessWorkElementLogic(BillOfProcessProcessWorkElementData))
            {
                try
                {
                    billOfProcessProcessWorkElementLogic.AddBillOfProcessProcessWorkElement(billOfProcessProcessWorkElement);
                    Logger.LogInformation("New BOP work element was added: {@billOfProcessProcessWorkElement}", billOfProcessProcessWorkElement);
                    return Created(billOfProcessProcessWorkElement.Name, billOfProcessProcessWorkElement);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the new BOP process work element.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the new BOP process work element.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Update an existing bill of process.  
        /// Note, the bill of process name column is NOT mutable per our own logic.  It is an auto-generated string.
        /// If the caller somehow changes it before calling this method, an exception will be thrown.
        /// </summary>
        /// <param name="billOfProcess"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("{billOfProcessId}")]
        public IActionResult UpdateBillOfProcess(BillOfProcess billOfProcess)
        {
            using (var billOfProcessLogic = new BillOfProcessLogic(BillOfProcessData, EquipmentData))
            {
                try
                {
                    billOfProcessLogic.UpdateBillOfProcess(billOfProcess);
                    Logger.LogInformation("Bill Of Process was updated: {@billOfProcess}", billOfProcess);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when updating the bill of process.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when updating the bill of process.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Update an existing bill of process process.  
        /// </summary>
        /// <param name="billOfProcessProcess"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Process/{billOfProcessProcessId}")]
        public IActionResult UpdateBillOfProcessProcess(BillOfProcessProcess billOfProcessProcess)
        {
            using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(BillOfProcessProcessData, BillOfProcessProcessWorkElementData))
            {
                try
                {
                    billOfProcessProcessLogic.UpdateBillOfProcessProcess(billOfProcessProcess);
                    Logger.LogInformation("BOP Process was updated: {@billOfProcessProcess}", billOfProcessProcess);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when updating the BOP process.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when updating the BOP process.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Replace all existing bill of process processes with a new set.
        /// </summary>
        /// <param name="billOfProcessId"></param>
        /// <param name="billOfProcessProcesses">List of new processes to save</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Process/Replace/{billOfProcessId}")]
        public IActionResult UpdateBillOfProcessProcesses(int billOfProcessId, [FromBody] List<BillOfProcessProcess> billOfProcessProcesses)
        {
            using (var billOfProcessProcessLogic = new BillOfProcessProcessLogic(BillOfProcessProcessData, BillOfProcessProcessWorkElementData))
            {
                try
                {
                    billOfProcessProcessLogic.UpdateBillOfProcessProcessList(billOfProcessId, billOfProcessProcesses, WorkElementSettings.Value.ImagesFolder);
                    Logger.LogInformation("BOP Processes were replaced for Bill of Process ID: {@billOfProcessId}", billOfProcessId);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when replacing the BOP process list.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when replacing the BOP process list.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Replace all existing bill of process process work elements with a new set.
        /// </summary>
        /// <param name="billOfProcessProcessId"></param>
        /// <param name="billOfProcessProcessWorkElements">List of new work elements to save</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("WorkElement/Replace/{billOfProcessProcessId}")]
        public IActionResult UpdateBillOfProcessProcessWorkElements(int billOfProcessProcessId, [FromBody] List<BillOfProcessProcessWorkElement> billOfProcessProcessWorkElements)
        {
            using (var billOfProcessProcessWorkElementLogic = new BillOfProcessProcessWorkElementLogic(BillOfProcessProcessWorkElementData))
            {
                try
                {
                    billOfProcessProcessWorkElementLogic.UpdateBillOfProcessProcessWorkElementList(billOfProcessProcessId, billOfProcessProcessWorkElements, WorkElementSettings.Value.ImagesFolder);
                    Logger.LogInformation("BOP Process work elements were replaced for BOP Process ID: {@billOfProcessId}", billOfProcessProcessId);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when replacing the BOP process work element list.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when replacing the BOP process work element list.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Update an existing bill of process process work element.  
        /// </summary>
        /// <param name="billOfProcessProcessWorkElement"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Process/WorkElement/{billOfProcessProcessWorkElementId}")]
        public IActionResult UpdateBillOfProcessProcessWorkElement(BillOfProcessProcessWorkElement billOfProcessProcessWorkElement)
        {
            using (var billOfProcessProcessWorkElementLogic = new BillOfProcessProcessWorkElementLogic(BillOfProcessProcessWorkElementData))
            {
                try
                {
                    billOfProcessProcessWorkElementLogic.UpdateBillOfProcessProcessWorkElement(billOfProcessProcessWorkElement);
                    Logger.LogInformation("BOP Process Work Element was updated: {@billOfProcessProcessWorkElement}", billOfProcessProcessWorkElement);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when updating the BOP process work element.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when updating the BOP process work element.  Please try again or contact MES support for help."));
                }
            }
        }

        #endregion
    }
}
