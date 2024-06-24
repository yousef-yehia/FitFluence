using Api.DTO.ExerciseDto;
using Api.DTO.WorkoutPlanDto;
using Core.Models;

namespace Api.Helper
{
    public static class CustomMappers
    {
        public static List<WorkoutPlanReturnDto> MapWorkoutplanToWorkoutPLanRetirnDto(List<WorkoutPlan> workOutplans)
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
                MuscleName = e.Muscle.Name,
                Description = e.Description,
                FocusAreaUrl = e.FocusAreaUrl,
            }).ToList();
            return exerciseReturn;
        }
    }
}
