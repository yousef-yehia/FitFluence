using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixCoachAndClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachsAndClients_Clients_ClientId",
                table: "CoachsAndClients");

            migrationBuilder.AddForeignKey(
                name: "FK_CoachsAndClients_Clients_ClientId",
                table: "CoachsAndClients",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachsAndClients_Clients_ClientId",
                table: "CoachsAndClients");

            migrationBuilder.AddForeignKey(
                name: "FK_CoachsAndClients_Clients_ClientId",
                table: "CoachsAndClients",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
