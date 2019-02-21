using System.ComponentModel.DataAnnotations;

namespace PonyUrl.Infrastructure.AspNetCore.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
