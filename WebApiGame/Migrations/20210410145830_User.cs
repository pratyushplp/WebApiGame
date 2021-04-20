using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiGame.Migrations
{
    public partial class User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "strength",
                table: "Characters",
                newName: "Strength");

            migrationBuilder.RenameColumn(
                name: "rpgclass",
                table: "Characters",
                newName: "Rpgclass");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Characters",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                table: "Characters",
                newName: "Intelligence");

            migrationBuilder.RenameColumn(
                name: "hitPoints",
                table: "Characters",
                newName: "HitPoints");

            migrationBuilder.RenameColumn(
                name: "defence",
                table: "Characters",
                newName: "Defence");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Characters",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_UserId",
                table: "Characters",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_UserId",
                table: "Characters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_UserId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Characters_UserId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Strength",
                table: "Characters",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "Rpgclass",
                table: "Characters",
                newName: "rpgclass");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Characters",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Intelligence",
                table: "Characters",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "HitPoints",
                table: "Characters",
                newName: "hitPoints");

            migrationBuilder.RenameColumn(
                name: "Defence",
                table: "Characters",
                newName: "defence");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Characters",
                newName: "id");
        }
    }
}
