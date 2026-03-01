using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_test.FakeObjects
{
    public class BillOfMaterialFake
    {
        public static BillOfMaterial GetSimpleBillOfMaterialWithNoParts()
        {
            return new BillOfMaterial()
            {
                BillOfMaterialId = 1,
                Name = "BOM0200001A",
                Description = "UE Coated Lite",
                EffectiveStartDate = new DateTime(2024,1,1,0,0,0),
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

        public static BillOfMaterial GetSimpleBillOfMaterialWithParts()
        {
            return new BillOfMaterial()
            {
                BillOfMaterialId = 1,
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
                BillOfMaterialParts = new List<BillOfMaterialPart>()
                {
                    new BillOfMaterialPart()
                    {
                        BillOfMaterialPartId = 1,
                        Quantity = 1,
                        Part = new Part()
                        {
                            PartId = 4,
                            PartNumber = "PT0100002",
                            PartRevision = "A",
                            Description = "ITO Coated Lite",
                            IsActive = true,
                            UnitOfMeasure = new UnitOfMeasure()
                            {
                                UnitOfMeasureId = 3,
                                Name = "Piece",
                                ShortName = "pc"
                            }
                        }
                    }
                }
            };
        }

        public static BillOfMaterial GetValidNewBillOfMaterial()
        {
            return new BillOfMaterial()
            {
                BillOfMaterialId = 0,
                Name = "BOM0200002A",
                Description = "UE Coated and Laminated Lite",
                EffectiveStartDate = new DateTime(2024, 1, 1, 0, 0, 0),
                EffectiveStartDateUtc = new DateTime(2024, 1, 1, 4, 0, 0),
                EffectiveEndDate = null,
                EffectiveEndDateUtc = null,
                Part = new Part()
                {
                    PartId = 21,
                    PartNumber = "PT0200002",
                    PartRevision = "A",
                    Description = "UE Coated and Laminated Lite",
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

        public static BillOfMaterial GetInvalidNewBillOfMaterial()
        {
            // This will be used to attempt an add, but since this billOfMaterial is already in the list, it will fail.
            return GetSimpleBillOfMaterialWithNoParts();
        }
    }
}
