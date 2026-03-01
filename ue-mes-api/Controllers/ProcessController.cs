using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;
using ue_mes_logic.Global;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProcessController : ControllerBase
    {
        #region Constructor

        ILogger Logger { get; }
        IProcessData ProcessData { get; }

        public ProcessController(ILogger<ProcessController> logger, IProcessData processData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ProcessData = processData ?? throw new ArgumentNullException(nameof(processData));
        }

        #endregion

        #region Public API Methods

        /// <summary>
        /// Return all production processes for the factory
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Process>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("All")]
        public List<Process> GetProcesses()
        {
            using (var processLogic = new ProcessLogic(ProcessData))
            {
                return processLogic.GetProcesses();
            }
        }

        /// <summary>
        /// Return all production processes that are not already included in the provided bill of process
        /// </summary>
        /// <param name="localSiteName">The local site to filter to.  Not all processes exist in every site.</param>
        /// <param name="billOfProcessId">Bill of Process ID to filter</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Process>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("NotInBop/{localSiteName}/{billOfProcessId}")]
        public List<Process> GetProcessesNotInBillOfProcess(string localSiteName, int billOfProcessId)
        {
            using (var processLogic = new ProcessLogic(ProcessData))
            {
                return processLogic.GetProcessesNotInBillOfProcessForLocalSite(localSiteName,billOfProcessId);
            }
        }

        #endregion
    }
}
