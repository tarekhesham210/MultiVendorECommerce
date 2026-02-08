using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermissionBasedAuz.Migrations
{
    public partial class addApplicationRoleWithUserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetRoles");
        }
    }
}
