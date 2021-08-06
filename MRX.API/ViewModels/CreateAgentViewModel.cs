using System.ComponentModel.DataAnnotations;

namespace MRX.API.ViewModels
{
    public class CreateAgentViewModel
    {
        [Required]
        public string Username {get;set;}
        [Required]
        public string Password{get;set;}
        [Required]
        public string Email{get;set;}
    }
}