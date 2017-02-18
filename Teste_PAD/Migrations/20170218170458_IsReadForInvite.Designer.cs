using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Teste_PAD.Models;

namespace Teste_PAD.Migrations
{
    [DbContext(typeof(BiciEventosDbContext))]
    [Migration("20170218170458_IsReadForInvite")]
    partial class IsReadForInvite
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Teste_PAD.Models.Attendance", b =>
                {
                    b.Property<int>("EventId");

                    b.Property<int>("UserId");

                    b.HasKey("EventId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("Teste_PAD.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<double>("EndLatitude");

                    b.Property<double>("EndLongitude");

                    b.Property<string>("EndTime");

                    b.Property<DateTime>("StartDate");

                    b.Property<double>("StartLatitude");

                    b.Property<double>("StartLongitude");

                    b.Property<string>("StartTime");

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Teste_PAD.Models.Invite", b =>
                {
                    b.Property<int>("EventId");

                    b.Property<int>("InvitedId");

                    b.Property<int>("InviterId");

                    b.Property<bool>("IsRead");

                    b.HasKey("EventId", "InvitedId", "InviterId");

                    b.HasIndex("InvitedId");

                    b.HasIndex("InviterId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("Teste_PAD.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.Property<DateTime>("RegisterDate");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Teste_PAD.Models.Attendance", b =>
                {
                    b.HasOne("Teste_PAD.Models.Event", "Event")
                        .WithMany("Attendances")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Teste_PAD.Models.User", "User")
                        .WithMany("Attendances")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Teste_PAD.Models.Event", b =>
                {
                    b.HasOne("Teste_PAD.Models.User", "User")
                        .WithMany("Events")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Teste_PAD.Models.Invite", b =>
                {
                    b.HasOne("Teste_PAD.Models.Event", "Event")
                        .WithMany("Invites")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Teste_PAD.Models.User", "Invited")
                        .WithMany("InvitesReceived")
                        .HasForeignKey("InvitedId");

                    b.HasOne("Teste_PAD.Models.User", "Inviter")
                        .WithMany("InvitesSent")
                        .HasForeignKey("InviterId");
                });
        }
    }
}
