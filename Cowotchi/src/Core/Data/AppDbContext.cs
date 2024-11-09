using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

	public virtual DbSet<Log> Logs { get; set; }

	#endregion

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Animal>(entity =>
		{
			entity.Property(e => e.Id)
				.ValueGeneratedOnAdd();
			ConfigureAuditEntityFields(entity);
		});

		modelBuilder.Entity<AnimalEvent>(entity =>
		{
			entity.Property(e => e.Id)
				.ValueGeneratedOnAdd();
			ConfigureAuditEntityFields(entity);
		});

		modelBuilder.Entity<AnimalEventType>(entity =>
		{
			ConfigureAuditEntityFields(entity);
		});

		modelBuilder.Entity<AnimalType>(entity =>
		{
			ConfigureAuditEntityFields(entity);
		});

		modelBuilder.Entity<Egg>(entity =>
		{
			entity.Property(e => e.Id)
				.ValueGeneratedOnAdd();
			ConfigureAuditEntityFields(entity);
		});

		modelBuilder.Entity<HatchRequirement>(entity =>
		{
			entity.Property(e => e.Id)
				.ValueGeneratedOnAdd();
			ConfigureAuditEntityFields(entity);
		});

		modelBuilder.Entity<HatchRequirementType>(entity =>
		{
			ConfigureAuditEntityFields(entity);
		});

		modelBuilder.Entity<Log>(entity =>
		{
			entity.Property(e => e.Id)
				.ValueGeneratedOnAdd();
			ConfigureAuditEntityFields(entity);
		});
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

	private void ConfigureAuditEntityFields<TEntity>(EntityTypeBuilder<TEntity> entity) where TEntity : class
	{
		entity.Property<DateTime>("CreatedDate")
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		entity.Property<DateTime>("ModifiedDate")
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		entity.Property<bool>("IsDeleted")
			.HasDefaultValue(false);
	}
}
