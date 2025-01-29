using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class migrationIn2025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserStatus",
                table: "Users",
                newName: "UserType");

            migrationBuilder.RenameColumn(
                name: "PostStatus",
                table: "Reactions",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "PostStatus",
                table: "Posts",
                newName: "EntityStatus");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Comments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CommentStatus",
                table: "Comments",
                newName: "EntityStatus");

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Reactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId",
                table: "Reactions",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Posts_PostId",
                table: "Reactions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Posts_PostId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_PostId",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users",
                newName: "UserStatus");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Reactions",
                newName: "PostStatus");

            migrationBuilder.RenameColumn(
                name: "EntityStatus",
                table: "Posts",
                newName: "PostStatus");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "EntityStatus",
                table: "Comments",
                newName: "CommentStatus");
        }
    }
}
