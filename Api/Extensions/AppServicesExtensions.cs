using System.Text;
using System.Text.Json.Serialization;
using Api.ApiResponses;
using Api.Helper;
using Core.Interfaces;
using Core.Models;
using FitFluence.Repository;
using Infrastructure.Data;
using Infrastructure.Data.Seed;
using Infrastructure.Helper;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Api.Extensions
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddMemoryCache();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "  stack-hup APIs", Version = "v1" });

                // Add JWT Authentication support in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            //services.AddScoped<IAppDbContext>(sp =>sp.GetRequiredService<AppDbContext>());
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<FoodSeeder>();
            services.AddTransient<RolesSeeder>();

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IPhotoService, PhotoService>();
            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IGoalRepository, GoalRepository>();
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IFavouriteFoodRepository, FavouriteFoodRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserDailyFoodsRepository, UserDailyFoodsRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IMuscleRepository, MuscleRepository>();
            services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
            services.AddScoped<IWorkoutHistoryRepository, WorkoutHistoryRepository>();
            services.AddScoped<ICoachRepository, CoachRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IDietPlanRepository, DietPlanRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<ApiResponse>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            services.AddHttpClient(); // Register IHttpClientFactory

            services.Configure<SmtpSetting>(config.GetSection("SMTP"));
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddAutoMapper(typeof(MappingProfiles));


            services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = config["Token:Issuer"],
                    //ValidAudience = builder.Configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]!))
                };
            });

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
            //})
            //.AddFacebook(options =>
            //{
            //    options.AppId = config["Facebook:AppId"];
            //    options.AppSecret = config["Facebook:Secret"];
            //});

            services.AddCors();

            return services;

        }
    }
}