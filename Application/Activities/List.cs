using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
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

            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await m_Context.Activities
                .ProjectTo<ActivityDto>(m_Mapper.ConfigurationProvider, new { currentUsername = m_UserAccessor.GetUsername() })
                .ToListAsync(cancellationToken);

                return Result<List<ActivityDto>>.Succes(result);
            }
        }
    }
}