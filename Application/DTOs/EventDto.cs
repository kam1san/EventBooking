namespace Application.DTOs
{
    public record EventDto(
        Guid Id,
        string Title,
        string Description,
        DateTime Date,
        int TotalSeats,
        int AvailableSeats
    );
}