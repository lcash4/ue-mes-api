using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using ue_mes_api.Repo;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;
using ue_mes_logic.dbo;

namespace ue_mes_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PartsController : ControllerBase
    {
        #region Constants

        private IReadOnlyList<PartTypeEnum> rawMaterialPartType = new List<PartTypeEnum>() { PartTypeEnum.RawMaterial };
        private IReadOnlyList<PartTypeEnum> subAssemblyPartType = new List<PartTypeEnum>() { PartTypeEnum.SubAssembly };
        private IReadOnlyList<PartTypeEnum> finishedGoodPartType = new List<PartTypeEnum>() { PartTypeEnum.FinishedGood };
        
        #endregion

        #region Constructor

        ILogger Logger { get; }
        IBillOfMaterialData BillOfMaterialData { get; }
        IEquipmentData EquipmentData { get; }
        IItemAttributeTypeData ItemAttributeTypeData { get; }
        IPartData PartData { get; }
        IPartItemAttributeTypeData PartItemAttributeTypeData { get; }

        public PartsController(ILogger<PartsController> logger,
            IBillOfMaterialData billOfMaterialData,
            IEquipmentData equipmentData,
            IItemAttributeTypeData itemAttributeTypeData,
            IPartData partData,
            IPartItemAttributeTypeData partItemAttributeTypeData)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            BillOfMaterialData = billOfMaterialData ?? throw new ArgumentNullException(nameof(billOfMaterialData));
            EquipmentData = equipmentData ?? throw new ArgumentNullException(nameof(equipmentData));
            ItemAttributeTypeData = itemAttributeTypeData ?? throw new ArgumentNullException(nameof(itemAttributeTypeData));
            PartData = partData ?? throw new ArgumentNullException(nameof(partData));
            PartItemAttributeTypeData = partItemAttributeTypeData ?? throw new ArgumentNullException(nameof(partItemAttributeTypeData));
        }

        #endregion

        #region Public API Methods

        #region Part Methods

        /// <summary>
        /// Return all parts and include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Part>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("{includeInactive}")]
        public List<Part> GetAllParts(bool includeInactive = false)
        {
            using (var partLogic = new PartLogic(BillOfMaterialData, EquipmentData, PartData, PartItemAttributeTypeData))
            {
                return partLogic.GetAllParts(includeInactive);
            }
        }

        /// <summary>
        /// Return all parts where the part type is SubAssembly or FinishedGood.
        /// Also, include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Part>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetSubAssemblyAndFinishedGoodParts/{includeInactive}")]
        public List<Part> GetSubAssemblyAndFinishedGoodParts(bool includeInactive = false)
        {
            var subAssemblyAndFinishedGoodPartTypes = subAssemblyPartType.Concat(finishedGoodPartType);

            using (var partLogic = new PartLogic(BillOfMaterialData, EquipmentData, PartData, PartItemAttributeTypeData))
            {
                return partLogic.GetPartsByPartTypes(subAssemblyAndFinishedGoodPartTypes.ToList(),includeInactive);
            }
        }

        /// <summary>
        /// Return all parts where the part type is RawMaterial.
        /// Also, include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Part>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetRawMaterialParts/{includeInactive}")]
        public List<Part> GetRawMaterialParts(bool includeInactive = false)
        {
            var rawMaterialPartTypes = rawMaterialPartType;

            using (var partLogic = new PartLogic(BillOfMaterialData, EquipmentData, PartData, PartItemAttributeTypeData))
            {
                return partLogic.GetPartsByPartTypes(rawMaterialPartTypes.ToList(), includeInactive);
            }
        }

        /// <summary>
        /// Return all parts where the part type is RawMaterial or SubAssembly.
        /// Also, include inactive parts if <paramref name="includeInactive"/> is set to true
        /// </summary>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Part>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetRawMaterialAndSubAssemblyParts/{includeInactive}")]
        public List<Part> GetRawMaterialAndSubAssemblyParts(bool includeInactive = false)
        {
            var rawMaterialAndSubAssemblyPartTypes = rawMaterialPartType.Concat(subAssemblyPartType);

            using (var partLogic = new PartLogic(BillOfMaterialData, EquipmentData, PartData, PartItemAttributeTypeData))
            {
                return partLogic.GetPartsByPartTypes(rawMaterialAndSubAssemblyPartTypes.ToList(), includeInactive);
            }
        }

        /// <summary>
        /// Return a part by unique part ID
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Part), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetByID/{partId}")]
        public Part GetPartByPartId(int partId)
        {
            using (var partLogic = new PartLogic(BillOfMaterialData, EquipmentData, PartData, PartItemAttributeTypeData))
            {
                return partLogic.GetPartByPartId(partId);
            }
        }

        /// <summary>
        /// Add a new part
        /// </summary>
        /// <param name="part"></param>
        [ProducesResponseType(typeof(Part), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("Add")]
        public IActionResult AddPart([FromBody] Part part)
        {
            using (var partLogic = new PartLogic(BillOfMaterialData, EquipmentData, PartData, PartItemAttributeTypeData))
            {
                try
                {
                    partLogic.AddPart(part);
                    Logger.LogInformation("New part was added: {@part}", part);
                    return Created(part.PartNumber, part);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when adding the new part.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when adding the new part.  Please try again or contact MES support for help."));
                }
            }
        }

        /// <summary>
        /// Update an existing part.  
        /// Note, the part number and part revision columns are NOT mutable per our own logic.
        /// If the caller somehow changes them before calling this method, an exception will be thrown.
        /// </summary>
        /// <param name="part"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("{partId}")]
        public IActionResult UpdatePart(Part part)
        {
            using (var partLogic = new PartLogic(BillOfMaterialData, EquipmentData, PartData, PartItemAttributeTypeData))
            {
                try
                {
                    partLogic.UpdatePart(part);
                    Logger.LogInformation("Part was updated: {@part}", part);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when updating the part.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when updating the part.  Please try again or contact MES support for help."));
                }
            }
        }

        #endregion

        #region PartItemAttributeType Methods

        /// <summary>
        /// Return a list of ItemAttributeType for use in client drop downs or lists
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<ItemAttributeType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("ItemAttributeTypes/GetAll")]
        public List<ItemAttributeType> GetAllItemAttributeTypes()
        {
            using (var itemAttributeTypeLogic = new ItemAttributeTypeLogic(ItemAttributeTypeData))
            {
                return itemAttributeTypeLogic.GetAllItemAttributeTypes();
            }
        }

        /// <summary>
        /// Return a list of PartItemAttributeType for the unique part ID
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<PartItemAttributeType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("ItemAttributeTypes/GetByPartId/{partId}")]
        public List<PartItemAttributeType> GetPartItemAttributeTypesByPartId(int partId)
        {
            using (var partItemAttributeTypeLogic = new PartItemAttributeTypeLogic(PartItemAttributeTypeData))
            {
                return partItemAttributeTypeLogic.GetPartItemAttributeTypesByPartId(partId);
            }
        }

        /// <summary>
        /// Update a parts item attribute types.  
        /// </summary>
        /// <param name="partId">ID of the part to update</param>
        /// <param name="partItemAttributeTypes">updated list of PartItemAttributeTypes to save</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("ItemAttributeTypes/Update/{partId}")]
        public IActionResult UpdatePartItemAttributeTypes(int partId, [FromBody] List<PartItemAttributeType> partItemAttributeTypes)
        {
            using (var partItemAttributeTypeLogic = new PartItemAttributeTypeLogic(PartItemAttributeTypeData))
            {
                try
                {
                    partItemAttributeTypeLogic.UpdatePartItemAttributeTypeList(partId, partItemAttributeTypes);
                    Logger.LogInformation("{0} item attribute types were saved for part ID: {1}", partItemAttributeTypes.Count, partId);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred when updating the part item attribute type list.");
                    return StatusCode(StatusCodes.Status500InternalServerError, String.Format("An error occurred when updating the part item attribute type list.  Please try again or contact MES support for help."));
                }
            }
        }

        #endregion

        #endregion
    }
}
