

﻿using AutoMapper;
using ires.Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Common
{
    [Route("api/[controller]")]
    public class BaseController(IMediator mediator, IMapper mapper) : Controller
    {
        protected readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        protected async Task<IActionResult> Handle<T1, T2, T3>(dynamic dto)
        {

            var queryOrCommmand = _mapper.Map<T2>(dto);

            return await Handle<T3>(queryOrCommmand);
        }

        protected async Task<IActionResult> Handle<T>(dynamic queryOrCommmand)
        {
            if (queryOrCommmand == null)
                return BadRequest();

            var result = new ServerResponse<T>();
            if (ModelState.IsValid)
            {
                try
                {
                    result.Data = await _mediator.Send(queryOrCommmand);
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message; 
                    result.Success = false;
                }
            }
            else
            {
                result.Message = ModelState.Values.SelectMany(m => m.Errors)
                    .Select(e => e.ErrorMessage).FirstOrDefault("");
            }

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}