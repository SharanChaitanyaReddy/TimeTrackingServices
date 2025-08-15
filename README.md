# TimeTrackingServices

TimeTrackingServices is an Azure-based .NET Web API application designed to manage time tracking, approvals, billing, and productivity benchmarks for teams and projects.

## Features

- **User Management**: Create, update, and manage users with roles and teams.
- **Time Tracking**: Log time entries for tasks and projects.
- **Approvals**: Manage approval workflows for time entries.
- **Billing**: Generate billing entries based on time logs and hourly rates.
- **Productivity Benchmarks**: Track expected vs. actual hours for users.
- **Holiday Management**: Define holidays to prevent time entries on non-working days.
- **Audit Logs**: Immutable logs for tracking user actions.
- **Authentication**: Secure login and registration using JWT-based authentication.

## Technologies Used

- **Framework**: .NET 8.0
- **Database**: PostgreSQL
- **ORM**: Dapper
- **Authentication**: JWT
- **API Documentation**: Swagger/OpenAPI
- **Testing**: xUnit, Moq

## Project Structure

```
TimeTrackingServices/
├── ClockIn/                # Main application
│   ├── Controllers/        # API controllers
│   ├── Models/             # Data models
│   ├── DataLayer/          # Database context and repositories
│   ├── Security/           # Authentication and security models
│   ├── Properties/         # Configuration files
│   ├── ClockIn.csproj      # Project file
├── ClockIn.UnitTest        # Unit tests
│   ├── Controllers/        # Test cases for controllers
│   ├── ClockIn.UnitTest.csproj # Test project file
├── SQL/                    # Database scripts
│   ├── initial_db_scripts.sql # Initial database schema
├── TimeTrackingServices.sln # Solution file
```

## Getting Started

### Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/TimeTrackingServices.git
   cd TimeTrackingServices
   ```

2. Set up the database:
   - Create a PostgreSQL database.
   - Run the SQL scripts in `SQL/initial_db_scripts.sql`.

3. Update the connection string in `ClockIn/appsettings.json`:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=clockinout_db;Username=postgres;Password=<YOUR DB PASSWORD>>"
   }
   ```

4. Restore dependencies:
   ```sh
   dotnet restore
   ```

5. Build the solution:
   ```sh
   dotnet build
   ```

6. Run the application:
   ```sh
   dotnet run --project ClockIn
   ```

### Running Tests

To execute unit tests:
```sh
   dotnet test
```

## API Documentation

Swagger is enabled for API documentation. Once the application is running, navigate to:
```
http://localhost:5046/swagger
```

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).

## Contact

For questions or support, please contact:
- **Email**: support@timetrackingservices.com
- **GitHub**: [yourusername](https://github.com/yourusername)
