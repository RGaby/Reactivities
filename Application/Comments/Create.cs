using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    public class CreateComment
    {
        public class Command : IRequest<Result<CommentDto>>
        {
            public required string Body { get; set; }
            public Guid ActivityId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Body).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<CommentDto>>
        {
            private DataContext m_DataContext;
            private IMapper m_Mapper;
            private IUserAccessor m_UserAccessor;

            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                m_DataContext = dataContext;
                m_Mapper = mapper;
                m_UserAccessor = userAccessor;
            }

            public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
            {

                var activity = await m_DataContext.Activities.FindAsync(request.ActivityId);
                if (activity == null) return null;

                var user = await m_DataContext.Users.Include(p => p.Photos).SingleOrDefaultAsync(user => user.UserName == m_UserAccessor.GetUsername());

                var comment = new Comment
                {
                    Author = user,
                    Activity = activity,
                    Body = request.Body
                };

                activity.Comments.Add(comment);

                var success = await m_DataContext.SaveChangesAsync() > 0;
                if (success) return Result<CommentDto>.Succes(m_Mapper.Map<CommentDto>(comment));

                return Result<CommentDto>.Failure("Failed to add commnet");
            }
        }

    }
}