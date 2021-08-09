using System.ComponentModel.DataAnnotations;

namespace MRX.API.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }


    }
}