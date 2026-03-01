using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IOrderItemData
    {
        /// <summary>
        /// Returns all active (order state = started\released) order items given the provided part IDs.
        /// This call will typically used in order management screens, such as starting orders
        /// </summary>
        /// <param name="partIds">list of part IDs to search</param>
        /// <returns></returns>
        public List<OrderItem> GetActiveOrderItemsForParts(List<int> partIds);
    }
}
