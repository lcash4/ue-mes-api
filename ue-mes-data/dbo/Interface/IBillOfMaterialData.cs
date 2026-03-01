using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IBillOfMaterialData
    {
        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="partId">Unique Part ID that is built from this <see cref="BillOfMaterial"/></param>
        /// <returns></returns>
        public BillOfMaterial GetBillOfMaterialByPartId(int partId);

        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="billOfMaterialId">Unique Bill of Material ID for the <see cref="BillOfMaterial"/></param>
        /// <returns></returns>
        public BillOfMaterial GetBillOfMaterialById(int billOfMaterialId);

        /// <summary>
        /// Returns a full <see cref="BillOfMaterial"/> entity with child entities included.
        /// The most recent by effective start date will be returned
        /// </summary>
        /// <param name="consumedPartId">Unique Part ID that needs to be included in the BOM part list</param>
        /// <returns></returns>
        public List<BillOfMaterial> GetBillOfMaterialsThatConsumePartId(int consumedPartId);

        /// <summary>
        /// Add a new bill of material
        /// </summary>
        /// <param name="billOfMaterial"></param>
        public void AddBillOfMaterial(BillOfMaterial billOfMaterial);

        /// <summary>
        /// Update an existing bill of material.  
        /// </summary>
        /// <param name="billOfMaterial"></param>
        public void UpdateBillOfMaterial(BillOfMaterial billOfMaterial);
    }
}
