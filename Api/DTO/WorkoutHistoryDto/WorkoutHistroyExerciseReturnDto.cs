namespace Api.DTO.WorkoutHistoryDto
{
    public class WorkoutHistroyExerciseReturnDto
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseGifUrl { get; set; }
        public int NumberOfReps { get; set; }
        public double Weight { get; set; }
    }
}
