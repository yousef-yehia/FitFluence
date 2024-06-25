using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class WorkoutHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<WorkoutHistoryExercise> WorkoutHistoryExercises { get; set; } = new List<WorkoutHistoryExercise>();
    }
}
