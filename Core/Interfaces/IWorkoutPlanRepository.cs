using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IWorkoutPlanRepository : IRepository<WorkoutPlan>
    {
        public Task<List<WorkoutPlan>> GetAllWorkoutPlansWithExercisesAsync(AppUser appUser, string? search = null, string? orderBy = null);
        public Task UpdateWorkoutPlanAsync(int workoutPlanId, string newWorkoutPlanName);
        public Task AddExerciseToWorkoutPLanAsync(int workoutPlanId, Exercise exercise, int numberOFReps, double weight);
        public Task DeleteExerciseFromWorkoutPlanAsync(int workoutPlanId, int exerciseId);

    }
}
