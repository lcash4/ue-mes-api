using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Global;

namespace ue_mes_test.FakeObjects
{
    public class SiteFake
    {
        public static Site GetValidSite()
        {
            var validSite = new Site()
            {
                SiteId = 1,
                Name = "Phoenix, AZ USA",
                DisplayName = "PHX"
            };

            return validSite;
        }
    }
}
