using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext m_DataContext;
            private IUserAccessor m_UserAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                this.m_UserAccessor = userAccessor;
                this.m_DataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await m_DataContext.Activities.Include(a => a.Attendees).ThenInclude(u => u.AppUser).FirstOrDefaultAsync(x => x.Id == request.Id);

                if (activity == null)
                {
                    return null;
                }

                var user = await m_DataContext.Users.FirstOrDefaultAsync(x => x.UserName == m_UserAccessor.GetUsername());

                if (user == null) return null;

                var hostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

                var attendance = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                if (attendance != null && hostUsername == user.UserName)
                {
                    activity.IsCancelled = !activity.IsCancelled;
                }

                if (attendance != null && hostUsername != user.UserName)
                {
                    activity.Attendees.Remove(attendance);
                }

                if (attendance == null)
                {
                    attendance = new Domain.ActivityAttendee
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };
                    activity.Attendees.Add(attendance);
                }

                var result = await m_DataContext.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Succes(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");
            }
        }
    }
}