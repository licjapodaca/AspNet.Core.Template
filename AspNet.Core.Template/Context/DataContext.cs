#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Microsoft.EntityFrameworkCore;

namespace AspNet.Core.Template.Context
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }
		
		#region DbSet region

		// TODO: Especificar las Entidades (DbSet) que utilizara el Microservicio

		#endregion

		/// <summary>
		/// Fluent API section
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
