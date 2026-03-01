using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class SuppliersController : ControllerBase
    {
        #region Constructor

        ILogger Logger { get; }
        ISupplierData SupplierData { get; }

        public SuppliersController(ILogger<SuppliersController> logger, ISupplierData supplierData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SupplierData = supplierData ?? throw new ArgumentNullException(nameof(supplierData));
        }

        #endregion

        #region Public API Methods

        /// <summary>
        /// Simply return a supplier by the supplied ID
        /// </summary>
        /// <param name="supplierId">ID of the supplier to search</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Supplier), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetByID/{supplierId}")]
        public Supplier GetSupplierById(int supplierId)
        {
            using (var supplierLogic = new SupplierLogic(SupplierData))
            {
                return supplierLogic.GetSupplierById(supplierId);
            }
        }

        /// <summary>
        /// Simply return a supplier by the supplied name
        /// </summary>
        /// <param name="supplierName">name of the supplier to search</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Supplier), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetByName/{supplierName}")]
        public Supplier GetSupplierByName(string supplierName)
        {
            using (var supplierLogic = new SupplierLogic(SupplierData))
            {
                return supplierLogic.GetSupplierByName(supplierName);
            }
        }

        /// <summary>
        /// Simply return all suppliers for use in a client list or dropdown
        /// </summary>
        /// <param name="includeInactive">Only include inactive when set to true.  Defaults to false</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Supplier>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetAll/{includeInactive}")]
        public List<Supplier> GetAllSuppliers(bool includeInactive = false)
        {
            using (var supplierLogic = new SupplierLogic(SupplierData))
            {
                return supplierLogic.GetAllSuppliers(includeInactive);
            }
        }

        /// <summary>
        /// Adds a new supplier
        /// If a supplier with the same name already exists, an exception will be thrown
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("Add")]
        public IActionResult AddSupplier([FromBody] Supplier supplier)
        {
            using (var supplierLogic = new SupplierLogic(SupplierData))
            {
                try
                {
                    supplierLogic.AddSupplier(supplier);
                    Logger.LogInformation("Added supplier successfully: {0}", supplier);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the supplier.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the supplier.  Please verify entry or contact MES support for help."));
                }
            }
        }
        #endregion
    }
}
