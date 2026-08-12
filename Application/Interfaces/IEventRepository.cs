using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task AddBookingAsync(Booking booking);
    }
}
