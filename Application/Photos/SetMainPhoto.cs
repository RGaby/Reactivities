using System.Linq.Expressions;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class SetMainPhoto
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private DataContext m_Context;
            private IUserAccessor m_UserAccesor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                m_Context = context;
                m_UserAccesor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await m_Context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == m_UserAccesor.GetUsername());

                if (user == null) return null;

                var newMainPhoto = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                if (newMainPhoto == null) return Result<Unit>.Failure("Photo was not found");

                var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
                if (currentMain != null)
                {
                    currentMain.IsMain = false;
                }

                newMainPhoto.IsMain = true;

                var success = await m_Context.SaveChangesAsync() > 0;
                if (success)
                {
                    return Result<Unit>.Succes(Unit.Value);
                }

                return Result<Unit>.Failure("Failure to set the main photo");
            }
        }
    }
}