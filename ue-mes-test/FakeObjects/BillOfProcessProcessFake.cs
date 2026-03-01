using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;

namespace ue_mes_test.FakeObjects
{
    public class BillOfProcessProcessFake
    {
        public static BillOfProcessProcess GetSimpleBillOfProcessProcessWithNoWorkElements()
        {
            return new BillOfProcessProcess()
            {
                BillOfProcessProcessId = 1,
                Sequence = 1,
                IsActive = true,
                BillOfProcess = new BillOfProcess() { BillOfProcessId = 1 },
                Process = new Process()
                {
                    ProcessId = 1,
                    Number = "00050",
                    Name = "LAB_UV_VIS_MEASUREMENT",
                    LoopNumber = 0
                }
            };
        }

        public static BillOfProcessProcess GetSimpleBillOfProcessProcessWithWorkElements()
        {
            return new BillOfProcessProcess()
            {
                BillOfProcessProcessId = 2,
                Sequence = 2,
                IsActive = true,
                BillOfProcess = new BillOfProcess() { BillOfProcessId = 1 },
                Process = new Process()
                {
                    ProcessId = 12,
                    Number = "00600",
                    Name = "LAB_ACTIVE_LAYER_COATER",
                    LoopNumber = 0
                },
                BillOfProcessProcessWorkElements = new List<BillOfProcessProcessWorkElement>()
                {
                    new BillOfProcessProcessWorkElement()
                    {
                        BillOfProcessProcessWorkElementId = 2,
                        Name = "Capture coater recipe",
                        Sequence = 1,
                        IsRequired = true,
                        IsActive = true,
                        WorkElementType = new WorkElementType()
                        {
                            WorkElementTypeId = 3,
                            Name = "Data Collection",
                            Description = "One to many process data points can be collected by the user"
                        },
                        BillOfProcessProcessWorkElementAttributes = new List<BillOfProcessProcessWorkElementAttribute>()
                        {
                            new BillOfProcessProcessWorkElementAttribute()
                            {
                                BillOfProcessProcessWorkElementAttributeId = 2,
                                AttributeValue = "",
                                WorkElementTypeAttribute = new WorkElementTypeAttribute()
                                {
                                    Name = "Active Layer Coater Recipe",
                                    IsRequiredAtSetup = false,
                                    IsRequiredAtRun = true,
                                    WorkElementType = new WorkElementType()
                                    {
                                        WorkElementTypeId = 3,
                                        Name = "Data Collection",
                                        Description = "One to many process data points can be collected by the user"
                                    },
                                    WorkElementTypeAttributeListItems = new List<WorkElementTypeAttributeListItem> ()
                                    {
                                        new WorkElementTypeAttributeListItem()
                                        {
                                            WorkElementTypeAttributeListItemId = 1,
                                            Name = "UE3.0",
                                            Description = "UE3.0"
                                        },
                                        new WorkElementTypeAttributeListItem()
                                        {
                                            WorkElementTypeAttributeListItemId = 2,
                                            Name = "UE3.3",
                                            Description = "UE3.3"
                                        },
                                        new WorkElementTypeAttributeListItem()
                                        {
                                            WorkElementTypeAttributeListItemId = 3,
                                            Name = "D310",
                                            Description = "D310"
                                        },
                                        new WorkElementTypeAttributeListItem()
                                        {
                                            WorkElementTypeAttributeListItemId = 4,
                                            Name = "UE361:A2",
                                            Description = "UE361:A2"
                                        }
                                    }
                                }

                            }
                        }
                    },

                    new BillOfProcessProcessWorkElement()
                    {
                        BillOfProcessProcessWorkElementId = 3,
                        Name = "Run glass through coater",
                        Sequence = 2,
                        IsRequired = true,
                        IsActive = true,
                        WorkElementType = new WorkElementType()
                        {
                            WorkElementTypeId = 1,
                            Name = "Text",
                            Description = "Text instructions are displayed to the user.  Also supports HTML."
                        },
                        BillOfProcessProcessWorkElementAttributes = new List<BillOfProcessProcessWorkElementAttribute>()
                        {
                            new BillOfProcessProcessWorkElementAttribute()
                            {
                                BillOfProcessProcessWorkElementAttributeId = 3,
                                AttributeValue = "Run the glass through the coater",
                                WorkElementTypeAttribute = new WorkElementTypeAttribute()
                                {
                                    Name = "Work Element Text",
                                    IsRequiredAtSetup = true,
                                    IsRequiredAtRun = true,
                                    WorkElementType = new WorkElementType()
                                    {
                                        WorkElementTypeId = 1,
                                        Name = "Text",
                                        Description = "Text instructions are displayed to the user.  Also supports HTML."
                                    }
                                }

                            }
                        }
                    }
                }
            };
        }

