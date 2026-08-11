using Domain.Entities;

namespace Application.Interfaces
{
    public interface IEventRepository
    {
        Task<Event?> GetById(Guid id);
        Task Add(Event ev);
        Task AddBooking(Booking booking);
        Task SaveChanges();
    }
}
