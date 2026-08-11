# EventBooking

Event booking platform built with Clean Architecture, DDD and CQRS (MediatR) on .NET 8.

## Stack

.NET 8 · ASP.NET Core Web API · PostgreSQL + EF Core · MediatR · ASP.NET Core Identity

## Architecture

Clean Architecture, 4 layers, dependencies point inward — toward Domain:

`API → Infrastructure → Application → Domain`

- **Domain** — `Event` (aggregate root) with invariant validation, `BookSeats()` as the single entry point for creating a `Booking`.
- **Application** — CQRS via MediatR, a separate Command/Query + Handler per use case.
- **Infrastructure** — EF Core, repositories, PostgreSQL.
- **API** — thin controllers, global `ExceptionMiddleware`.

## Implemented

- Clean Architecture + DDD + CQRS (MediatR)
- Optimistic concurrency (`RowVersion`)
- Global error handling

## API

| Method | Route | Description |
|---|---|---|
| `POST` | `/Event` | Create an event |
| `GET` | `/Event/{id}` | Get an event |
| `POST` | `/Event/{id}/book` | Book seats |

## Running locally

```bash
dotnet user-secrets set "ConnectionStrings:EventBookingConnection" "Host=localhost;Port=5432;Database=eventbooking;Username=postgres;Password=<your-password>" --project API
dotnet ef database update --project Infrastructure --startup-project API
dotnet run --project API
```

Swagger: `https://localhost:<port>/swagger`
