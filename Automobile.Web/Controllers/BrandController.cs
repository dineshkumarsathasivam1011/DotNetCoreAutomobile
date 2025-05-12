using Automobile.Web.Data;
using Automobile.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automobile.Web.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.BrandAutomobile.ToList();

            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            //Image Upload
            string webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;
            if(file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\brands");
                var extension = Path.GetExtension(file[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.BrandLogo = @"\images\brands\" + newFileName + extension;
            }


            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                _dbContext.BrandAutomobile.Add(brand);
                _dbContext.SaveChanges();
                TempData["success"] = "Brand created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
            
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _dbContext.BrandAutomobile.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Brand brand = _dbContext.BrandAutomobile.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpPost]
    public IActionResult Delete(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            if(!string.IsNullOrEmpty(brand.BrandLogo))
            {
                var objFileDb = _dbContext.BrandAutomobile.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                if (objFileDb.BrandLogo != null)
                {
                    var oldImagepath = Path.Combine(webRootPath, objFileDb.BrandLogo.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagepath))
                    {
                        // Delete the old file
                        System.IO.File.Delete(oldImagepath);
                    }

                }

            }
            _dbContext.BrandAutomobile.Remove(brand);
            _dbContext.SaveChanges();
            TempData["error"] = "Brand deleted successfully";
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            Brand brand = _dbContext.BrandAutomobile.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\brands");
                var extension = Path.GetExtension(file[0].FileName);

                // Delete the old file if it exists
                var objFileDb = _dbContext.BrandAutomobile.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                if (objFileDb.BrandLogo != null)
                {
                    var oldImagepath = Path.Combine(webRootPath, objFileDb.BrandLogo.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagepath))
                    {
                        // Delete the old file
                        System.IO.File.Delete(oldImagepath);
                    }

                }
                using (var fileStream = new FileStream(Path.Combine(uploads, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.BrandLogo = @"\images\brands\" + newFileName + extension;
            }
            if (ModelState.IsValid)
            {
                var objFileDb = _dbContext.BrandAutomobile.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                objFileDb.Name = brand.Name;
                objFileDb.EstablishedYear = brand.EstablishedYear;
                if (brand.BrandLogo != null)
                {
                    objFileDb.BrandLogo = brand.BrandLogo;
                }
                _dbContext.BrandAutomobile.Update(objFileDb);
                _dbContext.SaveChanges();
                TempData["warning"] = "Brand updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
