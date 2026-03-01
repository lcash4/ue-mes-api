using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IBillOfMaterialPartData
    {
        /// <summary>
        /// Add a range of parts to a bill of material
        /// </summary>
        /// <param name="billOfMaterialParts"></param>
        public void AddBillOfMaterialParts(List<BillOfMaterialPart> billOfMaterialParts);

        /// <summary>
        /// Will delete all <see cref="BillOfMaterialPart"/> records that match the incoming billOfMaterialId
        /// </summary>
        /// <param name="billOfMaterialId"></param>
        public void DeleteBillOfMaterialParts(int billOfMaterialId);
    }
}
