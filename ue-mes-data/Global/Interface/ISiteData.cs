using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Global;

namespace ue_mes_data.Global.Interface
{
    public interface ISiteData
    {
        /// <summary>
        /// Return a full site object for the given displayName
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public Site GetSiteByDisplayName(string displayName);

        /// <summary>
        /// Return a full site object for the given siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public Site GetSiteById(int siteId);
    }
}
