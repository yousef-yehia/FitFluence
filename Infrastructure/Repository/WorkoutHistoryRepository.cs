using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class WorkoutHistoryRepository : IWorkoutHistoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public WorkoutHistoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddExerciseToWorkoutHisterAsync(WorkoutHistory workoutHistory, Exercise exercise, int numberOfReps, double weight)
        {
            await _appDbContext.WorkoutHistoryExercises.AddAsync(new WorkoutHistoryExercise
            {
                WorkoutHistoryId = workoutHistory.Id,
                ExerciseId = exercise.Id,
                ExerciseName = exercise.Name,
                NumberOfReps = numberOfReps,
                Weight = weight,
                ExerciseGifUrl = exercise.GifUrl,
            });

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<WorkoutHistory> CreateWorkoutHistoryAsync(string appUserId)
        {
            await _appDbContext.WorkoutHistories.AddAsync(new WorkoutHistory
            {
                AppUserId = appUserId,
                Date = DateTime.UtcNow.Date,
            });

            await _appDbContext.SaveChangesAsync();

            return await _appDbContext.WorkoutHistories.FirstOrDefaultAsync(w=> w.Date == DateTime.UtcNow.Date);
        }

        public async Task DeleteWorkoutHistoryAsync(string appUserId)
        {
            var oldestWorkoutHistory = _appDbContext.WorkoutHistories
                .Where(w => w.AppUserId == appUserId)
                .AsNoTracking()
                .OrderBy(w => w.Date)
                .FirstOrDefault();

            _appDbContext.Remove(oldestWorkoutHistory);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<WorkoutHistory>> GetAllWorkoutHistoriesAsync(string appUserId)
        {
            return await _appDbContext.WorkoutHistories
                .Where(w => w.AppUserId == appUserId)
                .Include(w => w.WorkoutHistoryExercises)
                .AsNoTracking()
                .ToListAsync();
        }
        public bool DoesWorkHistoryDateExistsAsync(AppUser appUser ,DateTime date)
        {
            return appUser.WorkoutHistories.Any(w => w.Date == date);
        }
        public int GetWorkHistoriesCount(AppUser appUser)
        {
            return appUser.WorkoutHistories.Count();
        } 
        public WorkoutHistory GetWorkHistoriesByDate(AppUser appUser, DateTime date)
        {
            return appUser.WorkoutHistories.Where(w=> w.Date == date).FirstOrDefault();
        }

        public Task RemoveExerciseFromWorkoutHistoryAsync(WorkoutHistory workoutHistory, Exercise exercise)
        {
            throw new NotImplementedException();
        }

    }
}
