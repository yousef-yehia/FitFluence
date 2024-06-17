using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IExerciseRepository 
    {
        public Task DeleteAllExercisesAsync();
        public Task CreateExerciseAsync(Exercise exercise);
        public Task<bool> DeesExerciseExistsAsync(int id);
        public Task UpdateAsync(Exercise exercise);
        public Task DeleteAsync(Exercise exercise);

        public Task<Exercise> GetExerciseByIdAsync(int id);

        public Task<List<Exercise>> GetAllByMuscleAsync(int muscleId, string? search = null);
        public Task<List<Exercise>> GetAllAsync(string? search = null);
    }
}
