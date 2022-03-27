using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp_Omnivus.Models.Entities;

namespace WebApp_Omnivus
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<ProfileImageEntity> ProfileImages { get; set; }
        public virtual DbSet<ProfileEntity> Profiles { get; set; }
        
    }
}
