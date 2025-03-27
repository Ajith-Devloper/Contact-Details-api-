Angular and DotNet Core Developer Assessment

Overview
This is a full-stack application with Angular as the front-end and ASP.NET Core Web API as the backend. Data is stored in an MS SQL Server database.

 Features
- Contact Management (Add, Edit, Delete, View)
- Responsive Design with Bootstrap
- JWT Token Authentication
- Logging with NLog
- API Unit Tests using xUnit
- Centralized Error Handling with Custom Exception Filters



Requirements
- Node.js (v18+)
- Angular CLI (v18+)
- .NET SDK (v6.0+)
- SQL Server
- Postman (Optional for API testing)



 Installation

Backend Setup
1. Navigate to the API directory:
  
    cd ContactDetails-Api
  
2. Configure the SQL Server in appsettings.json =>   "ConnectionStrings": { "DB": "server =DESKTOP-UOJ5HEV\\MSSQLSERVER01;User id =Ajith;Password=1234;database = Contact;TrustServerCertificate=true;" },

3. Apply database migrations:
    
    dotnet ef database update
  
4. Run the API:

    dotnet run
  

### Frontend Setup
1. Navigate to the Angular directory:
   
    cd ContactDetails-UI
    
2. Install dependencies:
   
    npm install
  
3. Run the app:
   
    ng serve
    

The app will be available at `http://localhost:4200`.


## API Endpoints

| Method | Endpoint             | Description                |
|---------|----------------------|----------------------------|
| GET     | /api/contact          | Get all contacts           |
| GET     | /api/contact/{id}     | Get contact by ID          |
| POST    | /api/contact          | Add a new contact          |
| PUT     | /api/contact/{id}     | Update existing contact    |
| DELETE  | /api/contact/{id}     | Delete a contact           |

---

## Authentication
- Generate a JWT using `/api/auth/login`
- Use it for secured endpoints:

Authorization: Bearer {token}

 Logging
- NLog is used for logging in API and UI.
- Logs are stored in `Logs/logfile-${shortdate}.log`.

---
Testing
- API Tests:

  dotnet test
  
- UI Tests:
  
  ng test

 Additional Notes
- Follow clean code principles using the repository and service patterns.
- Use dependency injection and proper error handling.


Submission
Submit your GitHub repository link once complete.

Happy Coding!

