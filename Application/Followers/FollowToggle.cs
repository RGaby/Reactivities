using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain.obj;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Followers
{
    public class FollowToggle
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string TargetUsername { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext m_DataContext;
            private readonly IUserAccessor m_UserAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                this.m_UserAccessor = userAccessor;
                this.m_DataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await m_DataContext.Users.FirstOrDefaultAsync(x => x.UserName == m_UserAccessor.GetUsername());

                if (observer == null) return null;

                var target = await m_DataContext.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);

                if (target == null) return null;

                var following = await m_DataContext.UserFollowings.FindAsync(observer.Id, target.Id);

                if (following == null)
                {
                    following = new UserFollowing
                    {
                        Observer = observer,
                        Target = target,
                    };
                    m_DataContext.UserFollowings.Add(following);
                }
                else
                {
                    m_DataContext.UserFollowings.Remove(following);
                }


                var success = await m_DataContext.SaveChangesAsync() > 0;
                if (success) return Result<Unit>.Succes(Unit.Value);

                return Result<Unit>.Failure("Failed to update following");
            }
        }
    }
}