using System.ComponentModel.DataAnnotations;

namespace YawlUrl.Web.Router.Models
{
    public class GenerateUrlModel
    {
        [Required]
        public string LongUrl { get; set; }
    }
}
