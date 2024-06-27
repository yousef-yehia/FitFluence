using Api.DTO.CoachDto;
using Api.DTO.ExerciseDto;
using Api.DTO.WorkoutHistoryDto;
using Api.DTO.WorkoutPlanDto;
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
                Name = u.AppUser.Name,
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
                    NumberOfReps = we.NumberOfReps,
                    ExerciseId = we.ExerciseId,
                }).ToList()
            }).ToList();
            return workoutPlansReturn;
        }
    }
}
