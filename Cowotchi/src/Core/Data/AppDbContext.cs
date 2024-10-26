using Microsoft.EntityFrameworkCore;
using System;

public class AppDbContext : DbContext
{
	public AppDbContext()
	{
		Database.EnsureCreated();
	}

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public virtual DbSet<Animal> Animals { get; set; }
	public virtual DbSet<AnimalEvent> AnimalEvents { get; set; }
	public virtual DbSet<AnimalEventType> AnimalEventTypes { get; set; }
	public virtual DbSet<AnimalType> AnimalTypes { get; set; }

	public virtual DbSet<Egg> Eggs { get; set; }
	public virtual DbSet<HatchRequirement> HatchRequirements { get; set; }
	public virtual DbSet<HatchRequirementType> HatchRequirementTypes { get; set; }

	public virtual DbSet<Log> Logs { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Animal>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
		modelBuilder.Entity<AnimalEvent>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
		modelBuilder.Entity<AnimalEventType>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
		modelBuilder.Entity<AnimalType>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<Egg>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
		modelBuilder.Entity<HatchRequirement>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
		modelBuilder.Entity<HatchRequirementType>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<Log>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionString = Constants.ConnectionString;
		optionsBuilder.UseSqlite(connectionString);
	}
}
