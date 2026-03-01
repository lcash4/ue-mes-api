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
    public class InventoryLocationTypeLogic : LogicBase
    {
        IInventoryLocationTypeData InventoryLocationTypeData { get; }

        #region Constructor

        public InventoryLocationTypeLogic(IInventoryLocationTypeData inventoryLocationTypeData)
        {
            InventoryLocationTypeData = inventoryLocationTypeData ?? throw new ArgumentNullException(nameof(inventoryLocationTypeData));
        }

        #endregion

        /// <summary>
        /// Returns all <see cref="InventoryLocationType"/> entities.
        /// </summary>
        /// <returns></returns>
        public List<InventoryLocationType> GetAllInventoryLocationTypes()
        {
            return InventoryLocationTypeData.GetAllInventoryLocationTypes();
        }
    }
}
