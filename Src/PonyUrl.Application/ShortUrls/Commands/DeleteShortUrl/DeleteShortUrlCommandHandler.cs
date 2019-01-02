 
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using PonyUrl.Domain.Interfaces;
using PonyUrl.Common;

namespace PonyUrl.Application.ShortUrls.Commands.DeleteShortUrl
{
    public class DeleteShortUrlCommandHandler :   IRequestHandler<DeleteShortUrlCommand>
    {
        IShortUrlRepository _shortUrlRepository;

        public DeleteShortUrlCommandHandler(IShortUrlRepository shortUrlRepository)  
        {
            _shortUrlRepository = shortUrlRepository;
        }

        public async Task<Unit> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
        { 
            Validation.ArgumentNotNull(request.Id, "ID cannot be null!");

            await _shortUrlRepository.DeleteAsync(request.Id);
             
            return Unit.Value;
        }
    }
}
