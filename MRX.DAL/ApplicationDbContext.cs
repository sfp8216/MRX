using Microsoft.EntityFrameworkCore;
using MRX.DAL.Entity;
namespace MRX.DAL
{
    public class ApplicationDbContext:DbContext
    {
          public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)

    {
         
    }
    public DbSet<Value> Values{get;set;} 
    }
}