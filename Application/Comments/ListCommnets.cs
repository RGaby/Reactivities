using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    public class ListCommnets
    {
        public class Query : IRequest<Result<List<CommentDto>>>
        {
            public Guid ActivityId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
        {
            private DataContext m_DataContext;
            private IMapper m_Mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                m_DataContext = dataContext;
                m_Mapper = mapper;
            }

            public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await m_DataContext.Comments
                .Where(x => x.Activity != null && x.Activity.Id == request.ActivityId)
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<CommentDto>(m_Mapper.ConfigurationProvider)
                .ToListAsync();

                if (comments == null) return null;

                return Result<List<CommentDto>>.Succes(comments);
            }
        }
    }
}