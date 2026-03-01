// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;
using ue_mes_logic;
using ue_mes_logic.dbo;
using ue_mes_logic.Global;


    #region Constants

    IReadOnlyList<OrderStateEnum> createdOrderState = new List<OrderStateEnum>() { OrderStateEnum.Created };
    IReadOnlyList<OrderStateEnum> holdOrderState = new List<OrderStateEnum>() { OrderStateEnum.Hold };
    IReadOnlyList<OrderStateEnum> releasedOrderState = new List<OrderStateEnum>() { OrderStateEnum.Released };
    IReadOnlyList<OrderStateEnum> inProgressOrderState = new List<OrderStateEnum>() { OrderStateEnum.Started, OrderStateEnum.Complete };
    IReadOnlyList<OrderStateEnum> closedOrderState = new List<OrderStateEnum>() { OrderStateEnum.Closed };

    #endregion

//using (var siteLogic = new SiteLogic())
//{
//    var site = siteLogic.GetSiteByDisplayName("PHX");
//    Console.WriteLine("Site name = " + site.Name);
//}

//using (var unitOfMeasureLogic = new UnitOfMeasureLogic())
//{
//    var unitsOfMeasure = unitOfMeasureLogic.GetUnitsOfMeasure();
//    Console.WriteLine("Unit of measure count = " + unitsOfMeasure.Count());
//}

//using (var partLogic = new PartLogic())
//{
//    // I know I could call all parts and filter after, but this is to test the data layer
//    //var onlyActiveParts = partLogic.GetAllParts();
//    //var allParts = partLogic.GetAllParts(true);
//    //Console.WriteLine("All part count = " + allParts.Count());
//    //Console.WriteLine("Active only part count = " + onlyActiveParts.Count());

//    // Existing part, will result in error for add, success for update
//    //PartNumber	PartRevision	Description	UnitOfMeasureId
//    //PT001002 A   Float Glass 3
//    var part = new Part()
//    {
//        PartId = 11,
//        PartNumber = "PT001002",
//        PartRevision = "A",
//        Description = "Float Glass",
//        IsActive = true,
//        UnitOfMeasure = new UnitOfMeasure() { UnitOfMeasureId = 3 },
//        PartType = ue_mes_entities.Enums.PartTypeEnum.RawMaterial
//    };

//    // Existing part, but with modified part revision to test validation
//    //var part = new Part()
//    //{
//    //    PartId = 11,
//    //    PartNumber = "PT001002",
//    //    PartRevision = "C",
//    //    Description = "Float Glass",
//    //    IsActive = true,
//    //    UnitOfMeasure = new UnitOfMeasure() { UnitOfMeasureId = 3 },
//    //    PartType = ue_mes_entities.Enums.PartTypeEnum.RawMaterial
//    //};

//    // New part revision, should save for add, fail for update
//    //var part = new Part()
//    //{
//    //    PartNumber = "PT001002",
//    //    PartRevision = "B",
//    //    Description = "Float Glass update",
//    //    IsActive = true,
//    //    UnitOfMeasure = new UnitOfMeasure() { UnitOfMeasureId = 3 },
//    //    PartType = ue_mes_entities.Enums.PartTypeEnum.RawMaterial
//    //};

//    try
//    {
//        // partLogic.AddPart(part);
//        partLogic.UpdatePart(part);
//    }
//    catch (Exception ex)
//    { 
//        Console.WriteLine(ex.Message);
//    }    
//}

//using (var partTypeLogic = new PartTypeLogic())
//{
//    var partTypeList = partTypeLogic.GetPartTypes();
//    Console.WriteLine("part type count = " + partTypeList.Count().ToString());
//}

//using (var orderLogic = new OrderLogic())
//{

//    var allOrderStates = createdOrderState.Concat(holdOrderState).Concat(releasedOrderState).Concat(inProgressOrderState).Concat(closedOrderState);

//    var ordersList = orderLogic.GetOrdersByStates(allOrderStates.ToList());
//    Console.WriteLine("Order count = " + ordersList.Count());
//}

//using (var billOfProcessProcessData = new BillOfProcessProcessData())
//{
//    var aggregatedBopProcess = billOfProcessProcessData.GetBillOfProcessProcessesWithWorkElementHistoryByBillOfProcessId(2);
//    aggregatedBopProcess.ForEach(result =>
//    {
//        Console.WriteLine(string.Format(@"BOP ID: {0},  BOP Process ID: {1},  Process ID: {2},  BOP WE ID: {3},  History Count: {4}", result.BillOfProcessId, result.BillOfProcessProcessId, result.ProcessId, result.WorkElementId, result.WorkElementHistoryCount));
//    });

//}

//// Compare Inventory Item Attributes
//var currentInventoryItem = new InventoryItem()
//{
//    Part = new Part() { PartId = 1 },
//    InventoryItemAttributes = new List<InventoryItemAttribute> {
//        new InventoryItemAttribute() {
//            ItemAttributeType = new ItemAttributeType() { ItemAttributeTypeId=1 },
//            AttributeValue = "355"
//        },
//        new InventoryItemAttribute() {
//            ItemAttributeType = new ItemAttributeType() { ItemAttributeTypeId=2 },
//            AttributeValue = "508"
//        },
//        new InventoryItemAttribute() {
//            ItemAttributeType = new ItemAttributeType() { ItemAttributeTypeId=3 },
//            AttributeValue = "6"
//        }
//    }
//};

