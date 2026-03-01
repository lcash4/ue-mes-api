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
    public interface IInventoryItemScrapData
    {
        /// <summary>
        /// Return an InventoryItemScrap object by its serial number
        /// </summary>
        /// <param name="serialNumber">Unique serial number to search</param>
        /// <returns></returns>
        public InventoryItemScrap GetInventoryItemScrapBySerialNumber(string serialNumber);

        /// <summary>
        /// Add a new inventory item scrap.  Typically, this will be done from a quality screen
        /// </summary>
        /// <param name="inventoryItemScrap"></param>
        public void AddInventoryItemScrap(InventoryItemScrap inventoryItemScrap);

        /// <summary>
        /// Delete an existing inventory item scrap record. Typically this is done when a scrapped item is reworked successfully.
        /// </summary>
        /// <param name="inventoryItemScrapId"></param>
        public void DeleteInventoryItemScrap(long inventoryItemScrapId);
    }
}
