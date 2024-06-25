using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CoachAndClient
    {
        public int CoachId { get; set; }    
        public Coach Coach { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
