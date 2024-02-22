using Microsoft.EntityFrameworkCore;
using TopSpeed.Web.Models;

namespace TopSpeed.Web.Data
{
    public class ApplicationDbContext : DbContext   // Its using Entity FrameWork Core Library
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
            {


            }
        public DbSet<Brand> Brand { get; set; }  // Its a Table name of the data base

    }
}
