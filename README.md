# HelpDeskManagement

Help Desk Ticket Management System built using ASP.NET Core Web API, ASP.NET Core MVC, Entity Framework Core, SQL Server, xUnit, Moq and GitHub.

## Project Structure
- **HelpDesk.Api**: ASP.NET Core Web API with EF Core & Repository Pattern.
- **HelpDesk.Mvc**: ASP.NET Core MVC Application consuming Web API through `TicketService` HTTP Client Layer.
- **HelpDesk.Tests**: xUnit Unit Test Project with Moq testing `TicketController`.

## Features
- **API Endpoints**:
  - `GET /api/Ticket/All` - Get all tickets
  - `GET /api/Ticket/{id}` - Get ticket by Id
  - `POST /api/Ticket` - Create a new ticket
  - `PUT /api/Ticket/{id}` - Update an existing ticket
  - `DELETE /api/Ticket/{id}` - Delete a ticket
  - `GET /api/Ticket/Status/{status}` - Get tickets by status
- **MVC UI**:
  - **Dashboard**: Displays Total Tickets, Open Tickets, Closed Tickets.
  - **View All Tickets**: Table displaying all tickets.
  - **View Ticket Details**: Detailed ticket information view.
  - **Raise New Ticket**: Create form with Status hardcoded to 'Open' and Priority dropdown.
  - **Edit Ticket**: Update Title, Description, Priority dropdown, and Status dropdown.
  - **Delete Ticket**: Confirmation and ticket deletion.
  - **Filter Tickets by Status**: Filter tickets by selected Status.
- **Unit Testing**:
  - Fully mocked unit tests using Moq verifying all controller endpoints without DB dependency.

## How to Run
1. Start the API project (`HelpDesk.Api`):
   ```bash
   dotnet run --project HelpDesk.Api
   ```
2. Start the MVC project (`HelpDesk.Mvc`):
   ```bash
   dotnet run --project HelpDesk.Mvc
   ```
3. Run Unit Tests:
   ```bash
   dotnet test HelpDesk.Tests
   ```
