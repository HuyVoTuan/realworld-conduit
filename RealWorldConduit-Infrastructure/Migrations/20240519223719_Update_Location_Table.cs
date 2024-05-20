using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealWorldConduit_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Location_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Location_City",
                schema: "user",
                table: "Location");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "user",
                table: "Location",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Location_Slug",
                schema: "user",
                table: "Location",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Location_Slug",
                schema: "user",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "user",
                table: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_Location_City",
                schema: "user",
                table: "Location",
                column: "City");
        }
    }
}
