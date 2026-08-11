
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext context;

        public EventRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Event?> GetById(Guid id) => 
            await context.Events.Include(x => x.Bookings).FirstOrDefaultAsync(x => x.Id == id);
        public async Task Add(Event ev) =>
            await context.Events.AddAsync(ev);
        public async Task AddBooking(Booking booking) =>
            await context.Bookings.AddAsync(booking);
        public async Task SaveChanges() =>
            await context.SaveChangesAsync();
    }
}
