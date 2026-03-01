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
    public class WorkElementTypeAttributeLogic : LogicBase
    {
        IWorkElementTypeAttributeData WorkElementTypeAttributeData { get; }

        #region Constructor

        public WorkElementTypeAttributeLogic(IWorkElementTypeAttributeData workElementTypeAttributeData)
        {
            WorkElementTypeAttributeData = workElementTypeAttributeData ?? throw new ArgumentNullException(nameof(workElementTypeAttributeData));
        }

        #endregion

        /// <summary>
        /// Simply return all work element type attributes to be used in a client list or dropdown.
        /// </summary>
        /// <returns></returns>
        public List<WorkElementTypeAttribute> GetWorkElementTypeAttributes()
        {
            return WorkElementTypeAttributeData.GetWorkElementTypeAttributes();
        }
    }
}
