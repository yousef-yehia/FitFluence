using Core.Models;

namespace Api.DTO.WorkoutPlanDto
{
    public class WorkoutPlanReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdedd { get; set; }
        public IEnumerable<WorkoutPlanExerciseDto> WorkOutPlanExercises { get; set; } 
    }
}
