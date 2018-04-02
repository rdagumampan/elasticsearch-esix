using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace esix
{
    public class EsSimpleClient
    {
        private readonly string _server;

        public EsSimpleClient(string server)
        {
            _server = server;
        }

        public List<EsIndex> GetIndices(string indexPrefix, string indexInterval)
        {
            //get all indices
            var requestIndicesUri = new RestRequest($"{_server}/_aliases?pretty=true");

            var client = new RestClient(_server);
            var responseIndices = client.Execute<List<dynamic>>(requestIndicesUri).Data;

            var indices = new List<EsIndex>();
            foreach (var responseIndex in responseIndices.First())
            {
                var esIndex = new EsIndex
                {
                    Name = responseIndex.Key,
                    Server = _server,
                    Interval = indexInterval
                };
                esIndex.Initialize();
                indices.Add(esIndex);
            }

            var filterdIndices = indices.Where(ix => ix.Name.Contains(indexPrefix));
            return filterdIndices.ToList();
        }
    }
}