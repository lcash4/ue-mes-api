using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;

namespace ue_mes_logic.Global
{
    public class CoaterLogic : LogicBase
    {
        ICoaterData CoaterData { get; }

        #region Constructor

        public CoaterLogic(ICoaterData coaterData)
        {
            CoaterData = coaterData ?? throw new ArgumentNullException(nameof(coaterData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the first active layer coater for the site name
        /// </summary>
        /// <param name="siteName">Unique site name to search</param>
        /// <returns></returns>
        public Coater GetActiveLayerCoaterBySiteName(string siteName)
        {
            return CoaterData.GetActiveLayerCoaterBySiteName(siteName);
        }

        #endregion
    }
}
