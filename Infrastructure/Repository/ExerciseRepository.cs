using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _appDb;
        private readonly IPhotoService _photoService;
        public ExerciseRepository(AppDbContext appDbContext, IPhotoService photoService)
        {
            _appDb = appDbContext;
            _photoService = photoService;
        }
        public async Task<Exercise> CreateExerciseAsync(Exercise exercise, IFormFile gif, IFormFile focusArea)
        {
            var gifResponse = await _photoService.AddExercisePhotoAsync(gif);
            var focusAreaResponse = await _photoService.AddExercisePhotoAsync(focusArea);

            exercise.GifUrl = gifResponse.Url.ToString();
            exercise.FocusAreaUrl = focusAreaResponse.Url.ToString();
            await _appDb.Exercises.AddAsync(exercise);
            await SaveAsync();
            return exercise;
        }

        public async Task<bool> DeesExerciseExistsAsync(int id)
        {
            return await _appDb.Exercises.AnyAsync(e => e.Id == id);
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
            var exercises = await _appDb.Exercises.Where(e => e.MuscleId == muscleId).ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                exercises = exercises.Where(f => f.Name.Contains(search) || f.Description.Contains(search)).ToList();
            }

            exercises = exercises.OrderBy(f => f.Name).ToList();


            return exercises;
        }
        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            var exercises = await _appDb.Exercises.FirstOrDefaultAsync(e => e.Id == id);
            return exercises;
        }
        public async Task<Exercise> GetExerciseByNameAsync(string name)
        {
            var exercises = await _appDb.Exercises.FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower());
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
