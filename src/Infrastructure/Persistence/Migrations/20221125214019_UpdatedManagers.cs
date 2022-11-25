using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class UpdatedManagers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerClaims_Managers_ManagerId",
                table: "ManagerClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerRole_Managers_ManagersUserId",
                table: "ManagerRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Managers");

            migrationBuilder.RenameColumn(
                name: "ManagersUserId",
                table: "ManagerRole",
                newName: "ManagersId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Managers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerClaims_Managers_ManagerId",
                table: "ManagerClaims",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerRole_Managers_ManagersId",
                table: "ManagerRole",
                column: "ManagersId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerClaims_Managers_ManagerId",
                table: "ManagerClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_ManagerRole_Managers_ManagersId",
                table: "ManagerRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

            migrationBuilder.RenameColumn(
                name: "ManagersId",
                table: "ManagerRole",
                newName: "ManagersUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Managers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerClaims_Managers_ManagerId",
                table: "ManagerClaims",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerRole_Managers_ManagersUserId",
                table: "ManagerRole",
                column: "ManagersUserId",
                principalTable: "Managers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
