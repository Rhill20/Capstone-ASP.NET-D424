﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project_WEBAPP.Data;

#nullable disable

namespace Project_WEBAPP.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240401221922_GoodAttempt")]
    partial class GoodAttempt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("Project_WEBAPP.Models.EmployeeTimeLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ClockInTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ClockOutTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EmployeeTimeLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
