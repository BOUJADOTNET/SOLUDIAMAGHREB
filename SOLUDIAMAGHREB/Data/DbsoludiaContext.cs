using Microsoft.EntityFrameworkCore;
using SOLUDIAMAGHREB.Models;

namespace SOLUDIAMAGHREB.Data;

public partial class DbsoludiaContext : DbContext
{
    public DbsoludiaContext()
    {
    }

    public DbsoludiaContext(DbContextOptions<DbsoludiaContext> options)
        : base(options)
    {
    }

    public DbSet<Actmanager> Actmanagers { get; set; }


    public DbSet<Utilisateur> Utilisateurs { get; set; }
    public DbSet<BordereauManager> bordereauManagers { get; set; }
    public DbSet<BordereauItem> bordereauItems { get; set; }
    public DbSet<MyBorderauItems> MyBorderauItems { get; set; }
    public DbSet<Declarationlh> MyDeclarationlhs { get; set; }
    public DbSet<Analyse> MyAnalyses { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DEVELOPSLD\\SQL2010;Initial Catalog=DBSOLUDIAMAGHREB;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MyBorderauItems>(entity =>
        {
            entity.Property(e => e.Nlot)
            .IsRequired();
            entity.Property(e => e.Designation)
            .IsRequired();
            entity.Property(e => e.NomdeMarque)
            .IsRequired();
            entity.Property(e => e.Conditionnement)
            .IsRequired();
            entity.Property(e => e.Unite_Compte)
            .IsRequired();
            entity.Property(e => e.Quantite)
            .IsRequired();
            entity.Property(e => e.Prix_Unit_TVA)
            .IsRequired();
            entity.Property(e => e.Prix_Total_TVA)
            .IsRequired();
            entity.Property(e => e.AppelOffres)
           .IsRequired();


        });



        //////////////////////
        modelBuilder.Entity<BordereauManager>(entity =>
        {
            entity.ToTable("BORDEREAUMANAGER");
            entity.Property(b => b.NomberBordereau)
                .IsRequired()
                .HasMaxLength(10);
            entity.HasKey(e => e.NomberBordereau);

            entity.Property(e => e.DateCreation)
                .IsRequired()
                .HasDefaultValueSql("CAST(GETDATE() AS DATE)");

        });
        modelBuilder.Entity<BordereauItem>(entity =>
        {
            entity.HasKey(e => new
            {
                e.Nlot,
                e.Id
            }).HasName("PK_BordereauItems");

            entity.HasIndex(b => b.Id)
                .IsUnique();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

        });


        modelBuilder.Entity<BordereauItem>()

            .HasOne(i => i.BordereauManager)
            .WithMany(b => b.Items)  // Assuming BordereauManager has a collection of Items
            .HasForeignKey(i => i.NomberBordereau);


        modelBuilder.Entity<Actmanager>(entity =>
        {
            entity.HasKey(e => e.IdactEng).HasName("PK__ActManager");

            entity.ToTable("ACTMANAGER");

            entity.Property(e => e.IdactEng);
            entity.Property(e => e.Appel_Offres)
             .IsRequired(false);


            entity.Property(e => e.Objet_du_Marche)
            .IsRequired(false);


            entity.Property(e => e.Marche_passe)
            .IsRequired(false);


            entity.Property(e => e.Capital)
            .IsRequired(false)
            .HasMaxLength(100);

            entity.Property(e => e.NLot)
            .IsRequired(false)
            .HasMaxLength(100);

            entity.Property(e => e.MontantDh).IsRequired(false)
                .HasColumnName("Montant_DH");
            entity.Property(e => e.MontantHtTva)
                .HasColumnName("Montant_HT_TVA");
            entity.Property(e => e.MontantTva)
                .HasColumnName("Montant_TVA");
            entity.Property(e => e.MontantTvaComprise)
                .HasColumnName("Montant_TVA_Comprise");
            entity.Property(e => e.TauxTva)
                .HasColumnName("Taux_TVA");
            entity.Property(e => e.DateCreation)
                  .IsRequired() // Make the field required
                  .HasDefaultValueSql("CAST(GETDATE() AS DATE)");
        });

        modelBuilder.Entity<Declarationlh>(entity =>
        {
            entity.HasKey(e => e.IdDeclar).HasName("PK__Déclarationlhonneur");
            entity.ToTable("Declarationlh");
            entity.Property(e => e.IdDeclar).HasColumnName("idDeclar");
            entity.Property(e => e.AppelOffer)
                .IsRequired(false);
            entity.Property(e => e.ObjectMarche)
                .IsRequired(false);
            entity.Property(e => e.Capital)
            .IsRequired(false);
            entity.Property(e => e.DateCreation)
            .IsRequired()
            .HasDefaultValueSql("CAST(GETDATE() AS DATE)");


        });

        modelBuilder.Entity<Analyse>(entity =>
        {
            entity.ToTable("Analyse");
            entity.HasKey(e => e.idAvisAppelOff).HasName("Pk__IDAvisAppelOffer");
            entity.Property(b => b.idAvisAppelOff)
               .IsRequired()
               .HasMaxLength(50);
            entity.HasIndex(b => b.idAvisAppelOff)
               .IsUnique();
            entity.Property(e => e.idAvisAppelOff).ValueGeneratedOnAdd();

            entity.Property(b => b.NomberBordereau)
               .IsRequired();
            entity.Property(e => e.DateCreation)
                .IsRequired()
                .HasDefaultValueSql("CAST(GETDATE() AS DATE)");


        });

        /////////////////////////////////////
        // Configure foreign key relationship between Analyse and BordereauItem
        modelBuilder.Entity<Analyse>()
            .HasOne(a => a.BordereauItem) // Navigation property in Analyse
            .WithMany(b => b.Analyses)   // Navigation property in BordereauItem
            .HasForeignKey(a => a.Nlot)  // Foreign key in Analyse
            .HasPrincipalKey(b => b.Nlot) // Principal key in BordereauItem
            .OnDelete(DeleteBehavior.Cascade); // Configure delete behavior
        /////////////////////////////////////////////

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Utilisat__3717C98297588A3A");

            entity.ToTable("Utilisateur");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Clave)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Nom)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Prenom)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(70)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);



    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
