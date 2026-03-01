using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Authorization.Interface;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class BillOfMaterialLogic : LogicBase
    {
        IBillOfMaterialData BillOfMaterialData { get; }

        #region Constructor

        public BillOfMaterialLogic(IBillOfMaterialData billOfMaterialData)
        {
            BillOfMaterialData = billOfMaterialData ?? throw new ArgumentNullException(nameof(billOfMaterialData));
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="partId">Unique Part ID that is built from this <see cref="BillOfMaterial"/></param>
        /// <returns></returns>
        public BillOfMaterial GetBillOfMaterialByPartId(int partId)
        {
            return BillOfMaterialData.GetBillOfMaterialByPartId(partId);
        }

        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="billOfMaterialId">Unique Bill of Material ID for the <see cref="BillOfMaterial"/></param>
        /// <returns></returns>
        public BillOfMaterial GetBillOfMaterialById(int billOfMaterialId)
        {
            return BillOfMaterialData.GetBillOfMaterialById(billOfMaterialId);
        }

        /// <summary>
        /// Add a new bill of material
        /// </summary>
        /// <param name="billOfMaterial"></param>
        public void AddBillOfMaterial(BillOfMaterial billOfMaterial)
        {
            // Need to check if bill of material already exists.  If so, return "Bill Of Material already exists" exception
            var existingBillOfMaterial = BillOfMaterialData.GetBillOfMaterialByPartId(billOfMaterial.Part.PartId);
            if (existingBillOfMaterial != null)
                throw new InvalidDataException(string.Format("Bill Of Material {0} already exists", billOfMaterial.Name));

            // The BOM name is an auto-generated string with the format "BOM<Part.PartNumber.Replace("PT,"")><Part.Revision>"
            // This ensures the BOM names are unique and not require the caller to know the format.
            billOfMaterial.Name = string.Format("BOM{0}{1}", billOfMaterial.Part.PartNumber.Replace("PT", string.Empty), billOfMaterial.Part.PartRevision);

            BillOfMaterialData.AddBillOfMaterial(billOfMaterial);
        }

        /// <summary>
        /// Update an existing bill of material.  
        /// </summary>
        /// <param name="billOfMaterial"></param>
        public void UpdateBillOfMaterial(BillOfMaterial billOfMaterial)
        {
            BillOfMaterialData.UpdateBillOfMaterial(billOfMaterial);
        }

        #endregion
    }
}
