using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using ue_mes_entities.dbo;

namespace ue_mes_api.SignalR
{
    public class OrderItemUnitHub : Hub
    {
        public OrderItemUnitHub() { }

        /// <summary>
        /// Connections to the OrderItemUnitHub will be by equipment name only.  The idea is that a client can be opened at an equipment and see changes as they happen.
        /// If the client changes to a new equipment, the hub will be reconnected to the new equipment.
        /// There is no use case to see serial number level changes, so that will not be used for now.
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var equipmentFromQueryString = httpContext.Request.Query["equipmentName"];
            await Groups.AddToGroupAsync(Context.ConnectionId, equipmentFromQueryString);
            await Clients.Caller.SendAsync("LoadRunScreen", equipmentFromQueryString);
        }

        /// <summary>
        /// This hub call is triggered when one client moves assembly to a downstream equipment.
        /// We want the client to move ahead to the next equipment, but all other connected clients should simply refresh with its current equipment.
        /// The client logic is expected to handle the refresh and just needs to know that a move has happened.
        /// </summary>
        /// <param name="currentEquipmentName"></param>
        /// <returns></returns>
        public async Task SendOrderItemUnitMovedToNextEquipment(string currentEquipmentName)
        {
            await Clients.OthersInGroup(currentEquipmentName).SendAsync("ReceiveOrderItemUnitMovedToNextEquipment", currentEquipmentName);
        }
    }
}
