using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using ue_mes_data.Global;
using ue_mes_data.Global.Interface;
using ue_mes_entities.Global;

namespace ue_mes_logic.Global
{
    public class SiteLogic : LogicBase
    {
        ISiteData SiteData { get; }

        #region Constructor

        public SiteLogic(ISiteData siteData)
        {
            SiteData = siteData ?? throw new ArgumentNullException(nameof(siteData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get a site by its display name
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public Site GetSiteByDisplayName(string displayName)
        {
            if (!displayName.IsNullOrEmpty())
            {
                return SiteData.GetSiteByDisplayName(displayName);
            }

            return null;
        }

        #endregion
    }
}