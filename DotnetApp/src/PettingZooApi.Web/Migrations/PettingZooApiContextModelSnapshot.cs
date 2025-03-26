﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PettingZooApi.Web.Data;

#nullable disable

namespace PettingZooApi.Web.Migrations
{
    [DbContext(typeof(PettingZooApiContext))]
    partial class PettingZooApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PettingZooApi.Web.Models.Animal", b =>
                {
                    b.Property<int>("AnimalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("animal_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AnimalId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(120)")
                        .HasColumnName("name");

                    b.Property<int>("SpeciesId")
                        .HasColumnType("integer")
                        .HasColumnName("species_id");

                    b.HasKey("AnimalId");

                    b.HasIndex("SpeciesId");

                    b.ToTable("animals");
                });

            modelBuilder.Entity("PettingZooApi.Web.Models.Species", b =>
                {
                    b.Property<int>("SpeciesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("species_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SpeciesId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Diet")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnName("diet");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(120)")
                        .HasColumnName("name");

                    b.HasKey("SpeciesId");

                    b.ToTable("species");
                });

            modelBuilder.Entity("PettingZooApi.Web.Models.Animal", b =>
                {
                    b.HasOne("PettingZooApi.Web.Models.Species", "Species")
                        .WithMany()
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Species");
                });
#pragma warning restore 612, 618
        }
    }
}
