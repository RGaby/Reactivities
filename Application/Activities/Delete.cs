
using Application.Core;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext dataContext;

            public Handler(DataContext dataContext)
            {
                this.dataContext = dataContext;
            }


            public async Task<Result<Unit>> Handle(Command command, CancellationToken cancellationToken)
            {
                var activity = await dataContext.Activities.FindAsync(command.Id);
                if (activity == null)
                {
                    return null;
                }

                dataContext.Remove(activity);
                var result = await dataContext.SaveChangesAsync() > 0;
                if (!result)
                {
                    return Result<Unit>.Failure("Failed to delete activity");
                }
                return Result<Unit>.Succes(Unit.Value);
            }
        }
    }
}