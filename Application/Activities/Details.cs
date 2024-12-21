
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }


        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
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

            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await m_Context.Activities
                .ProjectTo<ActivityDto>(m_Mapper.ConfigurationProvider, new {currentUsername = m_UserAccessor.GetUsername()})
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                return Result<ActivityDto>.Succes(activity);
            }
        }
    }
}