using Api.DTO.ClientDto;
using Api.DTO.CoachDto;
using Api.DTO.DietPlanDto;
using Api.DTO.ExerciseDto;
using Api.DTO.FoodDto;
using Api.DTO.WorkoutHistoryDto;
using Api.DTO.WorkoutPlanDto;
using CloudinaryDotNet.Actions;
using Core.Models;

namespace Api.Helper
{
    public static class CustomMappers
    {
        public static List<WorkoutPlanReturnDto> MapWorkoutplanToWorkoutPLanReturnDto(List<WorkoutPlan> workOutplans)
        {
            var workoutPlansReturn = workOutplans.Select(w => new WorkoutPlanReturnDto
            {
                Id = w.Id,
                Name = w.Name,
                DateAdedd = w.DateAdedd,
                WorkOutPlanExercises = w.WorkOutPlanExercises.Select(we => new WorkoutPlanExerciseDto
                {
                    ExerciseId = we.ExerciseId,
                    ExerciseName = we.ExerciseName,
                    ExerciseGifUrl = we.ExerciseGifUrl,
                })
            }).ToList();
            return workoutPlansReturn;
        }
        public static List<ExerciseReturnDto> MapExerciseToExerciseReturnDto(List<Exercise> exercises)
        {
            var exerciseReturn = exercises.Select(e => new ExerciseReturnDto
            {
                Id = e.Id,
                Name = e.Name,
                GifUrl = e.GifUrl,
                //Sets = e.Sets,
                //MuscleName = e.Muscle.Name,
                Description = e.Description,
                FocusAreaUrl = e.FocusAreaUrl,
            }).ToList();
            return exerciseReturn;
        }

        public static List<CoachReturnDto> MapCoachToCoachReturnDto(List<Coach> coaches)
        {
            var coachReturn = coaches.Select(u => new CoachReturnDto
            {
                Age = u.AppUser.Age,
                Email = u.AppUser.Email,
                FullName = u.AppUser.Name,
                UserName = u.AppUser.UserName,
                PhoneNumber = u.AppUser.PhoneNumber,
                Gender = u.AppUser.Gender,
                CvUrl = u.CvUrl,
                FatWeight = u.AppUser.FatWeight,
                Height = u.AppUser.Height,
                ImageUrl = u.AppUser.ImageUrl,
                MuscleWeight = u.AppUser.MuscleWeight,
                UserId = u.AppUser.Id,
                Weight = u.AppUser.Weight,
            }).ToList();
            return coachReturn;
        }
        public static CoachReturnDto MapAppUserToCoachReturnDto(AppUser user)
        {
            var coachReturn = new CoachReturnDto
            {
                Age = user.Age,
                Email = user.Email,
                FullName = user.Name,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                CvUrl = user.Coach.CvUrl,
                FatWeight = user.FatWeight,
                Height = user.Height,
                ImageUrl = user.ImageUrl,
                MuscleWeight = user.MuscleWeight,
                UserId = user.Id,
                Weight = user.Weight,
            };
            return coachReturn;
        }
        public static List<ClientReturnDto> MapClientToClientReturnDto(List<Client> clients)
        {
            var clientReturn = clients.Select(u => new ClientReturnDto
            {
                Age = u.AppUser.Age,
                Email = u.AppUser.Email,
                UserName = u.AppUser.UserName,
                Name = u.AppUser.Name,
                PhoneNumber = u.AppUser.PhoneNumber,
                Gender = u.AppUser.Gender,
                FatWeight = u.AppUser.FatWeight,
                Height = u.AppUser.Height,
                ImageUrl = u.AppUser.ImageUrl,
                MuscleWeight = u.AppUser.MuscleWeight,
                UserId = u.AppUser.Id,
                Weight = u.AppUser.Weight,
                GoalWeight = u.AppUser.GoalWeight,
                ActivityLevel = u.AppUser.ActivityLevelName,
                MainGoal = u.AppUser.MainGoal,
            }).ToList();
            return clientReturn;
        } 
        public static List<ChatClientReturnDto> MapClientToChatClientReturnDto(List<Client> clients)
        {
            var clientReturn = clients.Select(u => new ChatClientReturnDto
            {
                Id = u.AppUserId,
                Email = u.AppUser.Email,
                User_name = u.AppUser.UserName,
                Image = u.AppUser.ImageUrl,
                Activity_level = u.AppUser.ActivityLevelName,
                MainGoal = u.AppUser.MainGoal,
                About = "",
                Created_At = "",
                Last_Active = "",
                Is_online = false,
                Push_token = "",
                Role = "client"
            }).ToList();
            return clientReturn;
        }

