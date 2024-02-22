using Microsoft.AspNetCore.Mvc;
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
            
            if(ModelState.IsValid)
            {
                _dbContext.Brand.Add(brand);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));   // Its Condititon true Redirect page
                
            }
            return View();
        }
    }
}
