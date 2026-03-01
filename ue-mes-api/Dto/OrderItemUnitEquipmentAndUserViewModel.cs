using ue_mes_entities.Authorization;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;

namespace ue_mes_api.Dto
{
    public class OrderItemUnitEquipmentAndUserViewModel
    {
        public OrderItemUnit OrderItemUnit { get; set; }
        public Equipment Equipment { get; set; }    
        public User User { get; set; }  
    }
}
