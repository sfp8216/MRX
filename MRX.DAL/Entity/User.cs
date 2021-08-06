using Microsoft.AspNetCore.Identity;

namespace MRX.DAL.Entity
{
    public class User:IdentityUser{
        public string CompanyName {get;set;}
    }
}