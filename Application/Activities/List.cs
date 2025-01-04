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
        public class Query : IRequest<Result<PagedList<ActivityDto>>>
        {
            public required ActivityParams PagingParams { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
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

            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = m_Context.Activities
                .Where(d => d.Date >= request.PagingParams.StartDate)
                .OrderBy(d => d.Date)
                .ProjectTo<ActivityDto>(m_Mapper.ConfigurationProvider, new { currentUsername = m_UserAccessor.GetUsername() })
                .AsQueryable();

                if (request.PagingParams.IsGoing && !request.PagingParams.IsHost)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.UserName == m_UserAccessor.GetUsername()));
                }

                if (!request.PagingParams.IsGoing && request.PagingParams.IsHost)
                {
                    query = query.Where(x => x.HostUsername == m_UserAccessor.GetUsername());
                }

                return Result<PagedList<ActivityDto>>.Succes(await PagedList<ActivityDto>.CreateAscyns(query, request.PagingParams.PageNumber, request.PagingParams.PageSize));
            }
        }
    }
}