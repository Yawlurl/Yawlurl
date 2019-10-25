using FluentValidation;
using PonyUrl.Common;

namespace PonyUrl.Application.ShortUrls.Commands
{
    public class CreateShortUrlValidator : AbstractValidator<CreateShortUrlCommand>
    {
        public CreateShortUrlValidator()
        {
            RuleFor(x => x.LongUrl).NotEmpty().Must(m => Check.IsValidUrl(m)).WithMessage("LongUrl must be a uri");
        }
    }
}
