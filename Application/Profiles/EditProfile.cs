using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class EditProfile
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string DisplayName { get; set; }
            public string Bio { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.DisplayName).NotEmpty().NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private DataContext m_DataContext;
            private IUserAccessor m_UserAccesor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                m_DataContext = dataContext;
                m_UserAccesor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await m_DataContext.Users.FirstOrDefaultAsync(x => x.UserName == m_UserAccesor.GetUsername());

                if (user == null) return null;

                if (user.DisplayName == request.DisplayName && user.Bio == request.Bio)
                {
                    return Result<Unit>.Succes(Unit.Value);
                }

                if (user.DisplayName != request.DisplayName)
                {
                    user.DisplayName = request.DisplayName;
                }

                user.Bio = request.Bio ?? user.Bio;

                var success = await m_DataContext.SaveChangesAsync() > 0;

                if (success)
                {
                    return Result<Unit>.Succes(Unit.Value);
                }

                return Result<Unit>.Failure("Problem editing the profile");
            }
        }
    }
}