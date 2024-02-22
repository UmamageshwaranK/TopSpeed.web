using Microsoft.AspNetCore.Mvc;
using OpenXmlPowerTools;
using TopSpeed.Web.Data;
using TopSpeed.Web.Models;

namespace TopSpeed.Web.Controllers
{
    //which is used to Crud Operation  its like an API
    public class BrandController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public BrandController(ApplicationDbContext dbContext , IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }
        [HttpGet]


        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.Brand.ToList();
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
            string webRootPath = _environment.WebRootPath; // Get the web path using webHost interface

            var file = HttpContext.Request.Form.Files;   // get the url and come with a request that request had a form that forms have whatever files we grab it

            
            if (file.Count > 0)
            {
                string newfilename = Guid.NewGuid().ToString();  //  Its give Globally Unique idenfier for our image .  Guid => Its an Object its convert to string
                  
                var upload = Path.Combine(webRootPath, @"images\brand" );  // Its combine two path

                var extension = Path.GetExtension(file[0].FileName);  // Get Extension

                using(var filestream = new FileStream(Path.Combine(upload,newfilename+extension),FileMode.Create))  
                {
                    file[0].CopyTo(filestream);
                }
                brand.BrandLogo = @"\images\brand\" + newfilename + extension;

            }

                if (ModelState.IsValid)
            {
                _dbContext.Brand.Add(brand);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));   // Its Condititon true Redirect page
                
            }
            return View();
        }
        [HttpGet]
        public IActionResult  Details(int id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.ID == id); // check the database id and Details page ID if its true get the coorect the details
            return View(brand);
        }
    }
}
