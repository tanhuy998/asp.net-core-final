using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        
        public IActionResult Index()
        {
            //HttpContext.sign
            return View();
        }
    }
}