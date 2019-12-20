using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.lib;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    public class ShopController : MasterController
    {
        //private _DbContext _context;
        public ShopController(_DbContext context):base(context)
        {
            //_context = context;
        }

        [Route("shop/{slug}/{pageNumber?}")]
        public async Task<IActionResult> Index(string slug, int? pageNumber)
        {
         

            var cat = _dbContext.Categories
                .Where<Category>(c => c.Slug == slug)
                .FirstOrDefault();
            
            try
            {
                var products = _dbContext.Products
                    .Where(p => p.Category == cat)
                    .Select(p => p);

                Dictionary<int, string> imgs = new Dictionary<int, string>();

                foreach (var product in products)
                {
                    var img = _dbContext.Images
                        .Where(img => img.Product == product)
                        .FirstOrDefault();

                    if (img != null)
                    {
                        imgs.Add(product.ProductId, img.Path);
                    }
                    else
                    {
                        imgs.Add(product.ProductId, "");
                    }
                }
                //string content = "";

                //foreach (Product product in products)
                //{
                //    content += product.Name + "\n";
                //}
                int pageSize = 2;
                ViewBag.category = cat;
                ViewBag.products = await PaginatedList<Product>.CreateAsync(products, pageNumber ?? 1, pageSize);
                ViewBag.categories = GetMasterData();
                ViewBag.imgs = imgs;

                return View();
            }
            catch (Exception e)
            {
               // var a = _context.Products.Where(p => p.Category == cat);
                return Content(e.StackTrace);
            }
            
        }

        
    }
}