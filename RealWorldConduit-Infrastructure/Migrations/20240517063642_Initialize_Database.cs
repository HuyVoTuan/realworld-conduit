using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealWorldConduit_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initialize_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "blog");

            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    Firstname = table.Column<string>(type: "text", nullable: false),
                    Lastname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                schema: "blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blog_User_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendship",
                schema: "user",
                columns: table => new
                {
                    BeingFollowedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendship", x => new { x.BeingFollowedUserId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_Friendship_User_BeingFollowedUserId",
                        column: x => x.BeingFollowedUserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendship_User_FollowerId",
                        column: x => x.FollowerId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    District = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshTokenString = table.Column<string>(type: "text", nullable: false),
                    ExpiredTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogTag",
                schema: "blog",
                columns: table => new
                {
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTag", x => new { x.BlogId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BlogTag_Blog_BlogId",
                        column: x => x.BlogId,
                        principalSchema: "blog",
                        principalTable: "Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogTag_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "blog",
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                schema: "blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Blog_BlogId",
                        column: x => x.BlogId,
                        principalSchema: "blog",
                        principalTable: "Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalSchema: "blog",
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteBlog",
                schema: "blog",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteBlog", x => new { x.UserId, x.BlogId });
                    table.ForeignKey(
                        name: "FK_FavoriteBlog_Blog_BlogId",
                        column: x => x.BlogId,
                        principalSchema: "blog",
                        principalTable: "Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteBlog_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_AuthorId",
                schema: "blog",
                table: "Blog",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_Slug",
                schema: "blog",
                table: "Blog",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogTag_TagId",
                schema: "blog",
                table: "BlogTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_BlogId",
                schema: "blog",
                table: "Comment",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentCommentId",
                schema: "blog",
                table: "Comment",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                schema: "blog",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBlog_BlogId",
                schema: "blog",
                table: "FavoriteBlog",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_FollowerId",
                schema: "user",
                table: "Friendship",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_City",
                schema: "user",
                table: "Location",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Location_UserId",
                schema: "user",
                table: "Location",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_RefreshTokenString",
                schema: "user",
                table: "RefreshToken",
                column: "RefreshTokenString",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                schema: "user",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Name",
                schema: "blog",
                table: "Tag",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "user",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Slug",
                schema: "user",
                table: "User",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogTag",
                schema: "blog");

            migrationBuilder.DropTable(
                name: "Comment",
                schema: "blog");

            migrationBuilder.DropTable(
                name: "FavoriteBlog",
                schema: "blog");

            migrationBuilder.DropTable(
                name: "Friendship",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Location",
                schema: "user");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "blog");

            migrationBuilder.DropTable(
                name: "Blog",
                schema: "blog");

            migrationBuilder.DropTable(
                name: "User",
                schema: "user");
        }
    }
}
