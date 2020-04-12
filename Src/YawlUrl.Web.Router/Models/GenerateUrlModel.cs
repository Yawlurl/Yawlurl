using System.ComponentModel.DataAnnotations;
using YawlUrl.Application.ShortUrls.Commands;

namespace YawlUrl.Web.Router.Models
{
    public class GenerateUrlModel
    {
        [Required]
        public string LongUrl { get; set; }


        public CreateShortUrlCommand ToCommandModel()
        {
            return new CreateShortUrlCommand
            {
                IsRouter = true,
                LongUrl = LongUrl,
                SlugKey = string.Empty
            };
        }
    }
}
