using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_data.dbo.Interface
{
    public interface IWorkElementTypeData
    {
        /// <summary>
        /// Simply return all work element types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<WorkElementType> GetWorkElementTypes();
    }
}
