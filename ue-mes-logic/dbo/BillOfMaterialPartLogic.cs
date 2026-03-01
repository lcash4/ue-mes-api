using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;

namespace ue_mes_logic.dbo
{
    public class BillOfMaterialPartLogic : LogicBase
    {
        IBillOfMaterialPartData BillOfMaterialPartData { get; }

        #region Constructor

        public BillOfMaterialPartLogic(IBillOfMaterialPartData billOfMaterialPartData)
        {
            BillOfMaterialPartData = billOfMaterialPartData ?? throw new ArgumentNullException(nameof(billOfMaterialPartData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Replace all existing bill of material parts with a new set.
        /// </summary>
        /// <param name="billOfMaterialId">ID of the bill of material to update</param>
        /// <param name="billOfMaterialParts">The new list of parts to assign to this bill of material</param>
        public void UpdateBillOfMaterialPartsList(int billOfMaterialId, List<BillOfMaterialPart> billOfMaterialParts)
        {
            // This method will first delete all existing BOM part records for the given bill of material.  
            // TODO: Put this into a transaction later

            // First delete the old
            BillOfMaterialPartData.DeleteBillOfMaterialParts(billOfMaterialId);

            // Then add the new
            BillOfMaterialPartData.AddBillOfMaterialParts(billOfMaterialParts);
        }

        #endregion
    }
}
