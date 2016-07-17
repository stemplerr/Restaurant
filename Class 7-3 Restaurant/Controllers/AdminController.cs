using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Restaurant.Data;
using Class_7_3_Restaurant.Models;
using Microsoft.AspNet.SignalR;
namespace Class_7_3_Restaurant.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult AdminIndex()
        {
            RestaurantRepository repo = new RestaurantRepository(Properties.Settings.Default.ConnString);
            AdminViewModel vm = new AdminViewModel();
            vm.Orders = repo.GetOrders();
            return View(vm);
        }
        public ActionResult Details(int orderId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.statusChanged(new { statusName = Status.Read });

            RestaurantRepository repo = new RestaurantRepository(Properties.Settings.Default.ConnString);
            DetailsViewModel vm = new DetailsViewModel();
            repo.MarkOrderAsRead(orderId);
            vm.OrderId = orderId;
            vm.MenuItems = repo.GetMenuItemsByOrder(orderId);
            return View(vm);
        }
        [HttpPost]
        public void MarkAsDelivered(int orderId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.statusChanged(new { statusName = Status.Delivered });

            RestaurantRepository repo = new RestaurantRepository(Properties.Settings.Default.ConnString);
            repo.MarkOrderAsDelivered(orderId);
        }
    }
}
