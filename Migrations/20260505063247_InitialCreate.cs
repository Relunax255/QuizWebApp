using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "CompletedQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompletedQuestions_CategoryId",
                table: "CompletedQuestions",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedQuestions_Categories_CategoryId",
                table: "CompletedQuestions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedQuestions_Categories_CategoryId",
                table: "CompletedQuestions");

            migrationBuilder.DropIndex(
                name: "IX_CompletedQuestions_CategoryId",
                table: "CompletedQuestions");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CompletedQuestions");
        }
    }
}
