using AutoMapper;
using ires.Application.Commands.Car;
using ires.Application.ViewModels;
using ires.Domain.Contracts;
using ires.Domain.Models;
using MediatR;

namespace ires.Application.CommandHandlers
{
    public class CarCommandHandler(ICarService _carService, IMapper _mapper) :
        IRequestHandler<CreateCarCommand, CarViewModel>,
        IRequestHandler<UpdateCarCommand>
    {
        public async Task<Unit> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            await _carService.Update(_mapper.Map<Car>(request));
            return new Unit();
        }

        public async Task<CarViewModel> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            var result = await _carService.Create(_mapper.Map<Car>(request));
            return _mapper.Map<CarViewModel>(result);
        }
    }
}
