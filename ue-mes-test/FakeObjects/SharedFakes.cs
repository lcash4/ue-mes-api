using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Authorization;
using ue_mes_entities.Global;

namespace ue_mes_test.FakeObjects
{
    public class SharedFakes
    {
        public static Line lineForEquipmentObjects = new Line()
        {
            LineId = 1,
            Name = "A",
            DisplayName = "PHX11A",
            Unit = new Unit()
            {
                UnitId = 1,
                Name = "1",
                DisplayName = "PHX11",
                Plant = new Plant()
                {
                    PlantId = 1,
                    Name = "1",
                    DisplayName = "PHX1",
                    Site = new Site()
                    {
                        SiteId = 1,
                        Name = "Phoenix, AZ USA",
                        DisplayName = "PHX"
                    }
                }
            }
        };
    }
}
