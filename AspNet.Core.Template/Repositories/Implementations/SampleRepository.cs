#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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

		public SampleRepository(ILogger<SampleRepository> logger)
		{
			_logger = logger;
		}

		public async Task<List<string>> ObtenerDatosAsync()
		{
			var lista = new List<string>();

			try
			{
				return await Task.Run<List<string>>(() =>
				{
					lista.Add("Uno");
					lista.Add("Dos");

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
