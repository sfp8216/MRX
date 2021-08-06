using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MRX.DAL.Entity;
namespace MRX.DAL
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {
          public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)

    {
         
    }
    public DbSet<Value> Values{get;set;} 
    }
}