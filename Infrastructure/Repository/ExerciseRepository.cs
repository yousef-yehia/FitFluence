using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _appDb;
        public ExerciseRepository(AppDbContext appDb, AppDbContext appDbContext) 
        {
            _appDb = appDbContext;
        }
        public async Task CreateExerciseAsync(Exercise exercise)
        {
            await _appDb.Exercises.AddAsync(exercise);
            await SaveAsync();
        }

        public async Task<bool> DeesExerciseExistsAsync(int id)
        {
            return await _appDb.Exercises.AnyAsync(e=> e.Id == id);
        }

        public async Task DeleteAllExercisesAsync()
        {
            _appDb.Exercises.RemoveRange(_appDb.Exercises);
            await SaveAsync();
        }

        public async Task<List<Exercise>> GetAllAsync(string search = null)
        {
            var exercises = await _appDb.Exercises.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                exercises = exercises.Where(f => f.Name.Contains(search) || f.Description.Contains(search)).ToList();
            }

            exercises = exercises.OrderBy(f => f.Name).ToList();
           

            return exercises;
        }
        public async Task<List<Exercise>> GetAllByMuscleAsync(int muscleId, string search = null)
        {
            var exercises = await _appDb.Exercises.Where(e=> e.MuscleId == muscleId).ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                exercises = exercises.Where(f => f.Name.Contains(search) || f.Description.Contains(search)).ToList();
            }

            exercises = exercises.OrderBy(f => f.Name).ToList();
           

            return exercises;
        }  
        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            var exercises = await _appDb.Exercises.FirstOrDefaultAsync(e=> e.Id == id);
            return exercises;
        }


        public async Task UpdateAsync(Exercise exercise)
        {
            _appDb.Update(exercise);
            await SaveAsync();
        }
        public async Task DeleteAsync(Exercise exercise)
        {
            _appDb.Exercises.Remove(exercise);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _appDb.SaveChangesAsync();
        }
    }
}
