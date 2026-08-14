# EventBooking

Event booking platform built with Clean Architecture, DDD and CQRS (MediatR) on .NET 8.

## Stack

.NET 8 · ASP.NET Core Web API · PostgreSQL + EF Core · MediatR · ASP.NET Core Identity · RabbitMQ

## Architecture

Clean Architecture, 4 layers, dependencies point inward — toward Domain:

`API → Infrastructure → Application → Domain`

- **Domain** — `Event` (aggregate root) with invariant validation, `BookSeats()` as the single entry point for creating a `Booking`.
- **Application** — CQRS via MediatR, a separate Command/Query + Handler per use case.
- **Infrastructure** — EF Core, repositories, PostgreSQL, RabbitMQ publisher.
- **API** — thin controllers, global `ExceptionMiddleware`.
- **EventBooking.Worker** — standalone background service, consumes domain events from RabbitMQ independently of the API process.

## Implemented

- Clean Architecture + DDD + CQRS (MediatR)
- Optimistic concurrency (`RowVersion`)
- Global error handling
- Request validation — FluentValidation as a MediatR pipeline behavior, structured per-field error responses
- Generic base repository (`IBaseRepository<T>` / `BaseRepository<T>`)
- JWT Authentication — register/login via ASP.NET Core Identity, Bearer tokens, write endpoints protected with `[Authorize]` while reads stay public
- Dockerized — API + PostgreSQL via a single `docker-compose up`, migrations applied automatically on startup
- RabbitMQ messaging — `BookSeats` publishes a `BookingCreatedDomainEvent` to a `direct` exchange after the transaction commits; a separate `EventBooking.Worker` process consumes and processes it independently, decoupled from the request/response cycle

## Roadmap

- [ ] Tests (unit + integration)
- [ ] CI/CD (GitHub Actions)

## API

| Method | Route | Description | Auth |
|---|---|---|---|
| `POST` | `/Auth/register` | Register a new user | – |
| `POST` | `/Auth/login` | Log in, get a JWT | – |
| `GET` | `/Auth/me` | Get current user info | required |
| `GET` | `/Event` | List all events | – |
| `GET` | `/Event/{id}` | Get an event | – |
| `POST` | `/Event` | Create an event | required |
| `POST` | `/Event/{id}/book` | Book seats | required |

## Running locally

### With Docker (recommended)

```bash
cp .env.example .env   # fill in your own values
docker-compose up --build
```

API + PostgreSQL + RabbitMQ + the Worker consumer all start together on a shared network, EF Core migrations apply automatically on startup — no manual setup needed.

Swagger: `http://localhost:8080/swagger`
RabbitMQ management UI: `http://localhost:15672`

### Without Docker

```bash
dotnet user-secrets set "ConnectionStrings:EventBookingConnection" "Host=localhost;Port=5432;Database=eventbooking;Username=postgres;Password=<your-password>" --project API
dotnet ef database update --project Infrastructure --startup-project API
dotnet run --project API
```

Swagger: `https://localhost:<port>/swagger`
