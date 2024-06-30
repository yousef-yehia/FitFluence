namespace Api.DTO.WorkoutHistoryDto
{
    public class AddExerciseToWorkoutHistoryDto
    {
        public int ExerciseId { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
    }
}
