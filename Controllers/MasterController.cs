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
    public class MasterController : Controller
    {
        protected _DbContext _dbContext;
        public MasterController(_DbContext context)
        {
            _dbContext = context;
            //ViewBag.host = String.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
        }

        public IEnumerable<Object> GetMasterData()
        {
            var masterData = _dbContext.Categories.ToList();

            return masterData;
        }


    }
}