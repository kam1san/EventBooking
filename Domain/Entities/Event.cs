using Domain.Exceptions;

namespace Domain.Entities
{
    public class Event
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Date { get; private set; }
        public int TotalSeats { get; private set; }
        public int AvailableSeats { get; private set; }
        public List<Booking> Bookings { get; private set; } = new();

        public byte[] RowVersion { get; private set; }

        #region Constructors

        private Event() { }

        public Event(string title, string description, DateTime date, int totalSeats)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Event title cannot be empty");

            if (date <= DateTime.UtcNow)
                throw new DomainException("Event date must be in the future");

            if (totalSeats <= 0)
                throw new DomainException("Total seats must be greater than zero");

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Date = date;
            TotalSeats = totalSeats;
            AvailableSeats = totalSeats;
        }

        #endregion

        #region Methods

        public Booking BookSeats(string userId, int seats)
        {
            if (seats <= 0)
                throw new DomainException("Invalid seat count");

            if (seats > AvailableSeats)
                throw new DomainException("Not enough seats");

            AvailableSeats -= seats;

            var booking = new Booking(userId, Id, seats);
            Bookings.Add(booking);
            return booking;
        }

        #endregion
    }
}
