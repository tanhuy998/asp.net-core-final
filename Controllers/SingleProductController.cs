using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    public class SingleProductController : MasterController
    {
        public SingleProductController(_DbContext context): base (context)
        {

        }

        [Route("product/{slug}")]
        public IActionResult Index(string slug)
        {
            var masterData = GetMasterData();

            ViewBag.categories = masterData;

            try
            {
                var product = _dbContext.Products
                    .Where(p => p.Slug == slug)
                    .FirstOrDefault();

                var img = _dbContext.Images
                    .Where(img => img.Product == product)
                    .FirstOrDefault();

                ViewBag.img = (img != null) ? img.Path : "";
                ViewBag.product = product;
            }
            catch (Exception ex)
            {
                return Content("404");
            }

            return View();
        }
    }
}