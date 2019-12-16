using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ShopController : MasterController
    {
        private _DbContext _context;
        public ShopController(_DbContext context):base(context)
        {
            _context = context;
        }

        [Route("shop/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
         

            var cat = _context.Categories
                .Where<Category>(c => c.Slug == slug)
                .FirstOrDefault();
            
            try
            {
                var products = _context.Products.Where(p => p.Category == cat);

                ViewBag.category = cat;
                ViewBag.products = products;
                ViewBag.categories = GetMasterData();
                //string content = "";

                //foreach (Product product in products)
                //{
                //    content += product.Name + "\n";
                //}

                return View();
            }
            catch (Exception e)
            {
                var a = _context.Products.Where(p => p.Category == cat);
                return Content(a.First().Name);
            }
            
        }

        
    }
}