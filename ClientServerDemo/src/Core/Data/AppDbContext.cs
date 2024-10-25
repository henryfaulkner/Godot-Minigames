using Microsoft.EntityFrameworkCore;
using System;

public class AppDbContext : DbContext
{
	public AppDbContext()
	{
		Database.EnsureCreated();
	}

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public virtual DbSet<Player> Players { get; set; }
	public virtual DbSet<Log> Logs { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Player>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<Log>()
			.Property(g => g.Id)
			.ValueGeneratedOnAdd();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionString = "Data Source=app_database.db;";
		optionsBuilder.UseSqlite(connectionString);
	}
}
