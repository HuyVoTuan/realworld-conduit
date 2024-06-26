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
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240526003842_Delete_Comment_SelfJoin")]
    partial class Delete_Comment_SelfJoin
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

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzBlobTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("TriggerName")
                        .HasColumnType("text")
                        .HasColumnName("trigger_name");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("text")
                        .HasColumnName("trigger_group");

                    b.Property<byte[]>("BlobData")
                        .HasColumnType("bytea")
                        .HasColumnName("blob_data");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.ToTable("qrtz_blob_triggers", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzCalendar", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("CalendarName")
                        .HasColumnType("text")
                        .HasColumnName("calendar_name");

                    b.Property<byte[]>("Calendar")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("calendar");

                    b.HasKey("SchedulerName", "CalendarName");

                    b.ToTable("qrtz_calendars", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzCronTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("TriggerName")
                        .HasColumnType("text")
                        .HasColumnName("trigger_name");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("text")
                        .HasColumnName("trigger_group");

                    b.Property<string>("CronExpression")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("cron_expression");

                    b.Property<string>("TimeZoneId")
                        .HasColumnType("text")
                        .HasColumnName("time_zone_id");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.ToTable("qrtz_cron_triggers", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzFiredTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("EntryId")
                        .HasColumnType("text")
                        .HasColumnName("entry_id");

                    b.Property<long>("FiredTime")
                        .HasColumnType("bigint")
                        .HasColumnName("fired_time");

                    b.Property<string>("InstanceName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("instance_name");

                    b.Property<bool>("IsNonConcurrent")
                        .HasColumnType("bool")
                        .HasColumnName("is_nonconcurrent");

                    b.Property<string>("JobGroup")
                        .HasColumnType("text")
                        .HasColumnName("job_group");

                    b.Property<string>("JobName")
                        .HasColumnType("text")
                        .HasColumnName("job_name");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.Property<bool?>("RequestsRecovery")
                        .HasColumnType("bool")
                        .HasColumnName("requests_recovery");

                    b.Property<long>("ScheduledTime")
                        .HasColumnType("bigint")
                        .HasColumnName("sched_time");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("state");

                    b.Property<string>("TriggerGroup")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("trigger_group");

                    b.Property<string>("TriggerName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("trigger_name");

                    b.HasKey("SchedulerName", "EntryId");

                    b.HasIndex("InstanceName")
                        .HasDatabaseName("idx_qrtz_ft_trig_inst_name");

                    b.HasIndex("JobGroup")
                        .HasDatabaseName("idx_qrtz_ft_job_group");

                    b.HasIndex("JobName")
                        .HasDatabaseName("idx_qrtz_ft_job_name");

                    b.HasIndex("RequestsRecovery")
                        .HasDatabaseName("idx_qrtz_ft_job_req_recovery");

                    b.HasIndex("TriggerGroup")
                        .HasDatabaseName("idx_qrtz_ft_trig_group");

                    b.HasIndex("TriggerName")
                        .HasDatabaseName("idx_qrtz_ft_trig_name");

                    b.HasIndex("SchedulerName", "TriggerName", "TriggerGroup")
                        .HasDatabaseName("idx_qrtz_ft_trig_nm_gp");

                    b.ToTable("qrtz_fired_triggers", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzJobDetail", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("JobName")
                        .HasColumnType("text")
                        .HasColumnName("job_name");

                    b.Property<string>("JobGroup")
                        .HasColumnType("text")
                        .HasColumnName("job_group");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsDurable")
                        .HasColumnType("bool")
                        .HasColumnName("is_durable");

                    b.Property<bool>("IsNonConcurrent")
                        .HasColumnType("bool")
                        .HasColumnName("is_nonconcurrent");

                    b.Property<bool>("IsUpdateData")
                        .HasColumnType("bool")
                        .HasColumnName("is_update_data");

                    b.Property<string>("JobClassName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job_class_name");

                    b.Property<byte[]>("JobData")
                        .HasColumnType("bytea")
                        .HasColumnName("job_data");

                    b.Property<bool>("RequestsRecovery")
                        .HasColumnType("bool")
                        .HasColumnName("requests_recovery");

                    b.HasKey("SchedulerName", "JobName", "JobGroup");

                    b.HasIndex("RequestsRecovery")
                        .HasDatabaseName("idx_qrtz_j_req_recovery");

                    b.ToTable("qrtz_job_details", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzLock", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("LockName")
                        .HasColumnType("text")
                        .HasColumnName("lock_name");

                    b.HasKey("SchedulerName", "LockName");

                    b.ToTable("qrtz_locks", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzPausedTriggerGroup", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("text")
                        .HasColumnName("trigger_group");

                    b.HasKey("SchedulerName", "TriggerGroup");

                    b.ToTable("qrtz_paused_trigger_grps", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSchedulerState", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("InstanceName")
                        .HasColumnType("text")
                        .HasColumnName("instance_name");

                    b.Property<long>("CheckInInterval")
                        .HasColumnType("bigint")
                        .HasColumnName("checkin_interval");

                    b.Property<long>("LastCheckInTime")
                        .HasColumnType("bigint")
                        .HasColumnName("last_checkin_time");

                    b.HasKey("SchedulerName", "InstanceName");

                    b.ToTable("qrtz_scheduler_state", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSimplePropertyTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("TriggerName")
                        .HasColumnType("text")
                        .HasColumnName("trigger_name");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("text")
                        .HasColumnName("trigger_group");

                    b.Property<bool?>("BooleanProperty1")
                        .HasColumnType("bool")
                        .HasColumnName("bool_prop_1");

                    b.Property<bool?>("BooleanProperty2")
                        .HasColumnType("bool")
                        .HasColumnName("bool_prop_2");

                    b.Property<decimal?>("DecimalProperty1")
                        .HasColumnType("numeric")
                        .HasColumnName("dec_prop_1");

                    b.Property<decimal?>("DecimalProperty2")
                        .HasColumnType("numeric")
                        .HasColumnName("dec_prop_2");

                    b.Property<int?>("IntegerProperty1")
                        .HasColumnType("integer")
                        .HasColumnName("int_prop_1");

                    b.Property<int?>("IntegerProperty2")
                        .HasColumnType("integer")
                        .HasColumnName("int_prop_2");

                    b.Property<long?>("LongProperty1")
                        .HasColumnType("bigint")
                        .HasColumnName("long_prop_1");

                    b.Property<long?>("LongProperty2")
                        .HasColumnType("bigint")
                        .HasColumnName("long_prop_2");

                    b.Property<string>("StringProperty1")
                        .HasColumnType("text")
                        .HasColumnName("str_prop_1");

                    b.Property<string>("StringProperty2")
                        .HasColumnType("text")
                        .HasColumnName("str_prop_2");

                    b.Property<string>("StringProperty3")
                        .HasColumnType("text")
                        .HasColumnName("str_prop_3");

                    b.Property<string>("TimeZoneId")
                        .HasColumnType("text")
                        .HasColumnName("time_zone_id");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.ToTable("qrtz_simprop_triggers", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSimpleTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("TriggerName")
                        .HasColumnType("text")
                        .HasColumnName("trigger_name");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("text")
                        .HasColumnName("trigger_group");

                    b.Property<long>("RepeatCount")
                        .HasColumnType("bigint")
                        .HasColumnName("repeat_count");

                    b.Property<long>("RepeatInterval")
                        .HasColumnType("bigint")
                        .HasColumnName("repeat_interval");

                    b.Property<long>("TimesTriggered")
                        .HasColumnType("bigint")
                        .HasColumnName("times_triggered");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.ToTable("qrtz_simple_triggers", "quartz");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", b =>
                {
                    b.Property<string>("SchedulerName")
                        .HasColumnType("text")
                        .HasColumnName("sched_name");

                    b.Property<string>("TriggerName")
                        .HasColumnType("text")
                        .HasColumnName("trigger_name");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("text")
                        .HasColumnName("trigger_group");

                    b.Property<string>("CalendarName")
                        .HasColumnType("text")
                        .HasColumnName("calendar_name");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<long?>("EndTime")
                        .HasColumnType("bigint")
                        .HasColumnName("end_time");

                    b.Property<byte[]>("JobData")
                        .HasColumnType("bytea")
                        .HasColumnName("job_data");

                    b.Property<string>("JobGroup")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job_group");

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job_name");

                    b.Property<short?>("MisfireInstruction")
                        .HasColumnType("smallint")
                        .HasColumnName("misfire_instr");

                    b.Property<long?>("NextFireTime")
                        .HasColumnType("bigint")
                        .HasColumnName("next_fire_time");

                    b.Property<long?>("PreviousFireTime")
                        .HasColumnType("bigint")
                        .HasColumnName("prev_fire_time");

                    b.Property<int?>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("priority");

                    b.Property<long>("StartTime")
                        .HasColumnType("bigint")
                        .HasColumnName("start_time");

                    b.Property<string>("TriggerState")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("trigger_state");

                    b.Property<string>("TriggerType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("trigger_type");

                    b.HasKey("SchedulerName", "TriggerName", "TriggerGroup");

                    b.HasIndex("NextFireTime")
                        .HasDatabaseName("idx_qrtz_t_next_fire_time");

                    b.HasIndex("TriggerState")
                        .HasDatabaseName("idx_qrtz_t_state");

                    b.HasIndex("NextFireTime", "TriggerState")
                        .HasDatabaseName("idx_qrtz_t_nft_st");

                    b.HasIndex("SchedulerName", "JobName", "JobGroup");

                    b.ToTable("qrtz_triggers", "quartz");
                });

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

                    b.Property<string>("Description")
                        .HasColumnType("text");

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

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

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
                    b.Property<Guid>("UserThatFollowId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserBeingFollowedId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserThatFollowId", "UserBeingFollowedId");

                    b.HasIndex("UserBeingFollowedId");

                    b.ToTable("Friendship", "user");
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

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("User", "user");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzBlobTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", "Trigger")
                        .WithMany("BlobTriggers")
                        .HasForeignKey("SchedulerName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trigger");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzCronTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", "Trigger")
                        .WithMany("CronTriggers")
                        .HasForeignKey("SchedulerName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trigger");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSimplePropertyTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", "Trigger")
                        .WithMany("SimplePropertyTriggers")
                        .HasForeignKey("SchedulerName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trigger");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzSimpleTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", "Trigger")
                        .WithMany("SimpleTriggers")
                        .HasForeignKey("SchedulerName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trigger");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", b =>
                {
                    b.HasOne("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzJobDetail", "JobDetail")
                        .WithMany("Triggers")
                        .HasForeignKey("SchedulerName", "JobName", "JobGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobDetail");
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

                    b.HasOne("RealWorldConduit_Domain.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");

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
                    b.HasOne("RealWorldConduit_Domain.Entities.User", "UserBeingFollowed")
                        .WithMany("UsersBeingFollowed")
                        .HasForeignKey("UserBeingFollowedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealWorldConduit_Domain.Entities.User", "UserThatFollow")
                        .WithMany("UsersThatFollow")
                        .HasForeignKey("UserThatFollowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserBeingFollowed");

                    b.Navigation("UserThatFollow");
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

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzJobDetail", b =>
                {
                    b.Navigation("Triggers");
                });

            modelBuilder.Entity("AppAny.Quartz.EntityFrameworkCore.Migrations.QuartzTrigger", b =>
                {
                    b.Navigation("BlobTriggers");

                    b.Navigation("CronTriggers");

                    b.Navigation("SimplePropertyTriggers");

                    b.Navigation("SimpleTriggers");
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
                    b.Navigation("Blogs");

                    b.Navigation("Comments");

                    b.Navigation("FavoriteBlogs");

                    b.Navigation("RefreshTokens");

                    b.Navigation("UsersBeingFollowed");

                    b.Navigation("UsersThatFollow");
                });
#pragma warning restore 612, 618
        }
    }
}
