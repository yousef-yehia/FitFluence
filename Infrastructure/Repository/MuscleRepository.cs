using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repository
{
    public class MuscleRepository : Repository<Muscle>, IMuscleRepository
    {
        private readonly AppDbContext _appDbContext; 
        private readonly IPhotoService _photoService;
        public MuscleRepository(AppDbContext appDb, IPhotoService photoService) : base(appDb)
        {
            _appDbContext = appDb;
            _photoService = photoService;
        }
        public async Task<Muscle> CreateMuscleAsync(Muscle muscle, IFormFile photo)
        {
            var photoResponse = await _photoService.AddMusclePhotoAsync(photo);

            muscle.ImageUrl = photoResponse.Url.ToString();
            await _appDbContext.Muscles.AddAsync(muscle);
            await SaveAsync();
            return muscle;
        }
    }
}
