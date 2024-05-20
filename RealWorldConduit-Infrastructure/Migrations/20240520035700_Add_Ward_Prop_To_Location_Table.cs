using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealWorldConduit_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Ward_Prop_To_Location_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ward",
                schema: "user",
                table: "Location",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ward",
                schema: "user",
                table: "Location");
        }
    }
}
