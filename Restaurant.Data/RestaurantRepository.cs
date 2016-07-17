using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data
{
    public class RestaurantRepository
    {
        private string _connString;
        public RestaurantRepository(string connString)
        {
            _connString = connString;
        }
        //---------------------- Client Side ---------------------------------------
        public IEnumerable<Category> GetCategories()
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                return context.Categories.ToList();
            }
        }
        public IEnumerable<MenuItem> GetMenuItems(int categoryId)
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                if (categoryId == 0)
                {
                    return context.MenuItems.ToList();
                }
                return context.MenuItems.Where(i => i.CategoryId == categoryId).ToList();
            }
        }


        public int GetNewShoppingCart()
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                ShoppingCart sc = new ShoppingCart();
                context.ShoppingCarts.InsertOnSubmit(sc);
                context.SubmitChanges();
                return sc.Id;
            }
        }
        public void AddToCart(int menuItemId, int shoppingCartId)
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                ShoppingCartItem existing = new ShoppingCartItem();
                if (context.ShoppingCartItems.Count() > 0)
                {
                    existing = context.ShoppingCartItems.FirstOrDefault
                             (i => i.ShoppingCartId == shoppingCartId && i.MenuItemId == menuItemId);
                }
                if (existing != null)
                {
                    existing.Quantity++;
                }
                else
                {
                    context.ShoppingCartItems.InsertOnSubmit(new ShoppingCartItem
                    {
                        MenuItemId = menuItemId,
                        ShoppingCartId = shoppingCartId,
                        Quantity = 1
                    });
                }
                context.SubmitChanges();
            }
        }

        public IEnumerable<MenuItemWithQuantity> GetShoppingCartMenuItems(int cartId)
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<MenuItem>(i => i.Category);
                context.LoadOptions = loadOptions;
                IEnumerable<ShoppingCartItem> cartItems =
                        context.ShoppingCartItems.Where(item => item.ShoppingCartId == cartId).ToList();
                List<MenuItemWithQuantity> items = new List<MenuItemWithQuantity>();
                foreach (ShoppingCartItem item in cartItems)
                {
                    items.Add(new MenuItemWithQuantity
                        {
                            MenuItem = context.MenuItems.First(i => i.Id == item.MenuItemId),
                            Quantity = item.Quantity
                        });
                }
                return items;
            }
        }
        public void DeleteFromOrder(int cartId, int itemId)
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                context.ShoppingCartItems.DeleteOnSubmit(
                    context.ShoppingCartItems.First(i => i.MenuItemId == itemId && i.ShoppingCartId == cartId));
                context.SubmitChanges();
            }
        }
        public int PlaceOrder(int cartId)
        {
            Order o = new Order();
            IEnumerable<MenuItemWithQuantity> items = GetShoppingCartMenuItems(cartId);
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                context.Orders.InsertOnSubmit(o);
                context.SubmitChanges();
                IEnumerable<OrderDetail> details = items.Select(item => new OrderDetail
                                  {
                                      OrderId = o.Id,
                                      MenuItemId = item.MenuItem.Id,
                                      Quantity = item.Quantity
                                  });

                context.OrderDetails.InsertAllOnSubmit(details);
                context.SubmitChanges();
            }
            return o.Id;
        }
        //------------------------- Admin Side --------------------------------
        public IEnumerable<Order> GetOrders()
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                return context.Orders.ToList();
            }
        }
        public void MarkOrderAsRead(int orderId)
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                Order o = context.Orders.First(or => or.Id == orderId);
                o.Status = (int)Status.Read;
                context.SubmitChanges();
            }
        }
        IEnumerable<OrderDetail> GetOrderDetails(int orderId)
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                return context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
            }
        }

        public IEnumerable<MenuItemWithQuantity> GetMenuItemsByOrder(int orderId)
        {
            var orderDetails = GetOrderDetails(orderId);
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                List<MenuItemWithQuantity> menuitems = new List<MenuItemWithQuantity>();
                foreach (OrderDetail od in orderDetails)
                {
                    MenuItemWithQuantity mi = new MenuItemWithQuantity();
                    mi.MenuItem = new MenuItem();
                    mi.MenuItem.Id = od.MenuItemId;
                    mi.MenuItem.Name = context.MenuItems.First(item => item.Id == od.MenuItemId).Name;
                    mi.Quantity = od.Quantity;
                    menuitems.Add(mi);
                }
                return menuitems;
            }
        }
        public void MarkOrderAsDelivered(int orderId)
        {
            using (RestaurantDataContext context = new RestaurantDataContext())
            {
                Order o = context.Orders.First(or => or.Id == orderId);
                o.Status = (int)Status.Delivered;
                context.SubmitChanges();
            }
        }
    }
}
