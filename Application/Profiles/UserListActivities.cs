using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class UserListActivities
    {
        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            public string Username { get; set; }
            public string Predicate { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
        {
            private readonly DataContext m_DataContext;
            private readonly IMapper m_Mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                m_DataContext = dataContext;
                m_Mapper = mapper;
            }

            public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = m_DataContext.ActivityAttendees
                .Where(u => u.AppUser.UserName == request.Username)
                .OrderBy(a => a.Activity.Date)
                .ProjectTo<UserActivityDto>(m_Mapper.ConfigurationProvider).AsQueryable();

                switch (request.Predicate)
                {
                    case "past":
                        query = query.Where(d => d.Date <= DateTime.UtcNow);
                        break;
                    case "hosting":
                        query = query.Where(d => d.HostUsername == request.Username);
                        break;
                    default:
                        query = query.Where(d => d.Date >= DateTime.UtcNow);
                        break;
                }

                var activities = await query.ToListAsync();

                return Result<List<UserActivityDto>>.Succes(activities);
            }
        }
    }
}