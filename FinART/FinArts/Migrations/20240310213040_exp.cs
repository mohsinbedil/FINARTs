using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FineArt.Migrations
{
    public partial class exp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExhibitionPostings_Submissions_Id",
                table: "ExhibitionPostings");

            migrationBuilder.DropIndex(
                name: "IX_ExhibitionPostings_Id",
                table: "ExhibitionPostings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExhibitionPostings");

            migrationBuilder.AddColumn<string>(
                name: "EPIMG",
                table: "ExhibitionPostings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPIMG",
                table: "ExhibitionPostings");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ExhibitionPostings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExhibitionPostings_Id",
                table: "ExhibitionPostings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExhibitionPostings_Submissions_Id",
                table: "ExhibitionPostings",
                column: "Id",
                principalTable: "Submissions",
                principalColumn: "Id");
        }
    }
}
