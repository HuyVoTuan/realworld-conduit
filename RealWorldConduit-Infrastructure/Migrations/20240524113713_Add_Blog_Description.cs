using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealWorldConduit_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Blog_Description : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "blog",
                table: "Blog",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "blog",
                table: "Blog");
        }
    }
}
