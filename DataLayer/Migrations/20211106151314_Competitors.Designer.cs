﻿// <auto-generated />
using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataLayer.Migrations
{
    [DbContext(typeof(EfDbContext))]
    [Migration("20211106151314_Competitors")]
    partial class Competitors
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataLayer.Entities.Competitor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("About")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Biography")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyOverview")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FeaturedPhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FeaturedVideo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Followers")
                        .HasColumnType("int");

                    b.Property<string>("GeneralInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.Property<string>("Mission")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Competitors");
                });

            modelBuilder.Entity("DataLayer.Entities.FacebookPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Comments")
                        .HasColumnType("int");

                    b.Property<Guid?>("CompetitorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.Property<string>("MediaFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MediaType")
                        .HasColumnType("int");

                    b.Property<int>("PostNumber")
                        .HasColumnType("int");

                    b.Property<int>("Reactions")
                        .HasColumnType("int");

                    b.Property<int>("Shares")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompetitorId");

                    b.ToTable("FacebookPosts");
                });

            modelBuilder.Entity("DataLayer.Entities.FacebookPost", b =>
                {
                    b.HasOne("DataLayer.Entities.Competitor", "Competitor")
                        .WithMany("FacebookPosts")
                        .HasForeignKey("CompetitorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Competitor");
                });

            modelBuilder.Entity("DataLayer.Entities.Competitor", b =>
                {
                    b.Navigation("FacebookPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