        public static BillOfProcessProcess GetValidNewBillOfProcessProcess()
        {
            return new BillOfProcessProcess()
            {
                BillOfProcessProcessId = 0,
                Sequence = 4,
                IsActive = true,
                BillOfProcess = new BillOfProcess() { BillOfProcessId = 1 },
                Process = new Process()
                {
                    ProcessId = 21,
                    Number = "02000",
                    Name = "LAB_IGU_ARGON_TEST",
                    LoopNumber = 0
                },
                BillOfProcessProcessWorkElements = new List<BillOfProcessProcessWorkElement>()
                {
                    new BillOfProcessProcessWorkElement()
                    {
                        BillOfProcessProcessWorkElementId = 0,
                        Name = "Run IGU through Argon Test",
                        Sequence = 1,
                        IsRequired = true,
                        IsActive = true,
                        WorkElementType = new WorkElementType()
                        {
                            WorkElementTypeId = 1,
                            Name = "Text",
                            Description = "Text instructions are displayed to the user.  Also supports HTML."
                        },
                        BillOfProcessProcessWorkElementAttributes = new List<BillOfProcessProcessWorkElementAttribute>()
                        {
                            new BillOfProcessProcessWorkElementAttribute()
                            {
                                BillOfProcessProcessWorkElementAttributeId = 0,
                                AttributeValue = "Run IGU through Argon Test and note measured result",
                                WorkElementTypeAttribute = new WorkElementTypeAttribute()
                                {
                                    Name = "Work Element Text",
                                    IsRequiredAtSetup = true,
                                    IsRequiredAtRun = true,
                                    WorkElementType = new WorkElementType()
                                    {
                                        WorkElementTypeId = 1,
                                        Name = "Text",
                                        Description = "Text instructions are displayed to the user.  Also supports HTML."
                                    }
                                }

                            }
                        }
                    }
                }
            };
        }

        public static BillOfProcessProcess GetInvalidNewBillOfProcessProcess()
        {
            // This will be used to attempt an add, but since this billOfProcessProcess is already in the list, it will fail.
            return GetSimpleBillOfProcessProcessWithNoWorkElements();
        }

        public static List<BillOfProcessProcess> GetSimpleBillOfProcessProcessList()
        {
            var validBillOfProcessProcessList = new List<BillOfProcessProcess>
            {
                GetSimpleBillOfProcessProcessWithNoWorkElements(),
                GetSimpleBillOfProcessProcessWithWorkElements()
            };

            return validBillOfProcessProcessList;
        }

        public static List<BillOfProcessProcess> GetSimpleBillOfProcessProcessListWithNewBillOfProcessProcessIncluded()
        {
            var validBillOfProcessProcessList = new List<BillOfProcessProcess>
            {
                GetSimpleBillOfProcessProcessWithNoWorkElements(),
                GetSimpleBillOfProcessProcessWithWorkElements(),
                GetValidNewBillOfProcessProcess()
            };

            return validBillOfProcessProcessList;
        }
    }
}
