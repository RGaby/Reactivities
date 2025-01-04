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
    public class Details
    {
        public class Query : IRequest<Result<Profile>>
        {
            public required string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Profile>>
        {
            private readonly DataContext m_Context;
            private readonly IMapper m_Mapper;
            private readonly IUserAccessor m_UserAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                this.m_UserAccessor = userAccessor;
                this.m_Mapper = mapper;
                this.m_Context = context;

            }

            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await m_Context.Users.ProjectTo<Profile>(m_Mapper.ConfigurationProvider, new { currentUsername = m_UserAccessor.GetUsername() })
                        .SingleOrDefaultAsync(x => x.UserName == request.Username);
                if (user == null) return null;

                return Result<Profile>.Succes(user);
            }
        }

    }
}