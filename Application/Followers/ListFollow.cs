using Application.Core;
using Application.Interfaces;
using Application.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers
{
    public class ListFollow
    {
        public class Query : IRequest<Result<List<Profiles.Profile>>>
        {
            public required string Predicate { get; set; }

            public required string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
        {
            private readonly DataContext m_DataContext;
            private readonly IMapper m_Mapper;
            private readonly IUserAccessor m_UserAccessor;

            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                this.m_UserAccessor = userAccessor;
                this.m_DataContext = dataContext;
                this.m_Mapper = mapper;
            }

            public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<Profiles.Profile>();

                switch (request.Predicate)
                {
                    case "followers":
                        profiles = await m_DataContext.UserFollowings.Where(x => x.Target.UserName == request.Username)
                        .Select(u => u.Observer).ProjectTo<Profiles.Profile>(m_Mapper.ConfigurationProvider, new { currentUsername = m_UserAccessor.GetUsername() })
                        .ToListAsync();
                        break;
                    case "following":
                        profiles = await m_DataContext.UserFollowings.Where(x => x.Observer.UserName == request.Username)
                        .Select(u => u.Target).ProjectTo<Profiles.Profile>(m_Mapper.ConfigurationProvider, new { currentUsername = m_UserAccessor.GetUsername() })
                        .ToListAsync();
                        break;
                }

                return Result<List<Profiles.Profile>>.Succes(profiles);
            }
        }
    }
}