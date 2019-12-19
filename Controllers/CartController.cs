using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WebApplication1.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    public class CartController : MasterController
    {
        public CartController(_DbContext context): base(context)
        {

        }

        public IActionResult Index()
        {
            //string strCart = Request.Cookies.ToList();

            var strCart = Request.Cookies["cart"];

            List<dynamic> list = new List<dynamic>();
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

                        list.Add(obj);
                    }
                }
            }

            
            //cart.GetType();
            //cart[0].
            var masterData = GetMasterData();
            ViewBag.categories = masterData;
            ViewBag.cart = list;
            ViewBag.total = totalPrice;
 
            return View();
        }
    }
}