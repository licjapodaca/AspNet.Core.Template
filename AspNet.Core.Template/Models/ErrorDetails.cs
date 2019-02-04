#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Newtonsoft.Json;

namespace AspNet.Core.Template.Models
{
	public class ErrorDetails
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
		public ErrorDetails InnerException { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
