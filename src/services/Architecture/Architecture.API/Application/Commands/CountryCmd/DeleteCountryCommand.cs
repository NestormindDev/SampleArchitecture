using MediatR;

namespace Architecture.API.Application.Commands
{
    public class DeleteCountryCommand : IRequest<bool>
    {
        public int Id { get; private set; }

        public DeleteCountryCommand(int id)
        {
            Id = id;
        }
    }
}
