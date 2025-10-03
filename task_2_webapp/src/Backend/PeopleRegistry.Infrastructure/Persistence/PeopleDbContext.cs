using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Backend.PeopleRegistry.Domain.Person;
using Backend.PeopleRegistry.Domain.Anschrift;
using Backend.PeopleRegistry.Domain.Telefonverbindung;
using System.Dynamic;

namespace Backend.PeopleRegistry.Infrastructure.Persistence;

public class PeopleDbContext : DbContext
{
    public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options) { }

    public DbSet<Person> Personen => Set<Person>();
    public DbSet<Anschrift> Anschriften => Set<Anschrift>();
    public DbSet<Telefonverbindung> Telefonverbindungs => Set<Telefonverbindung>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("dbo");

        b.Entity<Person>(e =>
        {
            e.ToTable("person");
            e.HasKey(x => x.Id).HasName("PK_Person");
            e.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            e.Property(x => x.Nachname).IsRequired().HasMaxLength(100);
            e.Property(x => x.Vorname).IsRequired().HasMaxLength(100);
            e.Property(x => x.Geburtsdatum).HasColumnType("date");
        });
        
        b.Entity<Anschrift>(e =>
        {
            e.ToTable("anschrift");
            e.HasKey(x => x.Id).HasName("PK_Anschrift");
            e.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            e.Property(x => x.Postleitzahl).IsRequired().HasMaxLength(10);
            e.Property(x => x.Ort).IsRequired().HasMaxLength(100);
            e.Property(x => x.Strasse).IsRequired().HasMaxLength(100);
            e.Property(x => x.Hausnummer).IsRequired().HasMaxLength(10);

            e.HasIndex(x => x.PersonId);
            e.HasOne(x => x.Person)
             .WithMany(p => p.Anschriften)
             .HasForeignKey(x => x.PersonId)
             .HasConstraintName("FK_Anschrift_Person")
             .OnDelete(DeleteBehavior.NoAction);
        });

        b.Entity<Telefonverbindung>(e =>
        {
            e.ToTable("telefonverbindung");
            e.HasKey(x => x.Id).HasName("PK_Telefonverbindung");
            e.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            e.Property(x => x.Telefonnummer).IsRequired().HasMaxLength(20);

            e.HasIndex(x => x.PersonId);
            e.HasOne(x => x.Person)
             .WithMany(p => p.Telefonverbindungen)
             .HasForeignKey(x => x.PersonId)
             .HasConstraintName("FK_Telefon_Person")
             .OnDelete(DeleteBehavior.NoAction);
        });
    
    }
}