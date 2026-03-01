using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.Global;

namespace ue_mes_test.FakeObjects
{
    public class EquipmentFake
    {
        public static Equipment GetValidEquipment()
        {
            var validEquipment = new Equipment()
            {
                EquipmentId = 1,
                Name = "PHX11A-BARCODE_MARKER",
                Line = SharedFakes.lineForEquipmentObjects,
                Process = new Process()
                {
                    ProcessId = 1,
                    Number = "10114",
                    Name = "BARCODE_MARKER",
                    LoopNumber = 1
                }
            };

            return validEquipment;
        }

        public static Equipment GetValidStartingEquipmentForBillOfProcess()
        {
            var validEquipment = new Equipment()
            {
                EquipmentId = 1,
                Name = "RWC11A-LAB_UV_VIS_MEASUREMENT",
                Line = SharedFakes.lineForEquipmentObjects,
                Process = new Process()
                {
                    ProcessId = 1,
                    Number = "00050",
                    Name = "LAB_UV_VIS_MEASUREMENT",
                    LoopNumber = 0
                }
            };

            return validEquipment;
        }

        public static List<Equipment> GetValidEquipmentListBySiteName()
        {
            var validEquipment = new List<Equipment>()
            {
                new Equipment() {
                    EquipmentId = 1,
                    Name = "PHX11A-BARCODE_MARKER",
                    Line = SharedFakes.lineForEquipmentObjects,
                    Process = new Process()
                    {
                        ProcessId = 1,
                        Number = "10114",
                        Name = "BARCODE_MARKER",
                        LoopNumber = 1
                    }
                },
                new Equipment() {
                    EquipmentId = 2,
                    Name = "PHX11A-BUSBAR_APPLY",
                    Line = SharedFakes.lineForEquipmentObjects,
                    Process = new Process () 
                    { 
                        ProcessId = 2,
                        Number = "20210",
                        Name = "BUSBAR_APPLY",
                        LoopNumber = 2
                    }
                },
                new Equipment() {
                    EquipmentId = 3,
                    Name = "PHX11A-ACTIVE_LAYER_COATER",
                    Line = SharedFakes.lineForEquipmentObjects,
                    Process = new Process ()
                    {
                        ProcessId = 3,
                        Number = "40210",
                        Name = "ACTIVE_LAYER_COATER",
                        LoopNumber = 4
                    }
                }
            };

            return validEquipment;
        }

        public static List<Equipment> GetValidEquipmentListBySiteNameAndProcessId()
        {
            var validEquipment = new List<Equipment>()
            {
                new Equipment() {
                    EquipmentId = 2,
                    Name = "PHX11A-BUSBAR_APPLY",
                    Line = SharedFakes.lineForEquipmentObjects,
                    Process = new Process ()
                    {
                        ProcessId = 2,
                        Number = "20210",
                        Name = "BUSBAR_APPLY",
                        LoopNumber = 2
                    }
                }
            };

            return validEquipment;
        }
    }
}
