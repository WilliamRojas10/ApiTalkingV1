using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class refactorizacionFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Users_UserId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Files_FileId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Posts_PostId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Users_UserId",
                table: "Reactions");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "FilesTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reactions",
                table: "Reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "course");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "Reactions",
                newName: "reaction");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "post");

            migrationBuilder.RenameIndex(
                name: "IX_Course_UserId",
                table: "course",
                newName: "IX_course_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reactions_UserId",
                table: "reaction",
                newName: "IX_reaction_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reactions_PostId",
                table: "reaction",
                newName: "IX_reaction_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserId",
                table: "post",
                newName: "IX_post_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_FileId",
                table: "post",
                newName: "IX_post_FileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_course",
                table: "course",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reaction",
                table: "reaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_post",
                table: "post",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "file_type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeFile = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_type", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "file",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file", x => x.Id);
                    table.ForeignKey(
                        name: "FK_file_file_type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "file_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_file_TypeId",
                table: "file",
                column: "TypeId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_course_user_UserId",
                table: "course",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_post_file_FileId",
                table: "post",
                column: "FileId",
                principalTable: "file",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_post_user_UserId",
                table: "post",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reaction_post_PostId",
                table: "reaction",
                column: "PostId",
                principalTable: "post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reaction_user_UserId",
                table: "reaction",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_post_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_user_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_course_user_UserId",
                table: "course");

            migrationBuilder.DropForeignKey(
                name: "FK_post_file_FileId",
                table: "post");

            migrationBuilder.DropForeignKey(
                name: "FK_post_user_UserId",
                table: "post");

            migrationBuilder.DropForeignKey(
                name: "FK_reaction_post_PostId",
                table: "reaction");

            migrationBuilder.DropForeignKey(
                name: "FK_reaction_user_UserId",
                table: "reaction");

            migrationBuilder.DropTable(
                name: "file");

            migrationBuilder.DropTable(
                name: "file_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_course",
                table: "course");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reaction",
                table: "reaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_post",
                table: "post");

            migrationBuilder.RenameTable(
                name: "course",
                newName: "Course");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "reaction",
                newName: "Reactions");

            migrationBuilder.RenameTable(
                name: "post",
                newName: "Posts");

            migrationBuilder.RenameIndex(
                name: "IX_course_UserId",
                table: "Course",
                newName: "IX_Course_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_reaction_UserId",
                table: "Reactions",
                newName: "IX_Reactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_reaction_PostId",
                table: "Reactions",
                newName: "IX_Reactions_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_post_UserId",
                table: "Posts",
                newName: "IX_Posts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_post_FileId",
                table: "Posts",
                newName: "IX_Posts_FileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reactions",
                table: "Reactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FilesTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeFile = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    EntityStatus = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Path = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_FilesTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FilesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Files_TypeId",
                table: "Files",
                column: "TypeId");

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
                name: "FK_Course_Users_UserId",
                table: "Course",
                column: "UserId",
                principalTable: "Users",
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
                name: "FK_Reactions_Posts_PostId",
                table: "Reactions",
                column: "PostId",
                principalTable: "Posts",
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
    }
}
