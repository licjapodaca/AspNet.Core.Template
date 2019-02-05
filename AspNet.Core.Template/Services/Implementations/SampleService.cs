#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AspNet.Core.Template.Repositories.Contracts;
using AspNet.Core.Template.Services.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.Core.Template.Services.Implementations
{
	public class SampleService : ISampleService
	{
		private readonly ILogger _logger;
		private readonly ISampleRepository _repository;

		public SampleService(ILogger<SampleService> logger, ISampleRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}

		public async Task<List<string>> ObtenerDatosAsync()
		{
			try
			{
				return await _repository.ObtenerDatosAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
