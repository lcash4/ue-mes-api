using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IInventoryLocationTypeData
    {
        /// <summary>
        /// Returns all <see cref="InventoryLocationType"/> entities.
        /// </summary>
        /// <returns></returns>
        public List<InventoryLocationType> GetAllInventoryLocationTypes();
    }
}
