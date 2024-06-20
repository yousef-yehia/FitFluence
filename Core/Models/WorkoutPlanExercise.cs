namespace Core.Models
{
    public class WorkoutPlanExercise
    {
        public int WorkoutPlanId { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseGifUrl { get; set;}
    }
}