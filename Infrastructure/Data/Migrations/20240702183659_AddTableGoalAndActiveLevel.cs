using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableGoalAndActiveLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGoals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Goals",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Goals");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Goals",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "MonthlyPrice",
                table: "Coachs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ActivityLevelName",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GoalWeight",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainGoal",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RecommendedCalories",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Goals",
                table: "Goals",
                column: "Name");

            migrationBuilder.CreateTable(
                name: "ActivityLevels",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLevels", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ActivityLevelName",
                table: "AspNetUsers",
                column: "ActivityLevelName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MainGoal",
                table: "AspNetUsers",
                column: "MainGoal");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ActivityLevels_ActivityLevelName",
                table: "AspNetUsers",
                column: "ActivityLevelName",
                principalTable: "ActivityLevels",
                principalColumn: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Goals_MainGoal",
                table: "AspNetUsers",
                column: "MainGoal",
                principalTable: "Goals",
                principalColumn: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ActivityLevels_ActivityLevelName",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Goals_MainGoal",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ActivityLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Goals",
                table: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ActivityLevelName",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MainGoal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MonthlyPrice",
                table: "Coachs");

            migrationBuilder.DropColumn(
                name: "ActivityLevelName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GoalWeight",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MainGoal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RecommendedCalories",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Goals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Goals",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Goals",
                table: "Goals",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserGoals",
                columns: table => new
                {
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GoalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGoals", x => new { x.AppUserId, x.GoalId });
                    table.ForeignKey(
                        name: "FK_UserGoals_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGoals_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGoals_GoalId",
                table: "UserGoals",
                column: "GoalId");
        }
    }
}
