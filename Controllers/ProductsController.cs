using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models;
using System.IO;
using WebApplication1.lib;

namespace WebApplication1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly _DbContext _context;

        public ProductsController(_DbContext context)
        {
            _context = context;
        }

        // GET: Products
        [Route("Products/{pageNumber?}")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            //ViewData["CurrentSort"] = sortOrder;
            //ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            //if (searchString != null)
            //{
            //    pageNumber = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            //ViewData["CurrentFilter"] = searchString;

            var students = from s in _context.Products
                           select s;
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    students = students.Where(s => s.LastName.Contains(searchString)
            //                           || s.FirstMidName.Contains(searchString));
            //}
            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        students = students.OrderByDescending(s => s.LastName);
            //        break;
            //    case "Date":
            //        students = students.OrderBy(s => s.EnrollmentDate);
            //        break;
            //    case "date_desc":
            //        students = students.OrderByDescending(s => s.EnrollmentDate);
            //        break;
            //    default:
            //        students = students.OrderBy(s => s.LastName);
            //        break;
            //}

            int pageSize = 8;
            return View(await PaginatedList<Product>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,Description,Active,Slug,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);

                var imgFile = Request.Form.Files["image"];
                    
                if(imgFile != null || imgFile.Length > 0)
                {
                    Image img = new Image();
                    img.Path = imgFile.FileName;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", imgFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(stream);
                    }


                    img.Product = product;
                    _context.Images.Add(img);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,Description,Active,Slug,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);

                    var imgFile = Request.Form.Files["image"];

                    if (imgFile != null || imgFile.Length > 0)
                    {
                        Image img = await _context.Images.Where(i => i.Product == product).FirstOrDefaultAsync();

                        string oldFile_Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", img.Path);
                        System.IO.File.Delete(oldFile_Path);

                        img.Path = imgFile.FileName;
                        
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", imgFile.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imgFile.CopyToAsync(stream);
                        }

                        _context.Images.Update(img);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
