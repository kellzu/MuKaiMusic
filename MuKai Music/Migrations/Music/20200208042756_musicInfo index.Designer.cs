﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MuKai_Music.DataContext;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MuKai_Music.Migrations.Music
{
    [DbContext(typeof(MusicContext))]
    [Migration("20200208042756_musicInfo index")]
    partial class musicInfoindex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MuKai_Music.Model.DataEntity.MusicInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AlbumName")
                        .HasColumnType("text");

                    b.Property<string>("ArtistName")
                        .HasColumnType("text");

                    b.Property<int?>("Duration")
                        .HasColumnType("integer");

                    b.Property<int?>("KuWo_Id")
                        .HasColumnType("integer");

                    b.Property<int?>("Migu_Id")
                        .HasColumnType("integer");

                    b.Property<string>("Migu_Url")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("Ne_AlbumId")
                        .HasColumnType("integer");

                    b.Property<int?>("Ne_ArtistId")
                        .HasColumnType("integer");

                    b.Property<int>("Ne_Id")
                        .HasColumnType("integer");

                    b.Property<string>("PicUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Ne_Id");

                    b.ToTable("MusicInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
