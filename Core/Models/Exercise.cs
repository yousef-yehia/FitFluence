using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Sets { get; set; }
        public string GifUrl { get; set; }
        public string Description { get; set; }
        public string FocusAreaUrl {  get; set; }
        public int MuscleId { get; set; }
        public Muscle Muscle{ get; set;}
        public List<WorkoutPlanExercise> workoutPlanExercises { get; set; }


    }
}
