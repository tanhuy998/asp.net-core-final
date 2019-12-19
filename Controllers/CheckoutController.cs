using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    public class CheckoutController : MasterController
    {
        public CheckoutController(_DbContext context): base(context)
        {

        }
        public IActionResult Index()
        {
            var strCart = Request.Cookies["cart"];

            //List<dynamic> list = new List<dynamic>();
            int totalPrice = 0;


            if (strCart != null && strCart != "")
            {
                JToken cart = JToken.Parse(strCart);



                foreach (var item in cart)
                {
                    var product = _dbContext.Products
                        .Find(int.Parse(item["id"].ToString()));

                    if (product != null)
                    {
                        dynamic obj = new ExpandoObject();

                        obj.Product = product;
                        obj.Quantity = item["qty"].ToString();
                        obj.Total = product.Price * int.Parse(obj.Quantity);

                        totalPrice += product.Price * int.Parse(obj.Quantity);

                        //list.Add(obj);
                    }
                }

                var masterData = GetMasterData();
                ViewBag.categories = masterData;
                //ViewBag.cart = list;
                ViewBag.total = totalPrice;

                return View();
            }
            else
            {
                string referer = Request.Headers["Referer"].ToString();

                if (referer != null)
                {
                    return Redirect(referer);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            //cart.GetType();
            //cart[0].
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([Bind("CustomerName,ShipAddress,ContacNumber,Note")] Order order)
        {
            

            var strCart = Request.Cookies["cart"];

            if (strCart != null && strCart != "")
            {
                JToken cart = JToken.Parse(strCart);

                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        string payment_method = Request.Form["payment_method"];
                        
                        var method = _dbContext.PaymentMethods
                            .Where(m => m.Name == payment_method)
                            .FirstOrDefault();

                        var payment = new Payment();
                        payment.PaymentMethodId = method.PaymentMethodId;
                        _dbContext.Payments.Add(payment);

                        Order ord = new Order();

                        ord.CustomerName = Request.Form["CustomerName"];
                        ord.ContactNumber = Request.Form["ContactNumber"];
                        ord.Note = Request.Form["Note"];
                        ord.ShipAdress = Request.Form["ShipAddress"];

                        ord.Payment = payment;
                        ord.Time = DateTime.Now;
                        _dbContext.Orders.Add(ord);

                        foreach (var item in cart)
                        {
                            var product = _dbContext.Products
                                .Find(int.Parse(item["id"].ToString()));

                            if (product != null)
                            {
                                var ord_product = new OrderProduct();

                                ord_product.Product = product;
                                ord_product.Quantity = int.Parse(item["qty"].ToString());
                                ord_product.Order = ord;

                                _dbContext.OrderProducts.Add(ord_product);
                            }
                        }

                        _dbContext.SaveChanges();

                        transaction.Commit();
                        Response.Cookies.Append("cart", "", new Microsoft.AspNetCore.Http.CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(-1)
                        });
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.StackTrace);
                    }

                }
            }

            //string script = @"<script>" +
            //    @"alert('Cảm ơn quý khác đã mua hàng của tại eFashion'); window.location = '/';" +
            //    @"</script>";
            return RedirectToAction("Index", "Home");
        }
    }
}