using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IMuscleRepository : IRepository<Muscle>
    {
        public Task<Muscle> CreateMuscleAsync(Muscle muscle, IFormFile photo);
    }
}
