#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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

		public SampleService(ILogger<SampleService> logger)
		{
			_logger = logger;
		}


	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
