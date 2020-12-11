﻿// <auto-generated />
using System;
using GoalSystemsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GoalSystemsAPI.Migrations
{
    [DbContext(typeof(InventoryContext))]
    [Migration("20201211184312_AdddExpiredFlag")]
    partial class AdddExpiredFlag
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("GoalSystemsAPI.Models.InventoryItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasMaxLength(4290)
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Expired")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<long>("Units")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("InventoryItems");
                });
#pragma warning restore 612, 618
        }
    }
}
