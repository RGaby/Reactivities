using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<Activity>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<Activity>>>
        {

            private readonly DataContext m_Context;

            public Handler(DataContext context)
            {
                m_Context = context;
            }

            public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await m_Context.Activities.ToListAsync();
                return Result<List<Activity>>.Succes(result);
            }
        }
    }
}