using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addIdToDietPlanFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DietPlanFoods",
                table: "DietPlanFoods");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DietPlanFoods",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DietPlanFoods",
                table: "DietPlanFoods",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DietPlanFoods_DietPlanId",
                table: "DietPlanFoods",
                column: "DietPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DietPlanFoods",
                table: "DietPlanFoods");

            migrationBuilder.DropIndex(
                name: "IX_DietPlanFoods_DietPlanId",
                table: "DietPlanFoods");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DietPlanFoods");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DietPlanFoods",
                table: "DietPlanFoods",
                columns: new[] { "DietPlanId", "FoodId" });
        }
    }
}
