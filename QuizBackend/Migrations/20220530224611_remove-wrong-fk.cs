using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizBackend.Migrations
{
    public partial class removewrongfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Quiz_QuizId",
                table: "Quiz");

            migrationBuilder.DropIndex(
                name: "IX_Quiz_QuizId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Quiz");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "Quiz",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_QuizId",
                table: "Quiz",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Quiz_QuizId",
                table: "Quiz",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id");
        }
    }
}
