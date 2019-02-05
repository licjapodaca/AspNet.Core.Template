#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace AspNet.Core.Template.Configurations.Logging
{
	public static class ConfigureLoggingServices
	{
		public static IWebHostBuilder ConfigureLoggingMicroservice(this IWebHostBuilder hostBuilder)
		{
			return hostBuilder.ConfigureLogging((hostingContext, logging) =>
			{
				logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				if (hostingContext.Configuration.GetValue("Loggers:Console:Enabled", false))
					logging.AddConsole();
				if (hostingContext.Configuration.GetValue("Loggers:Debug:Enabled", false))
					logging.AddDebug();
				if (hostingContext.Configuration.GetValue("Loggers:EventLog:Enabled", false))
				{
					logging.AddEventLog(new EventLogSettings
					{
						LogName = hostingContext.Configuration.GetValue<string>("Loggers:EventLog:LogName"),
						SourceName = hostingContext.Configuration.GetValue<string>("Loggers:EventLog:SourceName")
					});
				}
				logging.AddEventSourceLogger();
			});
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
