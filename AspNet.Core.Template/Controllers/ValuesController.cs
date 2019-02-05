using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Core.Template.Dto;
using AspNet.Core.Template.Services.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNet.Core.Template.Controllers
{
	/// <summary>
	/// Microservicio values
	/// </summary>
	[ApiVersion("1.0")]
	[Route("api/values")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly ISampleService _service;
		private readonly IMapper _mapper;

		/// <summary>
		/// Constructor del Microservicio
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="service"></param>
		/// <param name="mapper"></param>
		public ValuesController(
			ILogger<ValuesController> logger,
			IMapper mapper,
			ISampleService service)
		{
			_logger = logger;
			_service = service;
			_mapper = mapper;
		}

		// GET api/values
		/// <summary>
		/// Regreso de valores
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType(200)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Get()
		{
			try
			{
				var resultado = await _service.ObtenerDatosAsync();
				
				return Ok(_mapper.Map<IEnumerable<SampleDto>>(resultado));
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Testing GET
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<string> Get(int id)
		{
			return "value";
		}

		/// <summary>
		/// Testing POST
		/// </summary>
		/// <param name="value"></param>
		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		/// <summary>
		/// Testing PUT
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		/// <summary>
		/// Testing DELETE
		/// </summary>
		/// <param name="id"></param>
		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
