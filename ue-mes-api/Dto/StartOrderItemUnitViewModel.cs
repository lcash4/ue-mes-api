using ue_mes_entities.Authorization;
using ue_mes_entities.dbo;
using ue_mes_entities.Global;

namespace ue_mes_api.Dto
{
    public class StartOrderItemUnitViewModel
    {
        public OrderItem OrderItem { get; set; }
        public Part Part { get; set; }
        public Equipment Equipment { get; set; }
        public User User { get; set; }

    }
}
