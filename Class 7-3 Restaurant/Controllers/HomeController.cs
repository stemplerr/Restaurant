using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Class_7_3_Restaurant.Models;
using Restaurant.Data;
using Newtonsoft.Json;

namespace Class_7_3_Restaurant.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Categories()
        {
            RestaurantRepository repo = new RestaurantRepository(Properties.Settings.Default.ConnString);
            CategoryViewModel vm = new CategoryViewModel();
            vm.Categories = repo.GetCategories();
            return View(vm);
        }
        public ActionResult Index(int categoryId)
        {
            RestaurantRepository repo = new RestaurantRepository(Properties.Settings.Default.ConnString);
            HomeViewModel vm = new HomeViewModel();
            vm.MenuItems = repo.GetMenuItems(categoryId);
            return View(vm);
        }

        public ActionResult AddToCart(int itemId)
        {
            RestaurantRepository repo = new RestaurantRepository(Properties.Settings.Default.ConnString);
            if (Session["cartId"] == null)
            {
                Session["cartId"] = repo.GetNewShoppingCart();
            }
            int cartId = Convert.ToInt32(Session["cartId"]);
            repo.AddToCart(itemId, cartId);
            return Json(cartId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShoppingCart()
        {
            ShoppingCartViewModel vm = new ShoppingCartViewModel();
            RestaurantRepository repo = new RestaurantRepository(Properties.Settings.Default.ConnString);
            if (Session["cartId"] == null)
            {
                return null;
            }
            int cartId = Convert.ToInt32(Session["cartId"]);
            IEnumerable<MenuItemWithQuantity> items = repo.GetShoppingCartMenuItems(cartId);
            vm.ItemsWithQuantity = items;
            return View(vm);
        }

        //public void DeleteFromOrder(int itemId)
        //{
        //    RestaurantRepository repo = new RestaurantRepository(Properties.Settings.Default.ConnString);
        //    int cartId = Convert.ToInt32(Session["cartId"]);
        //    repo.DeleteFromOrder(cartId, itemId);
        //}
        [HttpPost]
        public ActionResult OrderPlaced()
        {
            int cartId = Convert.ToInt32(Session["cartId"]);
            OrderPlacedViewModel vm = new OrderPlacedViewModel();
            RestaurantRepository repo = new RestaurantRepository(Properties.Settings.Default.ConnString);
            vm.OrderId = repo.PlaceOrder(cartId);
            vm.Status = Status.Placed;
            return View(vm);
        }
    }
}
