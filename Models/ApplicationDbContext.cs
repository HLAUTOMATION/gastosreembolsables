using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GASTOS_REEMBOLSABLES_VMICA.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }


        public DbSet<AppUsuario> AppUsuario { get; set; }
    }
}
