using AutoMapper;
using ires.Application.Queries.Car;
using ires.Application.ViewModels;
using ires.Domain.Common;
using ires.Domain.Contracts;
using MediatR;

namespace ires.Application.QueryHandlers
{
    public class CarQueryHandler(ICarService _carService, IMapper _mapper) :
        IRequestHandler<GetCarByIdQuery, CarViewModel>,
        IRequestHandler<GetAllCarsQuery, PaginatedResult<CarViewModel>>
    {
        public async Task<PaginatedResult<CarViewModel>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<PaginatedResult<CarViewModel>>(await _carService.GetAllCars(request.data));
        }

        public async Task<CarViewModel> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<CarViewModel>(await _carService.GetCarById(request.id));
        }
    }
}
