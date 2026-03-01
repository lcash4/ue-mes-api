using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Global;

namespace ue_mes_test.FakeObjects
{
    public class ProcessFake
    {
        public static Process GetValidProcess()
        {
            var validProcess = new Process()
            {
                ProcessId = 1,
                Number = "10114",
                Name = "BARCODE_MARKER",
                LoopNumber = 1
            };

            return validProcess;
        }
        public static List<Process> GetEmptyProcessList()
        {
            return new List<Process>();
        }

        public static List<Process> GetValidProcessList()
        {
            var validProcessList = new List<Process>()
            {
                new Process () { ProcessId = 1,
                    Number = "10114",
                    Name = "BARCODE_MARKER",
                    LoopNumber = 1
                },
                new Process () { ProcessId = 2,
                    Number = "20210",
                    Name = "BUSBAR_APPLY",
                    LoopNumber = 2
                },
                new Process () { ProcessId = 3,
                    Number = "40210",
                    Name = "ACTIVE_LAYER_COATER",
                    LoopNumber = 4
                }
            };

            return validProcessList;
        }

        public static List<Process> GetValidProcessListNotInBillOfProcess()
        {
            var validProcessList = new List<Process>()
            {
                new Process () { ProcessId = 2,
                    Number = "20210",
                    Name = "BUSBAR_APPLY",
                    LoopNumber = 2
                },
                new Process () { ProcessId = 3,
                    Number = "40210",
                    Name = "ACTIVE_LAYER_COATER",
                    LoopNumber = 4
                }
            };

            return validProcessList;
        }
    }
}
