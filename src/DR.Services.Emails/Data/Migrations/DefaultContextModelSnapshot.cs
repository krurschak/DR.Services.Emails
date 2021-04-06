﻿// <auto-generated />
using System;
using DR.Services.Emails.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DR.Services.Emails.Data.Migrations
{
    [DbContext(typeof(DefaultContext))]
    partial class DefaultContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("DR.Services.Emails.Data.Models.Email", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AppId")
                        .HasColumnType("uuid");

                    b.Property<string>("Bcc")
                        .HasColumnType("text");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cc")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDateUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("EmailStatus")
                        .HasColumnType("integer");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdateUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Recipients")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("SentDateUtc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Signature")
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Emails");
                });
#pragma warning restore 612, 618
        }
    }
}
