using System.ComponentModel.DataAnnotations;

namespace PonyUrl.Infrastructure.MongoDb.Identity.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
