using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevFlow.Projects.Migrations
{
    /// <inheritdoc />
    public partial class ProjectModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "CurrentState",
                table: "Tickets",
                newName: "State");

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);           

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Tickets");            

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Tickets",
                newName: "CurrentState");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
