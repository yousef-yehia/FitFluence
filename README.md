Fitness Application API
The Fitness Application API provides endpoints to manage users, exercises, and food items. It supports user management, exercise tracking, and food management functionalities.

Table of Contents
Features
Prerequisites
Installation
Configuration
API Endpoints
Users
Exercises
Foods
Authentication
Authorization
Error Handling
Contributing
License
Features
User Management
CRUD operations for users.
User profile management including updating profile photo.
Exercise Management
CRUD operations for exercises.
Filter exercises by muscle groups.
Food Management
CRUD operations for food items.
Ability to rate and retrieve food items.
Prerequisites
Before running the application, ensure you have the following installed:

.NET SDK (compatible version)
Visual Studio or Visual Studio Code (optional)
SQL Server (or another compatible database)
Azure Cloudinary Account (for image upload functionality)
Installation
Clone the repository:

bash
Copy code
git clone <repository-url>
cd <repository-folder>
Restore dependencies:

bash
Copy code
dotnet restore
Update appsettings.json:

Configure database connection strings.
Configure Cloudinary account details (API key, secret, cloud name).
Apply database migrations:

bash
Copy code
dotnet ef database update
Run the application:

bash
Copy code
dotnet run
Configuration
appsettings.json contains configuration settings including database connection strings, Cloudinary credentials, and other application settings.
API Endpoints
Users
GET /api/User/GetAllUsers: Retrieves all users.
GET /api/User/GetAllClients: Retrieves all clients (users in the 'client' role).
POST /api/User/UpdateProfilePhoto: Updates user profile photo. Requires authentication.
PUT /api/User/UpdateUser: Updates user profile information. Requires authentication.
GET /api/User/GetUser?id={id}: Retrieves a user by ID.
DELETE /api/User/DeleteUser?username={username}: Deletes a user by username. Requires 'admin' role.
DELETE /api/User/DeleteMyAccount: Deletes the authenticated user's account. Requires authentication.
Exercises
GET /api/Exercise/GetAllExercises: Retrieves all exercises.
GET /api/Exercise/GetAllExercisesByMuscle?muscleId={muscleId}: Retrieves exercises by muscle ID.
GET /api/Exercise/GetExercise?id={id}: Retrieves an exercise by ID.
POST /api/Exercise/CreateExercise: Creates a new exercise. Requires 'admin' or 'coach' role.
PUT /api/Exercise/UpdateExercise?id={id}: Updates an exercise by ID. Requires 'admin' or 'coach' role.
DELETE /api/Exercise/DeleteExercise?id={id}: Deletes an exercise by ID. Requires 'admin' or 'coach' role.
Foods
GET /api/Food/GetAllFoods: Retrieves all food items.
GET /api/Food/GetFood?id={id}: Retrieves a food item by ID.
POST /api/Food/CreateFood: Creates a new food item. Requires 'admin' or 'coach' role.
POST /api/Food/GiveFoodRate?foodId={foodId}&rating={rating}: Rates a food item. Requires authentication.
PUT /api/Food/UpdateFoodRate?foodId={foodId}&rating={rating}: Updates the rating of a food item. Requires authentication.
DELETE /api/Food/DeleteFood?id={id}: Deletes a food item by ID. Requires 'admin' role.
Authentication
The API uses JWT (JSON Web Token) for authentication. Endpoints requiring authentication are marked with the [Authorize] attribute. Authentication endpoints are not part of this API but can be implemented using ASP.NET Core Identity or other authentication providers.

Authorization
Role-based authorization is implemented using the [Authorize(Roles = "admin, coach")] attribute. Roles can be configured in the database or via code, depending on the requirements.

Error Handling
Errors are handled centrally using try-catch blocks within each API controller method. Error responses follow a consistent format using the ApiResponse class for standardization.

Contributing
Contributions are welcome! Please follow these steps to contribute:

Fork the repository.
Create a new branch (git checkout -b feature/new-feature).
Make your changes.
Commit your changes (git commit -am 'Add new feature').
Push to the branch (git push origin feature/new-feature).
Create a new Pull Request.
License
This project is licensed under the MIT License - see the LICENSE file for details.
