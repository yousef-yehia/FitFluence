namespace Api.DTO.DietPlanDto
{
    public class DietPlanReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public List<DietPlanFoodReturnDto> DietPlanFoods { get; set; } = new List<DietPlanFoodReturnDto>(); 
    }
}
