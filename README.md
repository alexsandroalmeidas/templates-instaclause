# Interview Assignment – .NET Core + PostgreSQL

## 🚀 Setup Instructions

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [PostgreSQL 14+](https://www.postgresql.org/download/)

### Database Setup
1. Create PostgreSQL database:
   ```sql
   CREATE DATABASE "TaskListDb";
   ```
2. Update the connection string in `src/TaskList.Api/appsettings.json` if needed.
3. Apply migrations:
   ```bash
   dotnet ef database update --project src/TaskList.Api
   ```

### Run the API
```bash
dotnet run --project src/TaskList.Api
```

API will be available at:
- Swagger UI (development): https://localhost:5000/swagger

## ✅ Existing Functionality
- **Users**
  - Entity: `User` (`Id`, `FirstName`, `LastName`, `Email`)
  - CRUD endpoints at `/api/users`
  - Unique index on `Email` is configured in `AppDbContext`

## 📝 What the Candidate Needs to Add
The candidate should implement the Task Management feature.

### Requirements (to be implemented by the candidate)
1. Move business logic from `Controllers/UsersController` to `Services/UsersService`:

2. Add `TaskItem` entity:
   - `Id` (int)
   - `Title` (string, required)
   - `Description` (string, optional)
   - `Status` (enum: `Pending`, `InProgress`, `Completed`)
   - `DueDate` (DateTime, nullable)
   - `UserId` (foreign key → Users.Id)

3. Add `TasksController` with endpoints:
   - `POST /api/tasks` → Create a task for a user
   - `GET /api/tasks?userId={id}` → Get tasks for a user
   - `PUT /api/tasks/{id}/status` → Update only task status
   - `DELETE /api/tasks/{id}` → Delete a task

4. Implement Business Rules:
   - A user **cannot have more than 10 pending tasks**.
   - When marking a task `Completed`, set `DueDate` if it is null.
   - A user **cannot have duplicate task titles** (add composite unique index migration `UserId + Title`).
   - If a task’s `DueDate` is in the past, its status **cannot be Pending or InProgress**.
   - Deleting a **User** must cascade delete their **Tasks**.

4. Deliverable:
   - Candidate should provide a link **GitHub Repo** with changes implemented
