using APPDEVInc2.Controllers;
using APPDEVInc2.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Web.Mvc;
using System.Data.Entity;
using APPDEVInc2.Repository;
namespace APPDEVInc2.Models
{
    public class ShoppingCart
    {
        public static GenericUnitOfWork _UnitOfWork = new GenericUnitOfWork();
        ApplicationDbContext db = new ApplicationDbContext();
        string ShoppingCartid { get; set; }
        public const string CartSessionKey = "CartId";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartid = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(ShoppingCartController controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(TyresTbl product)
        {
            var x = _UnitOfWork.GetRepositoryInstance<StockTbl>().GetFirstorDefaultByParameter(a => a.TyreID == product.TyreID);
            // Get the matching cart and album instances
            var cartItem = db.Carts.SingleOrDefault(
                c => c.CartID == ShoppingCartid
                && c.StockID == x.StockID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    StockID = x.StockID,
                    CartID = ShoppingCartid,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            db.SaveChanges();
        }

       
        
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = db.Carts.Single(
                cart => cart.CartID == ShoppingCartid
                && cart.StockID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }
                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(
                cart => cart.CartID == ShoppingCartid);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(
                cart => cart.CartID == ShoppingCartid).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.Carts
                          where cartItems.CartID == ShoppingCartid
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartID == ShoppingCartid
                              select (int?)cartItems.Count *
                              cartItems.StockTbl.Price).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(OrdersTbl order , HttpContextBase context)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetailsTbl
                {
                    StockID = item.StockID,
                    OrderID = order.OrderID,

                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.StockTbl.Price);

                db.OrderDetailsTbls.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.TotalPrice = orderTotal;
            //HttpContext cont = new HttpContext(HttpContext.Current);
            order.UserID = context.User.Identity.GetUserId();
            order.Status = "Pending Payment";
            // Save the order
            db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderID;
        } 
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            
            return context.Session[CartSessionKey].ToString();
        }

      
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            HttpContext context = HttpContext.Current;
            var x = new ShoppingCart();
            var car = new ShoppingCart();
            string cid = context.Session["CartId"].ToString();
            

            var cart1 = _UnitOfWork.GetRepositoryInstance<Cart>().GetAllRecords();
            var last = cart1.Last();
            

            using (var dbs = new ApplicationDbContext())
            {
                var shoppingCart = dbs.Carts.Where(
                       c => c.CartID == cid);

                foreach (var item in shoppingCart)
                {
                    item.CartID = userName;
                }
                dbs.SaveChanges();

            }
               
        } 

    }
}
