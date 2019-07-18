using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class DeleteShortUrlCommand : IRequest 
    {
        [Required]
        public string ShortKey { get; set; }
    }
}
