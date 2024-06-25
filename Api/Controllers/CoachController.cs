﻿using Api.ApiResponses;
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
        private readonly ApiResponse _response;
        private readonly IMapper _mapper;

        public CoachController(UserManager<AppUser> userManager, ICoachRepository coachRepository, ApiResponse response, IMapper mapper, IClientRepository clientRepository)
        {
            _userManager = userManager;
            _coachRepository = coachRepository;
            _response = response;
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        [HttpGet("GetAllCoaches", Name = "GetAllCoaches")]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ApiResponse>> GetAllCoaches(int pageSize = 0, int pageNumber = 1)
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

        [HttpPost("AddClientToCoach", Name = "AddClientToCoach")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddClientToCoach(string clientAppUserId)
        {
            try
            {
                var appUser = await _userManager.FindByEmailFromClaimsPrincipalWithCoach(User);
                var coachId = appUser.Coach.CoachId;
                if (coachId == 0)
                {
                    return NotFound(_response.NotFoundResponse("Coach is not found"));
                }
                var clientId = await _clientRepository.GetClientIdFromAppUserIdAsync(clientAppUserId);

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

        [HttpGet("GetCoachClients", Name = "GetCoachClients")]
        [Authorize]
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
    }
}