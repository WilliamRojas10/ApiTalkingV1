using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddLevelToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_post_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_user_UserId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "user");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "comment",
                newName: "IX_comment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "comment",
                newName: "IX_comment_PostId");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_comment",
                table: "comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_comment_post_PostId",
                table: "comment",
                column: "PostId",
                principalTable: "post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_user_UserId",
                table: "comment",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comment_post_PostId",
                table: "comment");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_user_UserId",
                table: "comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comment",
                table: "comment");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "course");

            migrationBuilder.RenameTable(
                name: "comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_comment_UserId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_comment_PostId",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "user",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_post_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_user_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
