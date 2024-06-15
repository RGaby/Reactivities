
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext dataContext;

            public Handler(DataContext dataContext)
            {
                this.dataContext = dataContext;
            }

            public async Task Handle(Command command, CancellationToken cancellationToken)
            {
                var activity = await dataContext.Activities.FindAsync(command.Id);
                if (activity == null)
                {
                    return;
                }

                dataContext.Remove(activity);
                await dataContext.SaveChangesAsync();
            }
        }
    }
}