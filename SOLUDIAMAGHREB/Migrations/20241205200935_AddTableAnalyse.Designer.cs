﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SOLUDIAMAGHREB.Data;

#nullable disable

namespace SOLUDIAMAGHREB.Migrations
{
    [DbContext(typeof(DbsoludiaContext))]
    [Migration("20241205200935_AddTableAnalyse")]
    partial class AddTableAnalyse
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.Actmanager", b =>
                {
                    b.Property<string>("IdactEng")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Appel_Offres")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Capital")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("DateCreation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CAST(GETDATE() AS DATE)");

                    b.Property<string>("Marche_passe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MontantDh")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Montant_DH");

                    b.Property<decimal>("MontantHtTva")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Montant_HT_TVA");

                    b.Property<decimal>("MontantTva")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Montant_TVA");

                    b.Property<decimal>("MontantTvaComprise")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Montant_TVA_Comprise");

                    b.Property<string>("NLot")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NomberBordereau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Objet_du_Marche")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TauxTva")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Taux_TVA");

                    b.HasKey("IdactEng")
                        .HasName("PK__ActManager");

                    b.ToTable("ACTMANAGER", (string)null);
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.Analyse", b =>
                {
                    b.Property<int>("idAvisAppelOff")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idAvisAppelOff"));

                    b.Property<DateTime>("DateCreation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CAST(GETDATE() AS DATE)");

                    b.Property<decimal>("Montant_total_DHS")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Montant_total_littre_DHS")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Nlot")
                        .HasColumnType("int");

                    b.Property<string>("NomberBordereau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idAvisAppelOff")
                        .HasName("Pk__IDAvisAppelOffer");

                    b.HasIndex("Nlot");

                    b.HasIndex("idAvisAppelOff")
                        .IsUnique();

                    b.ToTable("Analyse", (string)null);
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.BordereauItem", b =>
                {
                    b.Property<int>("Nlot")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomberBordereau")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal>("Prix_Total_TVA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Prix_Unit_TVA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantite")
                        .HasColumnType("int");

                    b.Property<string>("Unite_Compte")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Nlot", "Id")
                        .HasName("PK_BordereauItems");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("NomberBordereau");

                    b.ToTable("bordereauItems");
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.BordereauManager", b =>
                {
                    b.Property<string>("NomberBordereau")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("DateCreation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CAST(GETDATE() AS DATE)");

                    b.Property<string>("Intitu_AppelOffres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NomberBordereau");

                    b.ToTable("BORDEREAUMANAGER", (string)null);
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.Declarationlh", b =>
                {
                    b.Property<string>("IdDeclar")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("idDeclar");

                    b.Property<string>("AppelOffer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Capital")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreation")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CAST(GETDATE() AS DATE)");

                    b.Property<string>("NomberBordereau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObjectMarche")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdDeclar")
                        .HasName("PK__Déclarationlhonneur");

                    b.ToTable("Declarationlh", (string)null);
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.MyBorderauItems", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("AppelOffres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Conditionnement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Nlot")
                        .HasColumnType("int");

                    b.Property<string>("NomberBordereau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomdeMarque")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Prix_Total_TVA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Prix_Unit_TVA")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantite")
                        .HasColumnType("int");

                    b.Property<string>("Unite_Compte")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("MyBorderauItems");
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.Utilisateur", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idUser");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUser"));

                    b.Property<string>("Clave")
                        .HasMaxLength(70)
                        .IsUnicode(false)
                        .HasColumnType("varchar(70)");

                    b.Property<string>("Email")
                        .HasMaxLength(70)
                        .IsUnicode(false)
                        .HasColumnType("varchar(70)");

                    b.Property<string>("Nom")
                        .HasMaxLength(70)
                        .IsUnicode(false)
                        .HasColumnType("varchar(70)");

                    b.Property<string>("Prenom")
                        .HasMaxLength(70)
                        .IsUnicode(false)
                        .HasColumnType("varchar(70)");

                    b.Property<string>("UserName")
                        .HasMaxLength(70)
                        .IsUnicode(false)
                        .HasColumnType("varchar(70)");

                    b.HasKey("IdUser")
                        .HasName("PK__Utilisat__3717C98297588A3A");

                    b.ToTable("Utilisateur", (string)null);
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.Analyse", b =>
                {
                    b.HasOne("SOLUDIAMAGHREB.Models.BordereauItem", "BordereauItem")
                        .WithMany("Analyses")
                        .HasForeignKey("Nlot")
                        .HasPrincipalKey("Nlot")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BordereauItem");
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.BordereauItem", b =>
                {
                    b.HasOne("SOLUDIAMAGHREB.Models.BordereauManager", "BordereauManager")
                        .WithMany("Items")
                        .HasForeignKey("NomberBordereau")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BordereauManager");
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.BordereauItem", b =>
                {
                    b.Navigation("Analyses");
                });

            modelBuilder.Entity("SOLUDIAMAGHREB.Models.BordereauManager", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
