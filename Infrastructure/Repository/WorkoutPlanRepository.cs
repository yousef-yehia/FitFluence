using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class WorkoutPlanRepository : Repository<WorkoutPlan>, IWorkoutPlanRepository
    {
        private readonly AppDbContext _appDbContext;
        public WorkoutPlanRepository(AppDbContext appDb) : base(appDb)
        {
            _appDbContext = appDb;
        }
        public async Task<List<WorkoutPlan>> GetAllWorkoutPlansWithExercisesAsync(AppUser appUser, string? search = null, string? orderBy = null)
        {
            var workoutPlan = await _appDbContext.WorkoutPlans
                .Where(w => w.AppUserId == appUser.Id)
                .Include(w => w.WorkOutPlanExercises)
                .ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                workoutPlan = workoutPlan.Where(f => f.Name.Contains(search)).ToList();
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy)
                {
                    case "name":
                        workoutPlan = workoutPlan.OrderBy(f => f.Name).ToList();
                        break;
                    default:
                        workoutPlan = workoutPlan.OrderBy(f => f.DateAdedd).ToList();
                        break;
                }
            }
            return workoutPlan;
        }
        public async Task AddExerciseToWorkoutPLanAsync(int workoutPlanId, Exercise exercise, int numberOFReps, double weight)
        {

            _appDbContext.WorkoutPlanExercises.Add(new WorkoutPlanExercise
            {
                ExerciseId = exercise.Id,
                ExerciseName = exercise.Name,
                ExerciseGifUrl = exercise.GifUrl,
                WorkoutPlanId = workoutPlanId,
                NumberOfReps = numberOFReps,
                Weight = weight
            });
            await _appDbContext.SaveChangesAsync();
        } 

        public async Task UpdateWorkoutPlanAsync(int workoutPlanId, string newWorkoutPlanName)
        {
            var workoutPlan = await _appDbContext.WorkoutPlans.FirstOrDefaultAsync(w => w.Id == workoutPlanId);
            workoutPlan.Name = newWorkoutPlanName;
            _appDbContext.Update(workoutPlan); 
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteExerciseFromWorkoutPlanAsync(int workoutPlanId, int exercisId)
        {
            var exercise = await _appDbContext.WorkoutPlanExercises.FirstOrDefaultAsync(w => w.WorkoutPlanId == workoutPlanId && w.ExerciseId == exercisId);
            _appDbContext.WorkoutPlanExercises.Remove(exercise);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
