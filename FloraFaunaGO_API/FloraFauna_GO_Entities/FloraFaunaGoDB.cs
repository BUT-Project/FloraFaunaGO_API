using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FloraFauna_GO_Entities
{
    public class FloraFaunaGoDB : IdentityDbContext<UtilisateurEntities, CustomRole, string>
    {
        public virtual DbSet<EspeceEntities> Espece { get; set; }
        public virtual DbSet<CaptureEntities> Captures { get; set; }
        public virtual DbSet<CaptureDetailsEntities> CaptureDetails { get; set; }
        public virtual DbSet<EspeceLocalisationEntities> EspeceLocalisation { get; set; }
        public virtual DbSet<LocalisationEntities> Localisation { get; set; }
        public virtual DbSet<UtilisateurEntities> Utilisateur { get; set; }
        public virtual DbSet<SuccesEntities> Succes { get; set; }
        public virtual DbSet<SuccesStateEntities> SuccesState { get; set; }

        public FloraFaunaGoDB()
            : base() { }

        public FloraFaunaGoDB(DbContextOptions<FloraFaunaGoDB> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
                options.UseSqlite($"Data Source=FloraFaunaGoDB.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EspeceEntities>().HasKey(e => e.Id);
            modelBuilder.Entity<CaptureEntities>().HasKey(c => c.Id);
            modelBuilder.Entity<CaptureDetailsEntities>().HasKey(cd => cd.Id);
            modelBuilder.Entity<LocalisationEntities>().HasKey(l => l.Id);
            modelBuilder.Entity<UtilisateurEntities>().HasKey(u => u.Id);
            modelBuilder.Entity<SuccesEntities>().HasKey(s => s.Id);
            modelBuilder.Entity<SuccesStateEntities>().HasKey(ss => ss.Id);

            // EspeceEntities - LocalisationEntities (Many-to-Many)
            modelBuilder.Entity<EspeceLocalisationEntities>()
                        .HasKey(el => new { el.EspeceId, el.LocalisationId });

            modelBuilder.Entity<EspeceLocalisationEntities>()
                .HasOne(el => el.Espece)
                .WithMany(e => e.Localisations)
                .HasForeignKey(el => el.EspeceId);

            modelBuilder.Entity<EspeceLocalisationEntities>()
                .HasOne(el => el.Localisation)
                .WithMany(l => l.EspeceLocalisation)
                .HasForeignKey(el => el.LocalisationId);

            // LocalisationEntities - CaptureDetailsEntities (One-to-One)
            modelBuilder.Entity<LocalisationEntities>()
                        .HasOne(l => l.CapturesDetail)
                        .WithOne(cd => cd.Localisation)
                        .HasForeignKey<CaptureDetailsEntities>(cd => cd.LocalisationId);

            // CaptureEntities - CaptureDetailsEntities (One-to-Many)
            modelBuilder.Entity<CaptureEntities>()
                        .HasMany(c => c.CaptureDetails)
                        .WithOne(cd => cd.Capture)
                        .HasForeignKey(cd => cd.CaptureId);

            // CaptureEntities - EspesceEntities (One-to-Many)
            modelBuilder.Entity<CaptureEntities>()
                        .HasOne(c => c.Espece)
                        .WithMany(e => e.Captures)
                        .HasForeignKey(c => c.EspeceId);

            // CaptureEntities - UtilisateurEntities (One-to-Many)
            modelBuilder.Entity<CaptureEntities>()
                        .HasOne(c => c.Utilisateur)
                        .WithMany(u => u.Captures)
                        .HasForeignKey(c => c.UtilisateurId);

            // UtilisateurEntities - SuccesEntities (Many-to-Many)
            modelBuilder.Entity<UtilisateurEntities>()
                .HasMany(u => u.SuccesState)
                .WithOne(ss => ss.UtilisateurEntities)
                .HasForeignKey(ss => ss.UtilisateurId);

            modelBuilder.Entity<SuccesEntities>()
                .HasMany(s => s.SuccesStates)
                .WithOne(ss => ss.SuccesEntities)
                .HasForeignKey(ss => ss.SuccesEntitiesId);

            modelBuilder.Entity<SuccesStateEntities>()
                .HasIndex(ss => new { ss.SuccesEntitiesId, ss.UtilisateurId })
                .IsUnique();
        }
    }
}