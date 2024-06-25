using Core.Models;

namespace Api.DTO.WorkoutHistoryDto
{
    public class WorkoutHistoryReturnDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<WorkoutHistroyExerciseReturnDto> WorkoutHistoryExercises { get; set; }
    }
}
