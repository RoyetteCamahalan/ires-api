using AutoMapper;
using ires.AppService.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ires_api.Common
{
    [Route("api/[controller]")]
    public class BaseController(IMediator _mediator, IMapper _mapper) : Controller
    {

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
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.message = ex.Message;
                    //return FilterException(ex);
                }
            }
            else
            {
                result.message = ModelState.Values.SelectMany(m => m.Errors)
                    .Select(e => e.ErrorMessage).FirstOrDefault("");
            }

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}