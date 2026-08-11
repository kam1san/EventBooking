using Application.Events.Commands.BookSeats;
using Application.Events.Commands.CreateEvent;
using Application.Events.Queries.GetEventById;
using MediatR;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await mediator.Send(new GetEventByIdQuery(id));
            return Ok(result);
        }

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

        [HttpPost("{id}/book")]
        public async Task<IActionResult> Book(Guid id, [FromBody] BookSeatsRequest request)
        {
            var userId = "demo-user";

            var bookingId = await mediator.Send(new BookSeatsCommand(id, userId, request.Seats));
            return Ok(new { bookingId });
        }
    }

    public record CreateEventRequest(string Title, string Description, DateTime Date, int TotalSeats);
    public record BookSeatsRequest(int Seats);
}