//var newInventoryItem1 = new InventoryItem()
//{
//    Part = new Part() { PartId = 1 },
//    InventoryItemAttributes = new List<InventoryItemAttribute> {
//        new InventoryItemAttribute() {
//            ItemAttributeType = new ItemAttributeType() { ItemAttributeTypeId=1 },
//            AttributeValue = "1500"
//        },
//        new InventoryItemAttribute() {
//            ItemAttributeType = new ItemAttributeType() { ItemAttributeTypeId=2 },
//            AttributeValue = "1000"
//        },
//        new InventoryItemAttribute() {
//            ItemAttributeType = new ItemAttributeType() { ItemAttributeTypeId=3 },
//            AttributeValue = "6"
//        }
//    }
//};

//var newInventoryItem2 = new InventoryItem()
//{
//    Part = new Part() { PartId = 1 },
//    InventoryItemAttributes = new List<InventoryItemAttribute> {
//        new InventoryItemAttribute() {
//            ItemAttributeType = new ItemAttributeType() { ItemAttributeTypeId=1 },
//            AttributeValue = "355"
//        },
//        new InventoryItemAttribute() {
//            ItemAttributeType = new ItemAttributeType() { ItemAttributeTypeId=2 },
//            AttributeValue = "508"
//        },
//        new InventoryItemAttribute() {
//            ItemAttributeType = new ItemAttributeType() { ItemAttributeTypeId=3 },
//            AttributeValue = "6"
//        }
//    }
//};

//var matchingCount1 = currentInventoryItem.InventoryItemAttributes.Where(ci1 => newInventoryItem1.InventoryItemAttributes.Any(ni1 => ni1.InventoryItemAttributeType.ItemAttributeTypeId == ci1.InventoryItemAttributeType.ItemAttributeTypeId));
//var matchingCount2 = currentInventoryItem.InventoryItemAttributes.Where(ci1 => newInventoryItem2.InventoryItemAttributes.Any(ni2 => ni2.InventoryItemAttributeType.ItemAttributeTypeId == ci1.InventoryItemAttributeType.ItemAttributeTypeId));
//var matchingCount3 = currentInventoryItem.InventoryItemAttributes.Where(ci1 => newInventoryItem1.InventoryItemAttributes.Any(ni1 => ni1.InventoryItemAttributeType.ItemAttributeTypeId == ci1.InventoryItemAttributeType.ItemAttributeTypeId && ci1.AttributeValue != ni1.AttributeValue));
//var matchingCount4 = currentInventoryItem.InventoryItemAttributes.Where(ci1 => newInventoryItem2.InventoryItemAttributes.Any(ni2 => ni2.InventoryItemAttributeType.ItemAttributeTypeId == ci1.InventoryItemAttributeType.ItemAttributeTypeId && ci1.AttributeValue != ni2.AttributeValue));

//var matchingCount1 = newInventoryItem1.InventoryItemAttributes.Where(ni1 => currentInventoryItem.InventoryItemAttributes.Any(ci1 => ci1.ItemAttributeType.ItemAttributeTypeId == ni1.ItemAttributeType.ItemAttributeTypeId));
//var matchingCount2 = newInventoryItem2.InventoryItemAttributes.Where(ni2 => currentInventoryItem.InventoryItemAttributes.Any(ci1 => ci1.ItemAttributeType.ItemAttributeTypeId == ni2.ItemAttributeType.ItemAttributeTypeId));
//var matchingCount3 = newInventoryItem1.InventoryItemAttributes.Where(ni1 => currentInventoryItem.InventoryItemAttributes.Any(ci1 => ci1.ItemAttributeType.ItemAttributeTypeId == ni1.ItemAttributeType.ItemAttributeTypeId && ci1.AttributeValue != ni1.AttributeValue));
//var matchingCount4 = newInventoryItem2.InventoryItemAttributes.Where(ni2 => currentInventoryItem.InventoryItemAttributes.Any(ci1 => ci1.ItemAttributeType.ItemAttributeTypeId == ni2.ItemAttributeType.ItemAttributeTypeId && ci1.AttributeValue != ni2.AttributeValue));

//Console.WriteLine("Matching attribute count for NI1: " + matchingCount1.Count());
//Console.WriteLine("Matching attribute count for NI2: " + matchingCount2.Count());
//Console.WriteLine("Matching attribute count for NI3: " + matchingCount3.Count());
//Console.WriteLine("Matching attribute count for NI4: " + matchingCount4.Count());

var inventoryItemData = new InventoryItemData();

using (var inventoryItemLogic = new InventoryItemLogic(inventoryItemData))
{
    int max_int = 311;
    Console.WriteLine("Tha max int is " + max_int);

    //var inventoryAll = inventoryItemLogic.GetInventoryItemsWithAttributesConcatenated();
    //var inventorySummed = inventoryItemLogic.GetInventoryItemsSummed();
    //Console.WriteLine("inventory all count = " + inventoryAll.Count());
    //Console.WriteLine("inventory summed count = " + inventorySummed.Count());
}