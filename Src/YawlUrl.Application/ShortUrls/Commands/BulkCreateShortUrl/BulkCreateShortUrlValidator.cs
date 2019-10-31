using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using YawlUrl.Common;

namespace YawlUrl.Application.ShortUrls.Commands.BulkCreateShortUrl
{
    public class BulkCreateShortUrlValidator : AbstractValidator<BulkCreateShortUrlCommand>
    {
        public BulkCreateShortUrlValidator()
        {
            RuleFor(x => x.LongUrls).NotEmpty().Must(m => m.All(s => Check.IsValidUrl(s))).WithMessage("LongUrls must be a uri");
        }
    }
}
