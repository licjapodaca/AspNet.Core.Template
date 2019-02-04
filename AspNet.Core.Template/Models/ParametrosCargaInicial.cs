#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using Newtonsoft.Json.Linq;

namespace AspNet.Core.Template.Models
{
	public class ParametrosCargaInicial
	{
		public string puertosMicroServicios;

		public string subdominio { get; set; }

		public JArray configuracionFolio { get; set; }

		public string domainServer { get; set; }
		public bool usarRutasRelativas { get; set; }
		public string httpProtocol { get; set; }
		public JArray rutasApi { get; set; }
		public JArray menuDinamico { get; set; }
		public JArray mensajes { get; set; }
		public bool inDevMode { get; set; }
		public JObject menuTree { get; set; }
		public JObject mascaras { get; set; }
		public JObject parametros { get; set; }
		public bool esEdge { get; set; }
		public bool esTenant { get; set; }
		public bool esWord { get; set; }
		public string url { get; set; }
		public string systrayWordProtocol { get; set; }
		public string systrayWordServerName { get; set; }
		public int? systrayWordPort { get; set; }
		public string systrayWordUri { get; set; }
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member