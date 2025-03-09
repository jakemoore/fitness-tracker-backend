# Fitness Tracker API

## Overview
This is the back-end API for a fitness tracking application. It provides endpoints to manage workouts and track workout logs. The API is built with **ASP.NET Core**, uses **PostgreSQL** as the database, **Dapper** as the ORM, and **Firebase** for user authentication.

## Technologies Used
- **ASP.NET Core** - Web API framework
- **PostgreSQL** - Database
- **Dapper** - Lightweight ORM
- **Firebase** - Authentication
- **Npgsql** - PostgreSQL driver for .NET

## Setup Instructions
### Prerequisites
- .NET 8 or later installed
- PostgreSQL database set up
- Firebase project configured

### Configuration
1. **Set up environment variables** for database connection and Firebase:
   - `ConnectionStrings:DefaultConnection` - Your PostgreSQL connection string.
   - `Firebase:ProjectId` - Your Firebase project ID.

2. **Run database migrations** (if applicable) or ensure the following tables exist:
   
   ```sql
   CREATE TABLE Workouts (
       Id SERIAL PRIMARY KEY,
       UserId VARCHAR(128) NOT NULL,
       Name VARCHAR(255) NOT NULL,
       Sets INT NOT NULL,
       Reps INT NOT NULL,
       CreatedAt TIMESTAMP DEFAULT NOW()
   );

   CREATE TABLE WorkoutLogs (
       Id SERIAL PRIMARY KEY,
       UserId VARCHAR(128) NOT NULL,
       WorkoutId SERIAL NOT NULL,
       DateCompleted TIMESTAMPTZ DEFAULT NOW(),
       Sets INT NOT NULL,
       Reps INT NOT NULL,
       Weight DECIMAL(5,2) NULL,
       Notes TEXT NULL,
       FOREIGN KEY (WorkoutId) REFERENCES Workouts(Id) ON DELETE CASCADE
   );
   ```

3. **Run the application**:
   ```sh
   dotnet run
   ```

## API Endpoints
### **Workout Management**
| Method | Endpoint            | Description             |
|--------|---------------------|-------------------------|
| GET    | `/api/workouts`     | Get list of workouts    |
| POST   | `/api/workouts`     | Add a new workout       |
| DELETE | `/api/workouts/{id}`| Delete a workout        |

### **Workout Logs**
| Method | Endpoint                      | Description                          |
|--------|-------------------------------|--------------------------------------|
| POST   | `/api/workouts/logs`          | Add a workout log                    |
| GET    | `/api/workouts/history`       | Get workout history (paginated)      |

## Authentication
- Firebase authentication is used.
- Requests must include a **Bearer token** in the `Authorization` header.
- The user ID is extracted from the token in the request context.

## Future Enhancements
- Add update endpoints for workouts and workout logs.
- Implement filtering and sorting for workout history.
- Add unit tests for the API.

## License
This project is licensed under the MIT License.

