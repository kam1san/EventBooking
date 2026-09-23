namespace Application.DTOs
{
    public class MyBookingDto
    {
        public Guid Id { get; set; }
        public int SeatsBooked { get; set; }
        public string EventTitle { get; set; } = "";
        public DateTime EventDate { get; set; }
    }
}
