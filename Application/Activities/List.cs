using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<Activity>>
        {
        }

        public class Handler : IRequestHandler<Query, List<Activity>>
        {

            private readonly DataContext m_Context;

            public Handler(DataContext context)
            {
                m_Context = context;
            }

            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await m_Context.Activities.ToListAsync();
            }
        }
    }
}