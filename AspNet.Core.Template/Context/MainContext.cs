#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.Core.Template.Context
{
	public class MainContext : DbContext
	{
		public MainContext(DbContextOptions<MainContext> options) : base(options) { }
		
		#region DbSet region



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
