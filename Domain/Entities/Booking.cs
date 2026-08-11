namespace Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; private set; }
        public Guid EventId { get; private set; }
        public string UserId { get; private set; }
        public int SeatsBooked { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Event Event { get; private set; }

        #region Constructors

        private Booking() { }

        public Booking(string userId, Guid eventId, int seats)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            EventId = eventId;
            SeatsBooked = seats;
            CreatedAt = DateTime.UtcNow;
        }

        #endregion
    }
}
