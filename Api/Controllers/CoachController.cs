using Api.ApiResponses;
using Api.DTO;
using Api.DTO.CoachDto;
using Api.Extensions;
using Api.Helper;
using AutoMapper;
using Azure;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICoachRepository _coachRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IWorkoutHistoryRepository _workoutHistoryRepository;
        private readonly ApiResponse _response;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public CoachController(UserManager<AppUser> userManager, ICoachRepository coachRepository, ApiResponse response, IMapper mapper, IClientRepository clientRepository, IPhotoService photoService, IWorkoutHistoryRepository workoutHistoryRepository)
        {
            _userManager = userManager;
            _coachRepository = coachRepository;
            _response = response;
            _mapper = mapper;
            _clientRepository = clientRepository;
            _photoService = photoService;
            _workoutHistoryRepository = workoutHistoryRepository;
        }

        [HttpGet("GetAllCoaches", Name = "GetAllCoaches")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllCoaches()
        {
            try
            {
                var AppUserList = await _coachRepository.GetAllCoachsAsync(includeProperties: "AppUser");
                var CoachesReturn = CustomMappers.MapCoachToCoachReturnDto(AppUserList);

                return Ok(_response.OkResponse(CoachesReturn));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpGet("GetAllCoachesWithPagination", Name = "GetAllCoachesWithPagination")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllCoachesWithPagination(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                var AppUserList = await _coachRepository.GetAllCoachsAsync(includeProperties: "AppUser");
                var CoachesReturn = CustomMappers.MapCoachToCoachReturnDto(AppUserList);
                var paginatedCoaches = Pagination<CoachReturnDto>.Paginate(CoachesReturn, pageNumber, pageSize);

                return Ok(_response.OkResponse(paginatedCoaches));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpGet("GetCoach", Name = "GetCoach")]
        [Authorize(Roles = "coach")]
        public async Task<ActionResult<ApiResponse>> GetCoach()
        {
            try
            {
                var coach = await _userManager.FindByEmailFromClaimsPrincipalWithCoach(User);
                var coachReturn = CustomMappers.MapAppUserToCoachReturnDto(coach);

                return Ok(_response.OkResponse(coachReturn));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPost("AddClientToCoach", Name = "AddClientToCoach")]
        [Authorize(Roles = "coach")]
        public async Task<ActionResult<ApiResponse>> AddClientToCoach(string clientUserName)
        {
            try
            {
                var appUser = await _userManager.FindByEmailFromClaimsPrincipalWithCoach(User);
                var coachId = appUser.Coach.CoachId;
                if (coachId == 0)
                {
                    return NotFound(_response.NotFoundResponse("Coach is not found"));
                }

                var clientId = await _clientRepository.GetClientIdFromAppUserNameAsync(clientUserName);

                if (clientId == 0)
                {
                    return NotFound(_response.NotFoundResponse("Client is not found"));
                }

                if (await _coachRepository.ClientExistInCoachClientsAsync(coachId, clientId))
                {
                    return BadRequest(_response.BadRequestResponse("Client already exists in coach"));
                }

                await _coachRepository.AddClientToCoachAsync(clientId, coachId);
                return Ok(_response.OkResponse("Client added to coach successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpGet("GetUserWorkoutHistory", Name = "GetUserWorkoutHistory")]
        [Authorize(Roles = "coach")]
        public async Task<ActionResult<ApiResponse>> GetUserWorkoutHistory(string userName)
        {
            try
            {
                var appUserCoach = await _userManager.FindByEmailFromClaimsPrincipalWithCoach(User);

                var appUser = await _userManager.FindByNameAsync(userName);

                var clientId = await _clientRepository.GetClientIdFromAppUserNameAsync(userName);

                if (clientId == 0)
                {
                    return NotFound(_response.NotFoundResponse("Client is not found"));
                }

                if (!await _coachRepository.ClientExistInCoachClientsAsync(appUserCoach.Coach.CoachId, clientId))
                {
                    return BadRequest(_response.BadRequestResponse("this isn't your client"));
                }

                List<WorkoutHistory> userWorkoutHistory = await _workoutHistoryRepository.GetAllWorkoutHistoriesAsync(appUser.Id);

                if(userWorkoutHistory == null)
                {
                    return Ok(_response.OkResponse("No workout history found for this user"));
                }

                var result = CustomMappers.MapWorkoutHistoryToWorkoutHistoryReturnDto(userWorkoutHistory);
                return Ok(_response.OkResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpGet("GetCoachClients", Name = "GetCoachClients")]
        [Authorize(Roles = "coach")]
        public async Task<ActionResult<ApiResponse>> GetCoachClients()
        {
            try
            {
                var appUser = await _userManager.FindByEmailFromClaimsPrincipalWithCoach(User);
                var coachId = appUser.Coach.CoachId;
                if (coachId == 0)
                {
                    return NotFound(_response.NotFoundResponse("Coach is not found"));
                }

                var coachClients = await _coachRepository.GetAllCoachClientsAsync(coachId);
                var coachClientsReturn = CustomMappers.MapClientToClientReturnDto(coachClients);
                return Ok(_response.OkResponse(coachClientsReturn));
            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPost("AddCv", Name = "AddCv")]
        [Authorize(Roles = "coach")]
        public async Task<ActionResult<ApiResponse>> AddCv(IFormFile cv)
        {
            try
            {
                var appUser = await _userManager.FindByEmailFromClaimsPrincipalWithCoach(User);
                var coachId = appUser.Coach.CoachId;
                if (coachId == 0)
                {
                    return NotFound(_response.NotFoundResponse("Coach is not found"));
                }

                var coach = await _coachRepository.GetAsync(c => c.CoachId == coachId);
                var uploadResult = _photoService.UploadPdfAsync(cv);
                coach.CvUrl = uploadResult.Result.Url.ToString();
                await _coachRepository.UpdateAsync(coach);
                return Ok(_response.OkResponse($"cv uploaded{coach.CvUrl}"));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }

        [HttpPut("UpdateCv", Name = "UpdateCv")]
        [Authorize(Roles = "coach")]
        public async Task<ActionResult<ApiResponse>> UpdateCv(IFormFile cv)
        {
            try
            {
                var appUser = await _userManager.FindByEmailFromClaimsPrincipalWithCoach(User);
                if (appUser.Coach.CoachId == 0)
                {
                    return NotFound(_response.NotFoundResponse("Coach is not found"));
                }
                var coach = appUser.Coach;
                var uploadResult = _photoService.UploadPdfAsync(cv);
                coach.CvUrl = uploadResult.Result.Url.ToString();
                await _coachRepository.UpdateAsync(coach);
                return Ok(_response.OkResponse($"cv uploaded{coach.CvUrl}"));

            }
            catch (Exception ex)
            {
                return BadRequest(_response.BadRequestResponse(ex.Message));
            }
        }
    }
}