using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using Bulky.DataAccess;

namespace BulkyWeb
{
	public class AppDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
			.Build();

			// Criando o DbContextOptionsBuilder manualmente
			var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
			// cria a connection string. 
			// requer a connectionstring no appsettings.json
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			builder.UseSqlServer(connectionString);

			// Cria o contexto
			return new ApplicationDbContext(builder.Options);
		}
	}
}
