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
    public class ReportingCart 
    {
        public static GenericUnitOfWork _UnitOfWork = new GenericUnitOfWork();
        ApplicationDbContext db = new ApplicationDbContext();
         string ReportCartId { get; set; }
         
        public const string CartSessionKey = "ReportCartId";
        public static ReportingCart GetCart(HttpContextBase context)
        {
            var cart = new ReportingCart();
            cart.ReportCartId = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ReportingCart GetCart(ReportCartController controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(StockTbl product)
        {
            var x = _UnitOfWork.GetRepositoryInstance<StockTbl>().GetFirstorDefaultByParameter(a => a.StockID == product.StockID);
            // Get the matching cart and album instances
            var cartItem = db.ReportCarts.SingleOrDefault(
                c => c.VehicleID == ReportCartId
                && c.StockID == x.StockID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new ReportCart
                {
                    StockID = x.StockID,
                    VehicleID = ReportCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.ReportCarts.Add(cartItem);
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
            var cartItem = db.ReportCarts.Single(
                cart => cart.VehicleID == ReportCartId
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
                    db.ReportCarts.Remove(cartItem);
                }
                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = db.ReportCarts.Where(
                cart => cart.VehicleID == ReportCartId);

            foreach (var cartItem in cartItems)
            {
                db.ReportCarts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }
        public List<ReportCart> GetCartItems()
        {
            return db.ReportCarts.Where(
                cart => cart.VehicleID == (ReportCartId)).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.ReportCarts
                          where cartItems.VehicleID == ReportCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in db.ReportCarts
                              where cartItems.VehicleID == ReportCartId
                              select (int?)cartItems.Count *
                              cartItems.StockTbl.Price).Sum();

            return total ?? decimal.Zero;
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
            string cid = context.Session["ReportCartId"].ToString();


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
