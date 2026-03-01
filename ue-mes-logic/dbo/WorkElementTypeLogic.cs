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
    public class WorkElementTypeLogic : LogicBase
    {
        IWorkElementTypeData WorkElementTypeData { get; }

        #region Constructor

        public WorkElementTypeLogic(IWorkElementTypeData workElementTypeData)
        {
            WorkElementTypeData = workElementTypeData ?? throw new ArgumentNullException(nameof(workElementTypeData));
        }

        #endregion

        /// <summary>
        /// Simply return all work element types for use in a client list or dropdown
        /// </summary>
        /// <returns></returns>
        public List<WorkElementType> GetWorkElementTypes()
        {
            return WorkElementTypeData.GetWorkElementTypes();
        }
    }
}
