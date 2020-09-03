﻿// <auto-generated />
using System;
using HTTAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HTTAPI.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20200829140006_FromDate")]
    partial class FromDate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HTTAPI.Models.ComeToOfficeRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("HRComments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeclined")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RequestNumber")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("ComeToOfficeRequest");
                });

            modelBuilder.Entity("HTTAPI.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<int>("EmployeeCode")
                        .HasColumnType("int");

                    b.Property<bool>("IsHrManager")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("HTTAPI.Models.HealthTrack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ContactWithCovidPeople")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfTravel")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("PreExistHealthIssue")
                        .HasColumnType("bit");

                    b.Property<string>("RequestNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResidentialAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<bool>("TravelOustSideInLast15Days")
                        .HasColumnType("bit");

                    b.Property<int>("ZoneId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LocationId");

                    b.HasIndex("ZoneId");

                    b.ToTable("HealthTrack");
                });

            modelBuilder.Entity("HTTAPI.Models.HealthTrackQuestionAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HealthTrackId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<string>("value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HealthTrackId");

                    b.HasIndex("QuestionId");

                    b.ToTable("HealthTrackQuestionAnswer");
                });

            modelBuilder.Entity("HTTAPI.Models.HealthTrackSymptom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HealthTrackId")
                        .HasColumnType("int");

                    b.Property<int>("SymptomId")
                        .HasColumnType("int");

                    b.Property<bool>("value")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("HealthTrackId");

                    b.HasIndex("SymptomId");

                    b.ToTable("HealthTrackSymptom");
                });

            modelBuilder.Entity("HTTAPI.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Location");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Tricity (CHD/PKL/Mohali)",
                            Order = 1,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Outside Tricity",
                            Order = 2,
                            Status = 0,
                            Type = "Radio"
                        });
                });

            modelBuilder.Entity("HTTAPI.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Question");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Please give a count of family members in your house?",
                            Order = 1,
                            Status = 0,
                            Type = "Input"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Is any Of them under 5 years or over 65 years in age?",
                            Order = 2,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Has any Of them presented Covid-19 related symptoms recently?",
                            Order = 3,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Has any Of them had any recent travel — abroad, inter-state or inter or district ? ",
                            Order = 4,
                            Status = 0,
                            Type = "Radio"
                        });
                });

            modelBuilder.Entity("HTTAPI.Models.Symptom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Symptom");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Fever",
                            Order = 1,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Shortness Of Breath",
                            Order = 2,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Dry Cough",
                            Order = 3,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Running Nose",
                            Order = 4,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Sore Throat",
                            Order = 5,
                            Status = 0,
                            Type = "Radio"
                        });
                });

            modelBuilder.Entity("HTTAPI.Models.Zone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Zone");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Containment Zone",
                            Order = 1,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Red Zone",
                            Order = 2,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Green Zone",
                            Order = 3,
                            Status = 0,
                            Type = "Radio"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Orange Zone",
                            Order = 4,
                            Status = 0,
                            Type = "Radio"
                        });
                });

            modelBuilder.Entity("HTTAPI.Models.ComeToOfficeRequest", b =>
                {
                    b.HasOne("HTTAPI.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HTTAPI.Models.HealthTrack", b =>
                {
                    b.HasOne("HTTAPI.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HTTAPI.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HTTAPI.Models.Zone", "Zone")
                        .WithMany()
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HTTAPI.Models.HealthTrackQuestionAnswer", b =>
                {
                    b.HasOne("HTTAPI.Models.HealthTrack", "HealthTrack")
                        .WithMany("HealthTrackQuestions")
                        .HasForeignKey("HealthTrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HTTAPI.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HTTAPI.Models.HealthTrackSymptom", b =>
                {
                    b.HasOne("HTTAPI.Models.HealthTrack", "HealthTrack")
                        .WithMany("HealthTrackSymptoms")
                        .HasForeignKey("HealthTrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HTTAPI.Models.Symptom", "Symptom")
                        .WithMany()
                        .HasForeignKey("SymptomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}