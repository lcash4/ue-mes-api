using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IItemAttributeTypeData
    {
        /// <summary>
        /// Returns all <see cref="ItemAttributeType"/> entities.
        /// </summary>
        /// <returns></returns>
        public List<ItemAttributeType> GetAllItemAttributeTypes();
    }
}
