using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class WorkoutHistoryExercise
    {
        public int WorkoutHistoryId { get; set; }
        public WorkoutHistory WorkoutHistory { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseGifUrl { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
    }
}
