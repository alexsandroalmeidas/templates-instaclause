# Interview Assignment – .NET Core + PostgreSQL

## 🚀 Setup Instructions

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [PostgreSQL 14+](https://www.postgresql.org/download/)

### Database Setup
1. Create PostgreSQL database:
   ```sql
   CREATE DATABASE "TemplatesDb";
   ```
2. Update the connection string in `src/Templates.Api/appsettings.json` if needed.
3. Apply migrations:
   ```bash
   dotnet ef database update --project src/Templates.Api
   ```

### Run the API
```bash
dotnet run --project src/Templates.Api
```

API will be available at:
- Swagger UI (development): https://localhost:5000/swagger

## ✅ Existing Functionality
- **Users**
  - Entity: `User` (`Id`, `FirstName`, `LastName`, `Email`)
  - CRUD endpoints at `/api/users`
  - Unique index on `Email` is configured in `AppDbContext`

## 📝 What the Candidate Needs to Add
The candidate should implement the Template Management feature.

### Requirements (to be implemented by the candidate)
1. Move business logic from `Controllers/UsersController` to `Services/UsersService`:

2. Add `Template` entity:
   - `Id` (int)
   - `Value` (string, required - valid scriban template)

3. Add `TemplatesController` with endpoints:
   - `PUT /api/templates` → Create a template
   - `Post /api/templates` → Updates a template
   - `GET /api/templates/{id}` → Get a template
   - `DELETE /api/templates/{id}` → Delete a template
   - `GET /api/templates/{id}/compile/{userId}` → Compiles a template for a user

4. Implementation:
   - Expand User entity with more information (for example: Street, City, Country, HouseNumber...)
   - Template Value field must be a valid scriban template, for example: "User {{ user.name }} lives in {{ user.city }}, {{ user.country }}"
   - Ensure that compile endpoint compiles given template using data of the specified user.
   - Implement `GET /api/templates/{id}/compile/{userId}/html` → Compiles the selected template for a specific user and generates HTML document using default styles indicated below.
   - Each user must be able to customize the styles of their HTML documents.
  
```css
h1 {
  font-family: "Arial";
  font-size: 16pt;
  font-weight: 700;
  text-decoration: underline;
  line-height: 1.15em;
}
h2 {
  font-family: "Arial";
  font-size: 12pt;
  font-weight: 700;
  line-height: 1.15em;
}

h3 {
  font-size: 10pt;
  font-weight: 700;
  line-height: 1.15em;
}

p {
  font-size: 10pt;
  font-weight: 400;
  line-height: 1.5em;
}
```

4. Deliverable:
   - Candidate should provide a link **GitHub Repo** with changes implemented
