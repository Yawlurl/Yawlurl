using System.ComponentModel.DataAnnotations;

namespace YawlUrl.Infrastructure.AspNetCore.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
