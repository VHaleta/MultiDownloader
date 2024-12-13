using Newtonsoft.Json.Linq;

namespace MultiDownloader.DatabaseApi.Host.Models
{
    public class GraphQlQuery
    {
        public string OperationName { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
