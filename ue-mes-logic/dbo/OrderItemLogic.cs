using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.dbo;
using ue_mes_data.dbo.Interface;
using ue_mes_data.Global.Interface;
using ue_mes_entities.dbo;
using ue_mes_entities.Enums;

namespace ue_mes_logic.dbo
{
    public class OrderItemLogic : LogicBase
    {
        IBillOfMaterialData BillOfMaterialData { get; }
        IEquipmentData EquipmentData { get; }
        IOrderItemData OrderItemData { get; }
        IPartData PartData { get; }
        IPartItemAttributeTypeData PartItemAttributeTypeData { get; }

        #region Constructor

        public OrderItemLogic(IBillOfMaterialData billOfMaterialData,
            IEquipmentData equipmentData,
            IOrderItemData orderItemData,
            IPartData partData,
            IPartItemAttributeTypeData partItemAttributeTypeData)
        {
            BillOfMaterialData = billOfMaterialData ?? throw new ArgumentNullException(nameof(billOfMaterialData));
            EquipmentData = equipmentData ?? throw new ArgumentNullException(nameof(equipmentData));
            OrderItemData = orderItemData ?? throw new ArgumentNullException(nameof(orderItemData));
            PartData = partData ?? throw new ArgumentNullException(nameof(partData));
            PartItemAttributeTypeData = partItemAttributeTypeData ?? throw new ArgumentNullException(nameof(partItemAttributeTypeData));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all active order items (order is released or started) that match the part.
        /// Note, an OrderItem is tied to the finished good part only, so if the part object is a sub assembly part, then we need to traverse up to find its finished good consumer.
        /// The logic will handle this and return a full list of OrderItems
        /// </summary>
        /// <param name="part">Part to search for order items</param>
        /// <returns></returns>
        public List<OrderItem> GetActiveOrderItemsByPartId(Part part)
        {
            using (var partLogic = new PartLogic(BillOfMaterialData, EquipmentData, PartData, PartItemAttributeTypeData))
            {
                List<Part> partList = new List<Part>() { part };

                if (part != null && part.PartType == PartTypeEnum.SubAssembly)
                    partList = partLogic.GetConsumingFinishedGoodPartsForSubAssemblyPart(part);

                var partIds = partList.Select(partListItem => partListItem.PartId).ToList();

                return OrderItemData.GetActiveOrderItemsForParts(partIds);
            }
        }

        #endregion
    }
}
