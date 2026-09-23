using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<List<Event>> GetAllAsync() =>
            await context.Events.Include(x => x.Bookings).ToListAsync();

        public override async Task<Event?> GetByIdAsync(Guid id) =>
            await context.Events.Include(x => x.Bookings).FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddBookingAsync(Booking booking) =>
            await context.Bookings.AddAsync(booking);

        public async Task<List<Booking>> GetBookingsByUserIdAsync(string userId) =>
            await context.Bookings.Include(b => b.Event).Where(b => b.UserId == userId).ToListAsync();
    }
}
