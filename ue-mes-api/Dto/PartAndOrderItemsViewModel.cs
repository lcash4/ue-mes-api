using System.Collections.Generic;
using ue_mes_entities.dbo;

namespace ue_mes_api.Dto
{
    public class PartAndOrderItemsViewModel
    {
        public Part PartToAssemble { get; set; }
        public List<OrderItem> ActiveOrderItems { get; set; }
    }
}
