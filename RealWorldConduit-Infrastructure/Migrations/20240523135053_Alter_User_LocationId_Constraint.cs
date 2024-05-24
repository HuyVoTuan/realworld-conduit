using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealWorldConduit_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_User_LocationId_Constraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Location_LocationId",
                schema: "user",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Location_LocationId",
                schema: "user",
                table: "User",
                column: "LocationId",
                principalSchema: "user",
                principalTable: "Location",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Location_LocationId",
                schema: "user",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Location_LocationId",
                schema: "user",
                table: "User",
                column: "LocationId",
                principalSchema: "user",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
