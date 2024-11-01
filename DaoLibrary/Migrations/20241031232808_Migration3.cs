using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class Migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "Posts",
                newName: "RegistrationDateTime");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "Comments",
                newName: "RegistrationDateTime");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "Users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDateTime",
                table: "Users",
                type: "datetime(6)",
                nullable: false)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDateTime",
                table: "Reactions",
                type: "datetime(6)",
                nullable: false)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FileType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeFile = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileType", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Files_TypeId",
                table: "Files",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_FileType_TypeId",
                table: "Files",
                column: "TypeId",
                principalTable: "FileType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_FileType_TypeId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "FileType");

            migrationBuilder.DropIndex(
                name: "IX_Files_TypeId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "RegistrationDateTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RegistrationDateTime",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "RegistrationDateTime",
                table: "Posts",
                newName: "RegistrationDate");

            migrationBuilder.RenameColumn(
                name: "RegistrationDateTime",
                table: "Comments",
                newName: "RegistrationDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Files",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
