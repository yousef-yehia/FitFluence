using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addAttributesToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Coachs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Coachs");

            migrationBuilder.DropColumn(
                name: "FatWeight",
                table: "Coachs");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Coachs");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Coachs");

            migrationBuilder.DropColumn(
                name: "MuscleWeight",
                table: "Coachs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Coachs");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Coachs");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "FatWeight",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "MuscleWeight",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "FatWeight",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MuscleWeight",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FatWeight",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MuscleWeight",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Coachs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Coachs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "FatWeight",
                table: "Coachs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Coachs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "Coachs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MuscleWeight",
                table: "Coachs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Coachs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Coachs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "FatWeight",
                table: "Clients",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "Clients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MuscleWeight",
                table: "Clients",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Clients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
