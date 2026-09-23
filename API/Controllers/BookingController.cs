using Application.Events.Queries.GetMyBookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        readonly IMediator mediator;

        public BookingController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await mediator.Send(new GetMyBookingsQuery(userId));
            return Ok(result);
        }

    }
}