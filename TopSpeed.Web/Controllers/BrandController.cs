using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using OpenXmlPowerTools;
using System.Security.AccessControl;
using TopSpeed.Web.Data;
using TopSpeed.Web.Models;

namespace TopSpeed.Web.Controllers
{
    //which is used to Crud Operation  its like an API
    public class BrandController : Controller
    {

        private readonly ApplicationDbContext _dbContext;  // its am object of Application dbContext
        private readonly IWebHostEnvironment _environment;  // Its a library of EntityFrameWorkCore 

        public BrandController(ApplicationDbContext dbContext , IWebHostEnvironment environment)
        {
            _dbContext = dbContext;    // Its come from topspeed.Web
            _environment = environment;      //  its an  Webhost
        }
        [HttpGet]


        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.Brand.ToList();       // its a come  from brand model class
             return View(brands);         // its return get details of the brand products  
        }
        [HttpGet]
        public IActionResult Create()
        {
                return View();
        }
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            string webRootPath = _environment.WebRootPath; // Get the web path using webHost interface  get the adress 

            var file = HttpContext.Request.Form.Files;   // get the url and come with a request that request had a form that forms have whatever files we grab it

            
            if (file.Count > 0)
            {
                string newfilename = Guid.NewGuid().ToString();  //  Its give Globally Unique idenfier for our image .  Guid => Its an Object its convert to string
                  
                var upload = Path.Combine(webRootPath, @"images\brand" );  // Its combine two path

                var extension = Path.GetExtension(file[0].FileName);  // Get Extension

                using(var filestream = new FileStream(Path.Combine(upload,newfilename+extension),FileMode.Create))  
                {
                    file[0].CopyTo(filestream);   // Copy the httpContext.request.forms.files to store the filestream 
                }
                brand.BrandLogo = @"\images\brand\" + newfilename + extension;   //Its store the model column

            }

                if (ModelState.IsValid)      //  if its true   if any form in model table its true
            {
                _dbContext.Brand.Add(brand);   // its going in dbContext   and then add the  Brand tables
                _dbContext.SaveChanges();       // and then  its savethe databases

                TempData["success"] = "Recorded Created SuccessFully";   // Its  an Temdata    TempData is a temprory storage data its a key and pair   its like a variable

                return RedirectToAction(nameof(Index));   // Its Condititon true Redirect page  to index
                
            }
            return View();
        }
        [HttpGet]
        public IActionResult  Details(int id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.ID == id); // check the database id and Details page ID if its true get the coorect the details
            return View(brand);
        }
        [HttpGet]  // show the Edit page
        public IActionResult Edit(int id)  // First view show  the page
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.ID == id); // check the database id and Details page ID if its true get the coorect the details
            return View(brand);
        }

        [HttpPost]   // Edit the page
        public IActionResult Edit(Brand brand)  //    SecondView write logic here
        {
            string webRootPath = _environment.WebRootPath; // Get the web path using webHost interface

            var file = HttpContext.Request.Form.Files;   // get the url and come with a request that request had a form that forms have whatever files we grab it


            if (file.Count > 0) 
            {
                string newfilename = Guid.NewGuid().ToString();  //  Its give Globally Unique idenfier for our image .  Guid => Its an Object its convert to string

                var upload = Path.Combine(webRootPath, @"images\brand");  // Its combine two path

                var extension = Path.GetExtension(file[0].FileName);  // Get Extension


                // Delete Old Image

                var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.ID == brand.ID);
                if(objFromDb.BrandLogo != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));     // Remove  \\ form the database  path of image

                    if (System.IO.File.Exists(oldImagePath))   // If any iamge my path 
                    {
                        System.IO.File.Delete(oldImagePath);   // Deleted
                    }

               
                }

                using (var filestream = new FileStream(Path.Combine(upload, newfilename + extension), FileMode.Create))
                {
                    file[0].CopyTo(filestream);
                }
                brand.BrandLogo = @"\images\brand\" + newfilename + extension;

            }
           

            if (ModelState.IsValid)
            {
                var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.ID == brand.ID);

                objFromDb.Name = brand.Name;
                objFromDb.Established = brand.Established;
                if(brand.BrandLogo != null)
                {
                    objFromDb.BrandLogo = brand .BrandLogo;
                }

                _dbContext.Brand.Update(objFromDb);      // Its an inbuild method its give an entity framework core given the method
                _dbContext.SaveChanges();  // Its Save Changes
                TempData["warning"] = "Updated Successfully";
                return RedirectToAction(nameof(Index));   // Its an redirect page  to index page
            }
            return View();   

        }
        [HttpDelete]

        [HttpGet]  // show the Edit page
        public IActionResult Delete(int id)  // First view show  the page
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.ID == id); // check the database id and Details page ID if its true get the coorect the details
            return View(brand);
        }
        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            string webRootPath = _environment.WebRootPath; // Get the web path using webHost interface

            if(!string.IsNullOrEmpty(webRootPath))
            {
                // Delete Old Image

                var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.ID == brand.ID);
                if (objFromDb.BrandLogo != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));     // Remove  \\ form the database  path of image

                    if (System.IO.File.Exists(oldImagePath))   // If any iamge my path 
                    {
                        System.IO.File.Delete(oldImagePath);   // Deleted
                    }

                }
            }
            _dbContext.Brand.Remove(brand);
            _dbContext.SaveChanges();
            TempData["error"] = "Deleted successfully";
            return RedirectToAction(nameof(Index));      // This return way
        }
    }
}
