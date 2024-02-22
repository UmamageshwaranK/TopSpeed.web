using Microsoft.EntityFrameworkCore;

namespace TopSpeed.Web.Data
{
    public class ApplicationDbContext : DbContext   // Its using Entity FrameWork Core Library
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
            {


            }

    }
}
