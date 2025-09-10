IPP - Company/Employee Management API

This is a .NET 9 Web API project using Clean Architecture with JWT authentication. The API supports managing Companies, Employees, Projects, and Assignments.


SETUP

1) Clone the repository:
git clone <https://github.com/grigavagyan/IPP>
cd IPP

2) Restore dependencies:
dotnet restore

3) Ensure SQL Server is running and accessible.


RUN

Start the API:
dotnet run --project IPP.API

Swagger UI is available at:
http://localhost:7777/swagger
https://localhost:8888/swagger


CONNECTION STRING

In appsettings.json:
"ConnectionStrings": {
"DefaultConnection": "Server=.;Database=IPP_DB;Trusted_Connection=True;TrustServerCertificate=True;"
}


SEED / SAMPLE DATA

Default users:

Email	            Password	    Role	     Name
admin@demo.com    Pass@123	    Admin	     Admin
user@demo.com     Pass@123	    User	     User

JWT CREDENTIALS

Endpoint: POST /auth/login

Request body example:
{
  "email": "admin@demo.com",
  "password": "Pass@123"
}

Response example:
{
  "accessToken": "<JWT_TOKEN>",
  "expiresAt": "2025-09-10T17:00:00Z"
}

Use in header:
Authorization: Bearer <JWT_TOKEN>


API ENDPOINTS

COMPANIES

Method	 Route	               Roles	   Description
GET	     /api/companies	       Auth	     Get list of companies (paginated)
GET	     /api/companies/{id}	 Auth	     Get a company by ID
POST	   /api/companies	       Admin	   Create a new company
PUT	     /api/companies/{id}	 Admin	   Update a company
DELETE	 /api/companies/{id}	 Admin	   Delete a company (?force=true for cascade)

EMPLOYEES

Method	  Route	                                            Roles	   Description
GET	      /api/employees	                                  Auth	   Get list of employees
GET	      /api/employees/{id}	                              Auth	   Get employee by ID
POST	    /api/employees	                                  Admin	   Create employee
PUT	      /api/employees/{id}	                              Admin	   Update employee
DELETE	  /api/employees/{id}	                              Admin	   Delete employee
POST	    /api/employees/{id}/projects	                    Admin	   Assign projects to employee
DELETE	  /api/employees/{employeeId}/projects/{projectId}	Admin	   Unassign project
GET	      /api/employees/{employeeId}/projects	            Auth	   Get projects for employee

PROJECTS

Method	  Route	                                Roles	        Description
GET	      /api/projects	                        Auth        	Get list of projects
GET	      /api/projects/{id}	                  Auth	        Get project by ID
POST	    /api/projects	                        Admin       	Create project
PUT	      /api/projects/{id}	                  Admin	        Update project
DELETE  	/api/projects/{id}	                  Admin  	      Delete project
GET	      /api/projects/{projectId}/employees	  Auth	        Get employees assigned to project
