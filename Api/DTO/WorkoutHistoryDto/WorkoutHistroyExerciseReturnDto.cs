﻿namespace Api.DTO.WorkoutHistoryDto
{
    public class WorkoutHistroyExerciseReturnDto
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseGifUrl { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
    }
}
