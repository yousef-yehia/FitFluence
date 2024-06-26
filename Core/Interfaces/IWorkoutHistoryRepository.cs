﻿using System;
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
        public Task AddExerciseToWorkoutHisterAsync(WorkoutHistory workoutHistory, Exercise exercise,int sets, int reps, double weight);
        public Task RemoveExerciseFromWorkoutHistoryAsync(WorkoutHistory workoutHistory, Exercise exercise);
        public Task DeleteWorkoutHistoryAsync(string appUserId);
        public Task<List<WorkoutHistory>> GetAllWorkoutHistoriesAsync(string appUserId);
        public bool DoesWorkHistoryDateExistsAsync(AppUser appUser, DateTime date);
        public bool IsExerciseInWorkoutHistory(Exercise exercise, WorkoutHistory workoutHistory);
        public int GetWorkHistoriesCount(AppUser appUser);
        public Task<WorkoutHistory> GetWorkoutHistoryByDateAsync(string appUserId, DateTime date);

    }
}
