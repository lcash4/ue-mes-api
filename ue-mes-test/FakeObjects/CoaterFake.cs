using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Global;

namespace ue_mes_test.FakeObjects
{
    public class CoaterFake
    {
        public static Coater GetValidCoater()
        {
            var validCoater = new Coater()
            {
                CoaterId = 1,
                Number = 1,
                Equipment = new Equipment()
                {
                    EquipmentId = 3,
                    Name = "PHX11A-ACTIVE_LAYER_COATER",
                    Line = SharedFakes.lineForEquipmentObjects,
                    Process = new Process()
                    {
                        ProcessId = 3,
                        Number = "40210",
                        Name = "ACTIVE_LAYER_COATER",
                        LoopNumber = 4
                    }
                }
            };

            return validCoater;
        }
    }
}
