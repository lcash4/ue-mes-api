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
    public class InventoryItemScrapLogic : LogicBase
    {
        IInventoryItemScrapData InventoryItemScrapData { get; }

        #region Constructor

        public InventoryItemScrapLogic(IInventoryItemScrapData inventoryItemScrapData)
        {
            InventoryItemScrapData = inventoryItemScrapData ?? throw new ArgumentNullException(nameof(inventoryItemScrapData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return an InventoryItemScrap object by its serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number to search</param>
        /// <returns></returns>
        public InventoryItemScrap GetInventoryItemScrapBySerialNumber(string serialNumber)
        {
            return InventoryItemScrapData.GetInventoryItemScrapBySerialNumber(serialNumber);
        }

        /// <summary>
        /// Add a new inventory item scrap.  Typically, this will be done from a quality screen
        /// </summary>
        /// <param name="inventoryItemScrap"></param>
        public void AddInventoryItemScrap(InventoryItemScrap inventoryItemScrap)
        {
            // Need to check if the order item unit is already scrapped.  If so, return "unit already scrapped" exception
            var existingScrap = InventoryItemScrapData.GetInventoryItemScrapBySerialNumber(inventoryItemScrap.InventoryItem.SerialNumber);
            if (existingScrap != null)
                throw new Exception(string.Format("Inventory Item with Serial {0}, is already scrapped.", inventoryItemScrap.InventoryItem.SerialNumber));

            InventoryItemScrapData.AddInventoryItemScrap(inventoryItemScrap);

        }

        #endregion
    }
}
