using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IWorkoutHistoryRepository
    {
        public Task<WorkoutHistory> CreateWorkoutHistoryAsync(string appUserId);
        public Task AddExerciseToWorkoutHisterAsync(WorkoutHistory workoutHistory, Exercise exercise, int numberOfReps, double weight);
        public Task RemoveExerciseFromWorkoutHistoryAsync(WorkoutHistory workoutHistory, Exercise exercise);
        public Task DeleteWorkoutHistoryAsync(string appUserId);
        public Task<List<WorkoutHistory>> GetAllWorkoutHistoriesAsync(string appUserId);
        public bool DoesWorkHistoryDateExistsAsync(AppUser appUser, DateTime date);
        public int GetWorkHistoriesCount(AppUser appUser);
        public WorkoutHistory GetWorkHistoriesByDate(AppUser appUser, DateTime date);

    }
}
