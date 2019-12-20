using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.lib;

namespace WebApplication1.Controllers
{
    public class SearchController : MasterController
    {
        public SearchController(_DbContext context): base(context)
        {

        }

        [Route("Search/{searchString}")]
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            Category cat = new Category { Name = "Kết quả tìm kiếm" };

            if (!string.IsNullOrEmpty(searchString))
            {
               
            }

            var products = _dbContext.Products
                   .Where(p => p.Name.Contains(searchString));

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

            int pageSize = 1;

            ViewBag.category = cat;
            ViewBag.products = await PaginatedList<Product>.CreateAsync(products,pageNumber ?? 1,pageSize);
            //await PaginatedList<Product>.CreateAsync(products, pageNumber ?? 1, pageSize);
            ViewBag.categories = GetMasterData();
            ViewBag.imgs = imgs;
            return View("Views/Shop/index.cshtml");

            //ViewBag.category = cat;
            //ViewBag.products = new List<Product>();
            //ViewBag.categories = GetMasterData();
            //ViewBag.imgs = new Dictionary<int, string>();
            //return View("Views/Shop/index.cshtml");
        }
    }
}