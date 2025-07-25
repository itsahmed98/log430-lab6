﻿// <auto-generated />
using ECommerceMcService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ECommerceMcService.Migrations
{
    [DbContext(typeof(ECommerceDbContext))]
    partial class ECommerceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ECommerceMcService.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ClientId"));

                    b.Property<string>("Adresse")
                        .HasColumnType("text");

                    b.Property<string>("Courriel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            ClientId = 1,
                            Adresse = "",
                            Courriel = "alice@dupont.ca",
                            Nom = "Alice Dupont"
                        },
                        new
                        {
                            ClientId = 2,
                            Adresse = "",
                            Courriel = "alex@alexandre.ca",
                            Nom = "Alex Alexandre"
                        },
                        new
                        {
                            ClientId = 3,
                            Adresse = "",
                            Courriel = "chris@christopher.ca",
                            Nom = "Chris Christopher"
                        },
                        new
                        {
                            ClientId = 4,
                            Adresse = "",
                            Courriel = "simon@samuel.ca",
                            Nom = "Simon Samuel"
                        });
                });

            modelBuilder.Entity("ECommerceMcService.Models.LignePanier", b =>
                {
                    b.Property<int>("LignePanierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LignePanierId"));

                    b.Property<int>("PanierId")
                        .HasColumnType("integer");

                    b.Property<int>("ProduitId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantite")
                        .HasColumnType("integer");

                    b.HasKey("LignePanierId");

                    b.HasIndex("PanierId");

                    b.ToTable("LignesPanier");

                    b.HasData(
                        new
                        {
                            LignePanierId = 1,
                            PanierId = 1,
                            ProduitId = 1,
                            Quantite = 2
                        },
                        new
                        {
                            LignePanierId = 2,
                            PanierId = 1,
                            ProduitId = 2,
                            Quantite = 1
                        });
                });

            modelBuilder.Entity("ECommerceMcService.Models.Panier", b =>
                {
                    b.Property<int>("PanierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PanierId"));

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.HasKey("PanierId");

                    b.ToTable("Paniers");

                    b.HasData(
                        new
                        {
                            PanierId = 1,
                            ClientId = 2
                        });
                });

            modelBuilder.Entity("ECommerceMcService.Models.LignePanier", b =>
                {
                    b.HasOne("ECommerceMcService.Models.Panier", null)
                        .WithMany("Lignes")
                        .HasForeignKey("PanierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ECommerceMcService.Models.Panier", b =>
                {
                    b.Navigation("Lignes");
                });
#pragma warning restore 612, 618
        }
    }
}
