using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class WorkoutPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdedd { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<WorkoutPlanExercise> WorkOutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();

    }
}
