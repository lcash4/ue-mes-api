using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;

namespace ue_mes_test.FakeObjects
{
    public class BillOfProcessFake
    {
        public static BillOfProcess GetSimpleBillOfProcessWithNoProcesses()
        {
            return new BillOfProcess()
            {
                BillOfProcessId = 1,
                Name = "BOP0200001A",
                Description = "UE Coated Lite",
                EffectiveStartDate = new DateTime(2024, 1, 1, 0, 0, 0),
                EffectiveStartDateUtc = new DateTime(2024, 1, 1, 4, 0, 0),
                EffectiveEndDate = null,
                EffectiveEndDateUtc = null,
                Part = new Part()
                {
                    PartId = 2,
                    PartNumber = "PT0200001",
                    PartRevision = "A",
                    Description = "UE Coated Lite",
                    IsActive = true,
                    UnitOfMeasure = new UnitOfMeasure()
                    {
                        UnitOfMeasureId = 3,
                        Name = "Piece",
                        ShortName = "pc"
                    }
                }
            };
        }

        public static BillOfProcess GetSimpleBillOfProcessWithProcesses()
        {
            return new BillOfProcess()
            {
                BillOfProcessId = 1,
                Name = "BOM0200001A",
                Description = "UE Coated Lite",
                EffectiveStartDate = new DateTime(2024, 1, 1, 0, 0, 0),
                EffectiveStartDateUtc = new DateTime(2024, 1, 1, 4, 0, 0),
                EffectiveEndDate = null,
                EffectiveEndDateUtc = null,
                Part = new Part()
                {
                    PartId = 2,
                    PartNumber = "PT0200001",
                    PartRevision = "A",
                    Description = "UE Coated Lite",
                    IsActive = true,
                    UnitOfMeasure = new UnitOfMeasure()
                    {
                        UnitOfMeasureId = 3,
                        Name = "Piece",
                        ShortName = "pc"
                    }
                },
                BillOfProcessProcesses = new List<BillOfProcessProcess>()
                {
                    new BillOfProcessProcess()
                    {
                        BillOfProcessProcessId = 1,
                        Sequence = 1,
                        IsActive = true,
                        Process = new Process ()
                        {
                            ProcessId = 1,
                            Number = "00050",
                            Name = "LAB_UV_VIS_MEASUREMENT",
                            LoopNumber = 0
                        },
                        BillOfProcessProcessWorkElements = new List<BillOfProcessProcessWorkElement>()
                        {
                            new BillOfProcessProcessWorkElement()
                            {
                                BillOfProcessProcessWorkElementId = 1,
                                Name = "Run UV Vis Test",
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
                                        BillOfProcessProcessWorkElementAttributeId = 1,
                                        AttributeValue = "Load glass and run UV Vis test",
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
                    },
                    new BillOfProcessProcess()
                    {
                        BillOfProcessProcessId = 2,
                        Sequence = 2,
                        IsActive = true,
                        Process = new Process ()
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
                    },
                    new BillOfProcessProcess()
                    {
                        BillOfProcessProcessId = 3,
                        Sequence = 3,
                        IsActive = true,
                        Process = new Process ()
                        {
                            ProcessId = 19,
                            Number = "00950",
                            Name = "LAB_ZETA_IMAGING",
                            LoopNumber = 0
                        },
                        BillOfProcessProcessWorkElements = new List<BillOfProcessProcessWorkElement>()
                        {
                            new BillOfProcessProcessWorkElement()
                            {
                                BillOfProcessProcessWorkElementId = 4,
                                Name = "Make Go\\No Go Decision",
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
                                        BillOfProcessProcessWorkElementAttributeId = 4,
                                        AttributeValue = "Send to IGU assembly if the module is good",
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
                    }
                }
            };
        }

        public static BillOfProcess GetValidNewBillOfProcess()
        {
            return new BillOfProcess()
            {
                BillOfProcessId = 2,
                Name = "BOP0300001A",
                Description = "UE Power Module",
                EffectiveStartDate = new DateTime(2024, 1, 1, 0, 0, 0),
                EffectiveStartDateUtc = new DateTime(2024, 1, 1, 4, 0, 0),
                EffectiveEndDate = null,
                EffectiveEndDateUtc = null,
                Part = new Part()
                {
                    PartId = 3,
                    PartNumber = "PT0300001",
                    PartRevision = "A",
                    Description = "UE Power Module",
                    IsActive = true,
                    UnitOfMeasure = new UnitOfMeasure()
                    {
                        UnitOfMeasureId = 3,
                        Name = "Piece",
                        ShortName = "pc"
                    }
                },
                BillOfProcessProcesses = new List<BillOfProcessProcess>()
                {
                    new BillOfProcessProcess()
                    {
                        BillOfProcessProcessId = 1,
                        Sequence = 1,
                        IsActive = true,
                        Process = new Process ()
                        {
                            ProcessId = 7,
                            Number = "01000",
                            Name = "LAB_IGU_ASSEMBLY",
                            LoopNumber = 0
                        },
                        BillOfProcessProcessWorkElements = new List<BillOfProcessProcessWorkElement>()
                        {
                            new BillOfProcessProcessWorkElement()
                            {
                                BillOfProcessProcessWorkElementId = 1,
                                Name = "Assemble the IGU",
                                Sequence = 1,
                                IsRequired = true,
                                IsActive = true,
                                WorkElementType = new WorkElementType()
                                {
                                    WorkElementTypeId = 2,
                                    Name = "Image",
                                    Description = "Similar to text, but also includes an option for 1 image to display instructions graphically"
                                },
                                BillOfProcessProcessWorkElementAttributes = new List<BillOfProcessProcessWorkElementAttribute>()
                                {
                                    new BillOfProcessProcessWorkElementAttribute()
                                    {
                                        BillOfProcessProcessWorkElementAttributeId = 1,
                                        AttributeValue = "Build the IGU.  Should look like the image below",
                                        WorkElementTypeAttribute = new WorkElementTypeAttribute()
                                        {
                                            Name = "Work Element Text",
                                            IsRequiredAtSetup = true,
                                            IsRequiredAtRun = true,
                                            WorkElementType = new WorkElementType()
                                            {
                                                WorkElementTypeId = 2,
                                                Name = "Image",
                                                Description = "Similar to text, but also includes an option for 1 image to display instructions graphically"
                                            }
                                        }
                                    },
                                    new BillOfProcessProcessWorkElementAttribute()
                                    {
                                        BillOfProcessProcessWorkElementAttributeId = 2,
                                        AttributeValue = "7_9.jpg",
                                        WorkElementTypeAttribute = new WorkElementTypeAttribute()
                                        {
                                            Name = "Work Element Image",
                                            IsRequiredAtSetup = true,
                                            IsRequiredAtRun = true,
                                            WorkElementType = new WorkElementType()
                                            {
                                                WorkElementTypeId = 2,
                                                Name = "Image",
                                                Description = "Similar to text, but also includes an option for 1 image to display instructions graphically"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public static BillOfProcess GetInvalidNewBillOfProcess()
        {
            // This will be used to attempt an add, but since this billOfProcess is already in the list, it will fail.
            return GetSimpleBillOfProcessWithNoProcesses();
        }

        public static List<BillOfProcess> GetSimpleBillOfProcessList()
        {
            var validBillOfProcessList = new List<BillOfProcess>
            {
                GetSimpleBillOfProcessWithNoProcesses(),
                GetSimpleBillOfProcessWithProcesses()
            };

            return validBillOfProcessList;
        }
    }
}
