using Application.Core;
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
            private IMapper m_Mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.m_Mapper = mapper;
                m_Context = context;
            }

            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await m_Context.Activities
                .ProjectTo<ActivityDto>(m_Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

                return Result<List<ActivityDto>>.Succes(result);
            }
        }
    }
}