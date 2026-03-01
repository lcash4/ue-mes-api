using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sprache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_logic.dbo;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class WorkElementController : ControllerBase
    {
        #region Constructor

        ILogger Logger { get; }
        IOptionsSnapshot<Settings.WorkElements> WorkElementSettings { get; }
        IWorkElementTypeAttributeData WorkElementTypeAttributeData { get; }
        IWorkElementTypeData WorkElementTypeData { get; }

        public WorkElementController(ILogger<WorkElementController> logger, 
            IOptionsSnapshot<Settings.WorkElements> workElementOptions,
            IWorkElementTypeAttributeData workElementTypeAttributeData,
            IWorkElementTypeData workElementTypeData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            WorkElementSettings = workElementOptions ?? throw new ArgumentNullException(nameof(workElementOptions));
            WorkElementTypeAttributeData = workElementTypeAttributeData ?? throw new ArgumentNullException(nameof(workElementTypeAttributeData));
            WorkElementTypeData = workElementTypeData ?? throw new ArgumentNullException(nameof(workElementTypeData));
        }

        #endregion

        #region Public API Methods

        /// <summary>
        /// Simply return all work element types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WorkElementType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("WorkElementType")]
        public IActionResult GetWorkElementTypes()
        {
            using (var workElementTypeLogic = new WorkElementTypeLogic(WorkElementTypeData))
            {
                var workElementTypes = workElementTypeLogic.GetWorkElementTypes();
                return Ok(workElementTypes);
            }
        }

        /// <summary>
        /// Simply return all work element type attributes to be used in a client list or dropdown.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WorkElementTypeAttribute>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("WorkElementTypeAttribute")]
        public List<WorkElementTypeAttribute> GetWorkElementTypeAttributes()
        {
            using (var workElementTypeAttributeLogic = new WorkElementTypeAttributeLogic(WorkElementTypeAttributeData))
            {
                return workElementTypeAttributeLogic.GetWorkElementTypeAttributes();
            }
        }

        [HttpPost]
        [Route("images/upload/{billOfProcessProcessId}/{workElementId}")]
        [RequestSizeLimit(5242880)]
        // This is not recommended
        // [DisableRequestSizeLimit]
        public IActionResult Upload(int billOfProcessProcessId, int workElementId, IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var fileExtension = file.FileName.Split('.')[1].ToString();
                    var updatedFilePath = string.Format(@"{0}\{1}_{2}.{3}", WorkElementSettings.Value.ImagesFolder, billOfProcessProcessId, workElementId, fileExtension);
                    // file.
                    using (Stream fileStream = new FileStream(updatedFilePath, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fileStream);
                    }

                    return Ok($"Uploaded: {updatedFilePath}");
                }

                return Ok($"Nothing Uploaded");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred when uploading the work element image.");
                return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when uploading the work element image.  Please try again or contact MES support for help."));
            }
        }

        /// <summary>
        /// Demonstrate how to use application settings
        /// </summary>
        /// <returns>Application settings</returns>
        /// <remarks>Don't do this in production! You can unintentionally unclose sensitive information</remarks>
        [HttpGet("Settings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Settings.WorkElements GetSettings()
        {
            return WorkElementSettings.Value;
        }

        #endregion
    }
}