        public static List<WorkoutHistoryReturnDto> MapWorkoutHistoryToWorkoutHistoryReturnDto(List<WorkoutHistory> workOutHostories)
        {
            var workoutPlansReturn = workOutHostories.Select(w => new WorkoutHistoryReturnDto
            {
                Id = w.Id,
                Date = w.Date,
                WorkoutHistoryExercises = w.WorkoutHistoryExercises.Select(we => new WorkoutHistroyExerciseReturnDto
                {
                    Weight = we.Weight,
                    ExerciseGifUrl = we.ExerciseGifUrl,
                    ExerciseName = we.ExerciseName,
                    Sets = we.Sets,
                    Reps = we.Reps,
                    ExerciseId = we.ExerciseId,
                }).ToList()
            }).ToList();
            return workoutPlansReturn;
        }

        public static List<FoodReturnDto> MapFoodToFoodReturnDto(List<Food> Foods, List<FavouriteFood> favouriteFood, List<FoodRating> foodRatings)
        {
            var foodReturnDto = Foods.Select(we => new FoodReturnDto
            {
                Id = we.Id,
                Name = we.Name,
                Calories = we.Calories,
                Protein = we.Protein,
                Fat = we.Fat,
                Carbohydrates = we.Carbohydrates,
                ImageUrl = we.ImageURL,
                IsFavourite = favouriteFood.Any(f => f.FoodId == we.Id),
                Description = we.Description,
                AvgRating = we.AvgRating,
                Fiber = we.Fiber,
                Serving = we.Serving,
                Verified = we.Verified,
                Rate = foodRatings.FirstOrDefault(f => f.FoodId == we.Id)?.Rate 
            }).ToList();
            return foodReturnDto;
        }
        public static List<FoodReturnDto> MapFavouriteFoodToFoodReturnDto(List<Food> Foods, List<FoodRating> foodRatings)
        {
            var foodReturnDto = Foods.Select(we => new FoodReturnDto
            {
                Id = we.Id,
                Name = we.Name,
                Calories = we.Calories,
                Protein = we.Protein,
                Fat = we.Fat,
                Carbohydrates = we.Carbohydrates,
                ImageUrl = we.ImageURL,
                IsFavourite = true,
                Description = we.Description,
                AvgRating = we.AvgRating,
                Fiber = we.Fiber,
                Serving = we.Serving,
                Verified = we.Verified,
                Rate = foodRatings.FirstOrDefault(f => f.FoodId == we.Id)?.Rate
            }).ToList();
            return foodReturnDto;
        }
        public static FoodReturnDto MapFoodToFoodReturnDto(Food food, List<FavouriteFood> favouriteFood, List<FoodRating> foodRatings)
        {
            var foodReturnDto = new FoodReturnDto
            {
                Id = food.Id,
                Name = food.Name,
                Calories = food.Calories,
                Protein = food.Protein,
                Fat = food.Fat,
                Carbohydrates = food.Carbohydrates,
                ImageUrl = food.ImageURL,
                IsFavourite = favouriteFood.Any(f => f.FoodId == food.Id),
                Description = food.Description,
                AvgRating = food.AvgRating,
                Fiber = food.Fiber,
                Serving = food.Serving,
                Verified = food.Verified,
                Rate = foodRatings.FirstOrDefault(f => f.FoodId == food.Id)?.Rate

            };
            return foodReturnDto;
        }
        public static List<DietPlanReturnDto> MapDietPlanToDietPlanReturnDto(List<DietPlan> dietPlans)
        {
            var dietPlansReturn = dietPlans.Select(w => new DietPlanReturnDto
            {
                Id = w.Id,
                Name = w.Name,
                DateCreated = w.DateCreated,
                
                DietPlanFoods = w.DietPlanFoods.Select(we => new DietPlanFoodReturnDto
                {
                    DietPlanFoodId = we.Id,
                    FoodId = we.FoodId,
                    FoodName= we.Food.Name,
                    FoodDescription = we.Food.Description,
                    FoodImageUrl = we.Food.ImageURL,
                    FoodWeight = we.Weight,
                }).ToList()
            }).ToList();
            return dietPlansReturn;
        }
    }
}
