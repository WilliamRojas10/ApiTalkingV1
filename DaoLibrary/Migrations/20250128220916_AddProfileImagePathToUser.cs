using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileImagePathToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_FileType_TypeId",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileType",
                table: "FileType");

            migrationBuilder.RenameTable(
                name: "FileType",
                newName: "FilesTypes");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Reactions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdPost",
                table: "Reactions",
                newName: "PostStatus");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Posts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdFile",
                table: "Posts",
                newName: "FileId");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdPost",
                table: "Comments",
                newName: "PostId");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDateTime",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilesTypes",
                table: "FilesTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId",
                table: "Reactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FileId",
                table: "Posts",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_FilesTypes_TypeId",
                table: "Files",
                column: "TypeId",
                principalTable: "FilesTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Files_FileId",
                table: "Posts",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Users_UserId",
                table: "Reactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_FilesTypes_TypeId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Files_FileId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Users_UserId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_UserId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Posts_FileId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilesTypes",
                table: "FilesTypes");

            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "FilesTypes",
                newName: "FileType");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reactions",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "PostStatus",
                table: "Reactions",
                newName: "IdPost");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Posts",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Posts",
                newName: "IdFile");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Comments",
                newName: "IdPost");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDateTime",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileType",
                table: "FileType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_FileType_TypeId",
                table: "Files",
                column: "TypeId",
                principalTable: "FileType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
