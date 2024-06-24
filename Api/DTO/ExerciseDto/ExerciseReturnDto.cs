namespace Api.DTO.ExerciseDto
{
    public class ExerciseReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Sets { get; set; }
        public string GifUrl { get; set; }
        public string Description { get; set; }
        public string FocusAreaUrl { get; set; }
        public string MuscleName { get; set; } 
    }
}
