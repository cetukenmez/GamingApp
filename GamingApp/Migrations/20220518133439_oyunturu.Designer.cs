﻿// <auto-generated />
using System;
using GamingApp.Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GamingApp.Migrations
{
    [DbContext(typeof(GamingAppContext))]
    [Migration("20220518133439_oyunturu")]
    partial class oyunturu
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.15");

            modelBuilder.Entity("GamingApp.Entity.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("GameAdminUserId")
                        .HasColumnType("int");

                    b.Property<bool>("GameClosed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("GameName")
                        .HasColumnType("longtext");

                    b.Property<int?>("OyunTuruId")
                        .HasColumnType("int");

                    b.HasKey("GameId");

                    b.HasIndex("OyunTuruId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("GamingApp.Entity.GameUser", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("GameUser");
                });

            modelBuilder.Entity("GamingApp.Entity.Logs", b =>
                {
                    b.Property<int>("LogsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<string>("InsertTable")
                        .HasColumnType("longtext");

                    b.Property<string>("IpAddress")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LogDesc")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("LogsId");

                    b.HasIndex("GameId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("GamingApp.Entity.OyunTuru", b =>
                {
                    b.Property<int>("OyunTuruId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("OyunTuruAdi")
                        .HasColumnType("longtext");

                    b.HasKey("OyunTuruId");

                    b.ToTable("OyunTuru");
                });

            modelBuilder.Entity("GamingApp.Entity.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<bool>("SessionClosed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SessionName")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("SessionTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("SessionId");

                    b.HasIndex("GameId");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("GamingApp.Entity.SessionUser", b =>
                {
                    b.Property<int>("SessionUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Cash")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("CashOut")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalCash")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("SessionUserId");

                    b.HasIndex("SessionId");

                    b.HasIndex("UserId");

                    b.ToTable("SessionUser");
                });

            modelBuilder.Entity("GamingApp.Entity.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserNameSurname")
                        .HasColumnType("longtext");

                    b.Property<string>("UserPhoto")
                        .HasColumnType("longtext");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("GamingApp.Entity.Game", b =>
                {
                    b.HasOne("GamingApp.Entity.OyunTuru", "OyunTuru")
                        .WithMany()
                        .HasForeignKey("OyunTuruId");

                    b.Navigation("OyunTuru");
                });

            modelBuilder.Entity("GamingApp.Entity.GameUser", b =>
                {
                    b.HasOne("GamingApp.Entity.Game", "Game")
                        .WithMany("GameUser")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamingApp.Entity.User", "User")
                        .WithMany("GameUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GamingApp.Entity.Logs", b =>
                {
                    b.HasOne("GamingApp.Entity.Game", "Game")
                        .WithMany("Logs")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("GamingApp.Entity.Session", b =>
                {
                    b.HasOne("GamingApp.Entity.Game", "Game")
                        .WithMany("Session")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("GamingApp.Entity.SessionUser", b =>
                {
                    b.HasOne("GamingApp.Entity.Session", "Session")
                        .WithMany("SessionUser")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamingApp.Entity.User", "User")
                        .WithMany("SessionUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GamingApp.Entity.Game", b =>
                {
                    b.Navigation("GameUser");

                    b.Navigation("Logs");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("GamingApp.Entity.Session", b =>
                {
                    b.Navigation("SessionUser");
                });

            modelBuilder.Entity("GamingApp.Entity.User", b =>
                {
                    b.Navigation("GameUser");

                    b.Navigation("SessionUser");
                });
#pragma warning restore 612, 618
        }
    }
}