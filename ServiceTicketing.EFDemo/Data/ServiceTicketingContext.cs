using Microsoft.EntityFrameworkCore;
using ServiceTicketing.EFDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceTicketing.EFDemo.Data
{
	public class ServiceTicketingContext : DbContext
	{
		public DbSet<User> Users => Set<User>();
		public DbSet<Category> Categories => Set<Category>();
		public DbSet<Ticket> Tickets => Set<Ticket>();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// NOTE: Replace the connection string to match your SQL Server environment.
			// This is intentionally explicit for Week 6 so students see where the connection is set.
			optionsBuilder.UseSqlServer(
				"Server=localhost;Database=IT410_ServiceTicketing_EfDemo;Trusted_Connection=True;TrustServerCertificate=True"
			)
			.EnableSensitiveDataLogging()
			.LogTo(Console.WriteLine);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Week 6: Minimal, intentional Fluent API configuration (no relationships yet).

			modelBuilder.Entity<User>(entity =>
			{
				entity.Property(u => u.FullName)
				.IsRequired();

				entity.Property(u => u.Email)
				.IsRequired()
				.HasMaxLength(255);

				entity.Property(u => u.CreatedOn)
				.IsRequired();

				entity.Property(u => u.PhoneNumber)
				.HasMaxLength(20);
			});

			modelBuilder.Entity<Category>(entity =>
			{
				entity.Property(c => c.Name)
				.IsRequired()
				.HasMaxLength(100);

				// Description intentionally optional
			});

			modelBuilder.Entity<Ticket>(entity =>
			{
				entity.Property(t => t.Title)
				.IsRequired()
				.HasMaxLength(200);

				entity.Property(t => t.Description)
				.IsRequired();

				entity.Property(t => t.Status)
				.IsRequired()
				.HasMaxLength(30);

				entity.Property(t => t.Priority)
				.IsRequired()
				.HasMaxLength(30);

				entity.Property(t => t.CreatedOn)
				.IsRequired();
			});
		}
	}
}
