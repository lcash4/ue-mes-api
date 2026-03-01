using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Enums;

namespace ue_mes_data.dbo.Interface
{
    public interface IPartTypeData
    {
        /// <summary>
        /// Simply return all part types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<PartTypeEnum> GetPartTypes();
    }
}
