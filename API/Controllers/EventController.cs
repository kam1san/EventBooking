using System.Security.Claims;
using Application.Events.Commands.BookSeats;
using Application.Events.Commands.CreateEvent;
using Application.Events.Queries.GetAllEvents;
using Application.Events.Queries.GetEventById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        readonly IMediator mediator;

        public EventController(IMediator mediator)
        { 
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllEventsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await mediator.Send(new GetEventByIdQuery(id));
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
        {
            var eventId = await mediator.Send(new CreateEventCommand(
                request.Title,
                request.Description,
                request.Date,
                request.TotalSeats
            ));

            return CreatedAtAction(nameof(GetById), new { id = eventId }, new { id = eventId });
        }

        [Authorize]
        [HttpPost("{idEvent}/book")]
        public async Task<IActionResult> Book(Guid idEvent, [FromBody] BookSeatsRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var bookingId = await mediator.Send(new BookSeatsCommand(idEvent, userId, request.Seats));
            return Ok(new { bookingId });
        }
    }

    public record CreateEventRequest(string Title, string Description, DateTime Date, int TotalSeats);
    public record BookSeatsRequest(int Seats);
}
