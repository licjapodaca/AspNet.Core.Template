﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AspNet.Core.Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.Core.Template.Services.Contracts
{
	public interface ISampleService
	{
		Task<List<SampleModel>> ObtenerDatosAsync();
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
