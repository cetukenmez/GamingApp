using GamingApp.Entity.Context;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingApp.Hubs
{
    public class MyHub : Hub
    {
        GamingAppContext db = new GamingAppContext();



        public MyHub()
        {

        }


        public async Task GetMasaOrderCountHub()
        {
            await Clients.All.SendAsync("ReceiveMasaOrderCount", 1);
            await Clients.All.SendAsync("ReceiveMasaOrderCounts", 1);

        }

        private static int ClientCount { get; set; } = 0;
        public async override Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ClientCount--;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);

            await base.OnDisconnectedAsync(exception);
        }
    }
}