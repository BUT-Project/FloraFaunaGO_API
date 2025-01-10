using Microsoft.EntityFrameworkCore;

namespace FloraFauna_GO_Entities;

public class FloraFaunaGoDB : DbContext
{
    public FloraFaunaGoDB()
    {
    }

    public FloraFaunaGoDB(DbContextOptions<FloraFaunaGoDB> options)
        : base(options)
    {
    }

    public virtual DbSet<EspeceEntities> Espece { get; set; }
    public virtual DbSet<CaptureEntities> Captures { get; set; }
    public virtual DbSet<CaptureDetailsEntities> CaptureDetails { get; set; }
    public virtual DbSet<HabitatEntities> Habitat { get; set; }
    public virtual DbSet<LocalisationEntities> Localisation { get; set; }
    public virtual DbSet<UtilisateurEntities> Utilisateur { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
            options.UseSqlite("Data Source=FloraFaunaGoDB.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Evènement obligatoire à déterminer

        base.OnModelCreating(modelBuilder);
    }
}