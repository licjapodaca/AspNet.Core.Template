#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AspNet.Core.Template.Context;
using AspNet.Core.Template.Models;
using AspNet.Core.Template.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.Core.Template.Repositories.Implementations
{
	public class SampleRepository : ISampleRepository
	{
		private readonly ILogger _logger;
		private readonly DataContext _ctx;

		public SampleRepository(ILogger<SampleRepository> logger, DataContext ctx)
		{
			_logger = logger;
			_ctx = ctx;
		}

		public async Task<List<SampleModel>> ObtenerDatosAsync()
		{
			try
			{
				return await Task.Run<List<SampleModel>>(() =>
				{
					var lista = new List<SampleModel>()
					{
						new SampleModel { Id = 1, FirstName = "Jorge", LastName = "Apodaca", DateOfBirth = DateTime.Parse("1977-02-22") },
						new SampleModel { Id = 2, FirstName = "Rodolfo", LastName = "Gomez", DateOfBirth = DateTime.Parse("1983-07-16") }
					};

					return lista;
				});
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
