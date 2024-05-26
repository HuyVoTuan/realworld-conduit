using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealWorldConduit_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Delete_Comment_SelfJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ParentCommentId",
                schema: "blog",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ParentCommentId",
                schema: "blog",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                schema: "blog",
                table: "Comment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentCommentId",
                schema: "blog",
                table: "Comment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentCommentId",
                schema: "blog",
                table: "Comment",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ParentCommentId",
                schema: "blog",
                table: "Comment",
                column: "ParentCommentId",
                principalSchema: "blog",
                principalTable: "Comment",
                principalColumn: "Id");
        }
    }
}
