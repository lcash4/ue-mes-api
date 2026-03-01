using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.Enums;

namespace ue_mes_logic.dbo
{
    public class PartTypeLogic : LogicBase
    {
        IPartTypeData PartTypeData { get; }

        #region Constructor

        public PartTypeLogic(IPartTypeData partTypeData)
        {
            PartTypeData = partTypeData ?? throw new ArgumentNullException(nameof(partTypeData));
        }

        #endregion

        /// <summary>
        /// Simply return all part types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<PartTypeEnum> GetPartTypes()
        {
            return PartTypeData.GetPartTypes();
        }
    }
}
