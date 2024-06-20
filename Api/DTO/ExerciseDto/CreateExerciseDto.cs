namespace Api.DTO.ExerciseDto
{
    public class CreateExerciseDto
    {

        public required string Name { get; set; }
        public required string Sets { get; set; }
        public required IFormFile Gif { get; set; }
        public required string Description { get; set; }
        public required IFormFile FocusArea { get; set; }
        public int MuscleId { get; set; }
    }
}
