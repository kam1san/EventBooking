using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Events.Queries.GetMyBookings
{
    public class GetMyBookingsQueryHandler : IRequestHandler<GetMyBookingsQuery, List<MyBookingDto>>
    {
        readonly IEventRepository eventRepository;
        readonly IMapper mapper;

        public  GetMyBookingsQueryHandler(IEventRepository eventRepository, IMapper mapper)
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
        }

        public async Task<List<MyBookingDto>> Handle(GetMyBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await eventRepository.GetBookingsByUserIdAsync(request.idUser);
            return mapper.Map<List<MyBookingDto>>(bookings);
        }
    }
}
