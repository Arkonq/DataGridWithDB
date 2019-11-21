using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BindingLesson
{
	public class Context : DbContext
	{
		public Context()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=BorisHome\\Boris;Database=ShopDbTest;Trusted_Connection=true;");
		}

		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Name = "Bread",
					Price = 80,
					Quantity = 5
				},
				new Product
				{
					Name = "Milk",
					Price = 160,
					Quantity = 10
				},
				new Product
				{
					Name = "Coffee",
					Price = 60,
					Quantity = 15
				}
				);
		}
	}
}
