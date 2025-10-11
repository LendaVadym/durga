# Durga Server API

A comprehensive .NET 9 Web API with authentication, Entity Framework Core, and Docker support.

## Features

- **ASP.NET Core 9.0** - Latest .NET framework
- **JWT Authentication** - Secure token-based authentication
- **ASP.NET Core Identity** - User management and authentication
- **Entity Framework Core** - ORM with SQL Server support
- **OpenAPI/Swagger** - API documentation with JWT support
- **Docker Support** - Containerized deployment
- **CORS Enabled** - Cross-origin resource sharing
- **Auto Database Migration** - Automatic database setup on startup

## Project Structure

```
server/
├── Durga.Api/
│   ├── Controllers/
│   │   ├── AuthController.cs      # Authentication endpoints
│   │   └── WeatherForecastController.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs # Entity Framework context
│   ├── DTOs/
│   │   └── AuthDtos.cs           # Data transfer objects
│   ├── Models/
│   │   └── ApplicationUser.cs    # Extended Identity user model
│   ├── Dockerfile                # Docker configuration
│   ├── Program.cs                # Application startup
│   └── appsettings.json          # Configuration
├── docker-compose.yml            # Docker Compose configuration
├── Durga.sln                     # Solution file
└── README.md
```

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (optional, for containerized deployment)
- [SQL Server](https://www.microsoft.com/sql-server/) or Docker for SQL Server

### Running Locally

1. **Clone the repository**
   ```bash
   cd server
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure database connection**
   Update the connection string in `Durga.Api/appsettings.json` if needed:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost,1433;Database=DurgaDb;User Id=sa;Password=YourPassword;TrustServerCertificate=true;MultipleActiveResultSets=true"
     }
   }
   ```

4. **Run the application**
   ```bash
   dotnet run --project Durga.Api
   ```

5. **Access the API**
   - API Base URL: `http://localhost:5000`
   - Swagger Documentation: `http://localhost:5000/swagger`
   - OpenAPI Specification: `http://localhost:5000/openapi/v1.json`

### Running with Docker

1. **Using Docker Compose (Recommended)**
   ```bash
   docker-compose up -d
   ```
   This will start both the API and SQL Server containers.

2. **Building the API container only**
   ```bash
   docker build -t durga-api ./Durga.Api
   docker run -p 5000:5000 durga-api
   ```

## API Endpoints

### Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register a new user | No |
| POST | `/api/auth/login` | Login user | No |
| GET | `/api/auth/profile` | Get user profile | Yes |
| POST | `/api/auth/change-password` | Change user password | Yes |
| POST | `/api/auth/logout` | Logout user | Yes |

### Authentication Request/Response Examples

**Register User:**
```json
POST /api/auth/register
{
  "email": "user@example.com",
  "password": "password123",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Login User:**
```json
POST /api/auth/login
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "expiresAt": "2024-01-15T12:00:00Z"
}
```

## Configuration

### JWT Settings

Configure JWT settings in `appsettings.json`:

```json
{
  "JWT": {
    "SecretKey": "YourVeryLongSecretKeyForJWTTokens123456789",
    "Issuer": "Durga.Api",
    "Audience": "Durga.Client",
    "ExpiryInDays": 7
  }
}
```

### Database Configuration

The application uses SQL Server by default. The connection string can be configured in `appsettings.json` or through environment variables:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your connection string here"
  }
}
```

## Development

### Adding New Features

1. **Models**: Add new entities to the `Models` folder
2. **DTOs**: Create data transfer objects in the `DTOs` folder
3. **Controllers**: Add new controllers to the `Controllers` folder
4. **Database**: Update `ApplicationDbContext` with new DbSets
5. **Migrations**: Create and run database migrations

### Database Migrations

```bash
# Add a new migration
dotnet ef migrations add MigrationName --project Durga.Api

# Update database
dotnet ef database update --project Durga.Api
```

## Docker Configuration

### Environment Variables

The following environment variables are used in the Docker setup:

- `ASPNETCORE_ENVIRONMENT`: Set to "Development" or "Production"
- `ConnectionStrings__DefaultConnection`: Database connection string
- `JWT__SecretKey`: JWT secret key
- `JWT__Issuer`: JWT issuer
- `JWT__Audience`: JWT audience
- `JWT__ExpiryInDays`: JWT token expiry in days

### Docker Compose Services

- **durga-api**: The .NET API application
- **sql-server**: SQL Server 2022 database
- **Network**: `durga-network` for inter-container communication
- **Volume**: `sql-server-data` for database persistence

## Security

- JWT tokens are used for authentication
- Passwords are hashed using ASP.NET Core Identity
- HTTPS can be enabled by removing the `--no-https` flag and configuring certificates
- CORS is configured to allow all origins (modify for production)

## Production Considerations

1. **HTTPS**: Enable HTTPS and configure proper certificates
2. **CORS**: Restrict CORS to specific origins
3. **Secrets**: Use Azure Key Vault or similar for storing secrets
4. **Database**: Use a managed database service
5. **Logging**: Configure structured logging
6. **Health Checks**: Add health check endpoints
7. **Rate Limiting**: Implement rate limiting for API endpoints

## Troubleshooting

### Common Issues

1. **Database Connection Errors**
   - Ensure SQL Server is running
   - Verify connection string
   - Check firewall settings

2. **JWT Token Issues**
   - Verify JWT configuration
   - Check token expiry
   - Ensure secret key is properly set

3. **Docker Issues**
   - Check Docker daemon is running
   - Verify port availability
   - Check container logs: `docker-compose logs durga-api`

### Viewing Logs

```bash
# Application logs
dotnet run --project Durga.Api

# Docker logs
docker-compose logs -f durga-api
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## License

This project is licensed under the MIT License.