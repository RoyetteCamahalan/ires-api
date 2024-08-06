using AutoMapper;
using ires.AppService.Dto.Car;
using ires.CarRental.Commands.Car;
using ires.CarRental.Queries.Car;
using ires.CarRental.ViewModels;
using ires.Domain.Common;
using ires_api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController(IMediator _mediator, IMapper _mapper) : BaseController(_mediator, _mapper)
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            return await Handle<PaginatedResult<CarViewModel>>(new GetAllCarsQuery(request));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await Handle<CarViewModel>(new GetCarByIdQuery(id));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCarRequestDto requestDto)
        {
            return await Handle<CreateCarRequestDto, CreateCarCommand, CarViewModel>(requestDto);
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCarRequestDto requestDto)
        {
            return await Handle<UpdateCarRequestDto, UpdateCarCommand, object>(requestDto);
        }
    }
}
