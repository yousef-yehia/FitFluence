using Application.Abstractions.Messaging;
using Application.DTO.FoodDto;

namespace Application.Foods.Create
{
    public record CreateFoodCommand(CreateFoodDto createFoodDto) : ICommand<FoodDto>;
}
