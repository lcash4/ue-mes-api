using System.Collections.Generic;
using ue_mes_entities.dbo;

namespace ue_mes_api.Dto
{
    public class InventoryLocationWithQuantityViewModel
    {
        public InventoryLocation InventoryLocation { get; set; }
        public decimal InventoryItemQuantity { get; set; }
    }
}
