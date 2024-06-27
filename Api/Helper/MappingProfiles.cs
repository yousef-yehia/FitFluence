using AutoMapper;
using Core.Models;
using Api.DTO;
using Api.DTO.FoodDto;
using Api.DTO.ExerciseDto;
using Api.DTO.WorkoutPlanDto;
using Api.DTO.AuthDto;
using Api.DTO.MuscleDto;


namespace Api.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterRequestDto, AppUser>()
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                        .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();


            CreateMap<CoachRegisterRequestDto, AppUser>()
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                        .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();

            CreateMap<UpdateClientDto, AppUser>()
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                        .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();

            CreateMap<ClientRegisterRequestDto, AppUser>()
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                        .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();

            CreateMap<ClientRegisterRequestDto, AppUser>().ReverseMap();
            CreateMap<CoachRegisterRequestDto, AppUser>().ReverseMap();
            CreateMap<UpdateClientDto, AppUser>().ReverseMap();
            CreateMap<UpdateCoachDto, AppUser>().ReverseMap();

            CreateMap<CreateGoalDto, Goal>().ReverseMap();
            CreateMap<GoalDto, Goal>().ReverseMap();
            CreateMap<FoodReturnDto, Food>().ReverseMap();
            CreateMap<CreateFoodDto, Food>().ReverseMap();
            CreateMap<CreateFoodDto, Food>().ReverseMap();

            //CreateMap<UserDailyFoods, UserDailyFoodDto>().ReverseMap();
            CreateMap<UserDailyFood, Food>().ReverseMap();

            CreateMap<ExerciseReturnDto, Exercise>().ReverseMap();
            CreateMap<CreateExerciseDto, Exercise>().ReverseMap();

            CreateMap<MuscleReturnDto, Muscle>().ReverseMap();

            CreateMap<ClientRegisterRequestDto, Client>().ReverseMap();
            CreateMap<CoachRegisterRequestDto, Coach>().ReverseMap();
            CreateMap<UpdateClientDto, Client>().ReverseMap();
            CreateMap<UpdateCoachDto, Coach>().ReverseMap();
        }      
    }
}

