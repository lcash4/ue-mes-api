using ue_mes_entities.dbo;

namespace ue_mes_api.Dto
{
    public class MoveInventoryItem
    {
        public InventoryItem InventoryItem { get; set; }
        
        public InventoryLocation DestinationLocation { get; set; }

        public decimal QuantityToMove { get; set; }
    }
}
