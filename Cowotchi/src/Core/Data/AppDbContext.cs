using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class AppDbContext : DbContext
{
	public AppDbContext()
	{
		Database.EnsureCreated();
	}

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
	{ 
		Database.EnsureCreated();
	}
	
	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		AddAuditInfo();

		return await base.SaveChangesAsync(cancellationToken);
	}

	#region Tables

	public virtual DbSet<Animal> Animals { get; set; }
	public virtual DbSet<AnimalEvent> AnimalEvents { get; set; }
	public virtual DbSet<AnimalEventType> AnimalEventTypes { get; set; }
	public virtual DbSet<AnimalType> AnimalTypes { get; set; }

	public virtual DbSet<Egg> Eggs { get; set; }
	public virtual DbSet<HatchRequirement> HatchRequirements { get; set; }
	public virtual DbSet<HatchRequirementType> HatchRequirementTypes { get; set; }
	public virtual DbSet<NameOption> NameOptions { get; set; }

	public virtual DbSet<Log> Logs { get; set; }

	#endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Animal>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
		modelBuilder.Entity<AnimalEvent>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<Egg>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
		modelBuilder.Entity<HatchRequirement>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
		modelBuilder.Entity<NameOption>()
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
	
	private void AddAuditInfo()
	{
		var entities = ChangeTracker.Entries<AuditEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
		var utcNow = DateTime.Now;
		
		foreach (var entity in entities)
		{
			if (entity.State == EntityState.Added)
			{
				entity.Entity.CreatedDate = utcNow;
				entity.Entity.ModifiedDate = utcNow;
			}

			if (entity.State == EntityState.Modified)
			{
				entity.Entity.ModifiedDate = utcNow;
			}
		}
	}
}
