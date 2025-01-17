using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraFauna_GO_Entities
{
    public class FloraFaunaGoDB : DbContext
    {
        public virtual DbSet<EspeceEntities> Espece { get; set; }
        public virtual DbSet<CaptureEntities> Captures { get; set; }
        public virtual DbSet<CaptureDetailsEntities> CaptureDetails { get; set; }
        public virtual DbSet<HabitatEntities> Habitat { get; set; }
        public virtual DbSet<LocalisationEntities> Localisation { get; set; }
        public virtual DbSet<UtilisateurEntities> Utilisateur { get; set; }
        public virtual DbSet<SuccesEntities> Succes { get; set; }
        public virtual DbSet<SuccesStateEntities> SuccesState { get; set; }

        public FloraFaunaGoDB() { }

        public FloraFaunaGoDB(DbContextOptions<FloraFaunaGoDB> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
                options.UseSqlite("Data Source=FloraFaunaGoDB.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // EspeceEntities - LocalisationEntities (One-to-Many)
            modelBuilder.Entity<EspeceEntities>()
                .HasMany(e => e.Localisations)
                .WithOne(l => l.Espece)
                .HasForeignKey(l => l.EspeceId);

            // CaptureEntities - CaptureDetailsEntities (One-to-Many)
            modelBuilder.Entity<CaptureEntities>()
                .HasMany(c => c.CaptureDetails)
                .WithOne(cd => cd.Capture)
                .HasForeignKey(cd => cd.CaptureId);

            // CaptureEntities - UtilisateurEntities (Many-to-One)
            modelBuilder.Entity<CaptureEntities>()
                .HasOne(c => c.Utilisateur)
                .WithMany(u => u.Captures)
                .HasForeignKey(c => c.UtilisateurId);

            // UtilisateurEntities - SuccesStateEntities (One-to-Many)
            modelBuilder.Entity<UtilisateurEntities>()
                .HasMany(u => u.SuccesState)
                .WithOne(ss => ss.User)
                .HasForeignKey(ss => ss.UtilisateurId);

            // SuccesEntities - SuccesStateEntities (One-to-Many)
            modelBuilder.Entity<SuccesEntities>()
                .HasMany(s => s.State)
                .WithOne(ss => ss.SuccesEntities)
                .HasForeignKey(ss => ss.SuccesEntitiesId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
