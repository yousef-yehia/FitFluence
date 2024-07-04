# Fitness Application API
The Fitness Application API provides endpoints to manage users, exercises, and food items. It supports user management, exercise tracking, and food management functionalities.

## Table of Contents
1. Features
2. API Endpoints
3. Authentication
4. Authorization
5. Error Handling

# Features
- User Management
- CRUD operations for users.
- User profile management including updating profile photo.
- Exercise Management
- CRUD operations for exercises.
- Filter exercises by muscle groups.
- Food Management
- CRUD operations for food items.
- Ability to rate and retrieve food items.
- Prerequisites
- Before running the application, ensure you have the following installed:

#API Endpoints
- Users
1. GET /api/User/GetAllUsers: Retrieves all users.
2. GET /api/User/GetAllClients: Retrieves all clients (users in the 'client' role).
3. POST /api/User/UpdateProfilePhoto: Updates user profile photo. Requires authentication.
4. PUT /api/User/UpdateUser: Updates user profile information. Requires authentication.
5. GET /api/User/GetUser?id={id}: Retrieves a user by ID.
6. DELETE /api/User/DeleteUser?username={username}: Deletes a user by username. Requires 'admin' role.
7. DELETE /api/User/DeleteMyAccount: Deletes the authenticated user's account. Requires authentication.
- Exercises
1. GET /api/Exercise/GetAllExercises: Retrieves all exercises.
2. GET /api/Exercise/GetAllExercisesByMuscle?muscleId={muscleId}: Retrieves exercises by muscle ID.
3. GET /api/Exercise/GetExercise?id={id}: Retrieves an exercise by ID.
4. POST /api/Exercise/CreateExercise: Creates a new exercise. Requires 'admin' or 'coach' role.
5. PUT /api/Exercise/UpdateExercise?id={id}: Updates an exercise by ID. Requires 'admin' or 'coach' role.
6. DELETE /api/Exercise/DeleteExercise?id={id}: Deletes an exercise by ID. Requires 'admin' or 'coach' role.
- Foods
1. GET /api/Food/GetAllFoods: Retrieves all food items.
2. GET /api/Food/GetFood?id={id}: Retrieves a food item by ID.
3. POST /api/Food/CreateFood: Creates a new food item. Requires 'admin' or 'coach' role.
4. POST /api/Food/GiveFoodRate?foodId={foodId}&rating={rating}: Rates a food item. Requires authentication.
5. PUT /api/Food/UpdateFoodRate?foodId={foodId}&rating={rating}: Updates the rating of a food item. Requires authentication.
6. DELETE /api/Food/DeleteFood?id={id}: Deletes a food item by ID. Requires 'admin' role.
- Fitness Plans
1. GET /api/DietPlan/GetDietPlans Retrieves all diet plans.
2. GET /api/WorkoutPlan/GetWorkoutPlans Retrieves all workout plans.
3. POST /api/DailyFood/AddDailyFood Adds daily food intake for a user.
4. POST /api/WorkoutHistory/AddWorkoutHistory
- And other APIs
# Authentication
-The API uses JWT (JSON Web Token) for authentication. Endpoints requiring authentication are marked with the [Authorize] attribute. Authentication endpoints are not part of this API but can be implemented using ASP.NET Core Identity or other authentication providers.

# Authorization
- Role-based authorization is implemented using the [Authorize(Roles = "admin, coach")] attribute. Roles can be configured in the database or via code, depending on the requirements.

# Error Handling
- Errors are handled centrally using try-catch blocks within each API controller method. Error responses follow a consistent format using the ApiResponse class for standardization.
