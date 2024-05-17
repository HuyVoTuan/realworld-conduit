﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RealWorldConduit_Infrastructure;

#nullable disable

namespace RealWorldConduit_Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20240517063642_Initialize_Database")]
    partial class Initialize_Database
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "pgcrypto");
            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Blog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Blog", "blog");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.BlogTag", b =>
                {
                    b.Property<Guid>("BlogId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("BlogId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("BlogTag", "blog");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BlogId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ParentCommentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("UserId");

                    b.ToTable("Comment", "blog");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.FavoriteBlog", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BlogId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserId", "BlogId");

                    b.HasIndex("BlogId");

                    b.ToTable("FavoriteBlog", "blog");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Friendship", b =>
                {
                    b.Property<Guid>("BeingFollowedUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("BeingFollowedUserId", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Friendship", "user");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("City");

                    b.HasIndex("UserId");

                    b.ToTable("Location", "user");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpiredTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RefreshTokenString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RefreshTokenString")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken", "user");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tag", "blog");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text");

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("User", "user");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Blog", b =>
                {
                    b.HasOne("RealWorldConduit_Domain.Entities.User", "Author")
                        .WithMany("Blogs")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.BlogTag", b =>
                {
                    b.HasOne("RealWorldConduit_Domain.Entities.Blog", "Blog")
                        .WithMany("BlogTags")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealWorldConduit_Domain.Entities.Tag", "Tag")
                        .WithMany("BlogTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Comment", b =>
                {
                    b.HasOne("RealWorldConduit_Domain.Entities.Blog", "Blog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealWorldConduit_Domain.Entities.Comment", "ParentComment")
                        .WithMany()
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealWorldConduit_Domain.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");

                    b.Navigation("ParentComment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.FavoriteBlog", b =>
                {
                    b.HasOne("RealWorldConduit_Domain.Entities.Blog", "Blog")
                        .WithMany("FavoriteBlogs")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealWorldConduit_Domain.Entities.User", "User")
                        .WithMany("FavoriteBlogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Friendship", b =>
                {
                    b.HasOne("RealWorldConduit_Domain.Entities.User", "BeingFollowedUser")
                        .WithMany("BeingFollowedUser")
                        .HasForeignKey("BeingFollowedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealWorldConduit_Domain.Entities.User", "Follower")
                        .WithMany("Follower")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BeingFollowedUser");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Location", b =>
                {
                    b.HasOne("RealWorldConduit_Domain.Entities.User", "User")
                        .WithMany("Locations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("RealWorldConduit_Domain.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Blog", b =>
                {
                    b.Navigation("BlogTags");

                    b.Navigation("Comments");

                    b.Navigation("FavoriteBlogs");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.Tag", b =>
                {
                    b.Navigation("BlogTags");
                });

            modelBuilder.Entity("RealWorldConduit_Domain.Entities.User", b =>
                {
                    b.Navigation("BeingFollowedUser");

                    b.Navigation("Blogs");

                    b.Navigation("Comments");

                    b.Navigation("FavoriteBlogs");

                    b.Navigation("Follower");

                    b.Navigation("Locations");

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}