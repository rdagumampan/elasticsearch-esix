using System;
using System.Text.RegularExpressions;
using RestSharp;

namespace esix
{
    public class EsIndex
    {
        public string Name { get; set; }

        public string Server { get; set; }

        public string Interval { get; set; }

        public int Age { get; set; }

        public bool IsOnline { get; set; }

        public void Initialize()
        {
            GetState();
            GetAge();
        }

        private void GetState()
        {
            var requestnNodesUri = new RestRequest($"{this.Server}/{Name}/_stats");

            var client = new RestClient(Server);
            var response = client.Execute<dynamic>(requestnNodesUri).Data;

            if (!response.IsSuccessful)
            {
                throw new Exception($"Request index state failed. Data: {response.ErrorException}");
            }
        }

        private void GetAge()
        {
            var indexPattern = string.Empty;
            if (string.Equals(Interval.ToLower(), "hourly"))
            {
                //logstash-2014-04-19 10:00
                indexPattern = @"^\w+[-\.](\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2})$";
            }

            if (string.Equals(Interval.ToLower(), "daily"))
            {
                //logstash-2014-04-19
                indexPattern = @"^\w+[-\.](\d+)[-\.](\d+)[-\.](\d+)([-\.](\d+))*";
            }

            if (string.Equals(Interval.ToLower(), "monthly"))
            {
                //logstash-2014-04
                indexPattern = @"^\w+[-\.](\d+)[-\.](\d+)*";
            }

            var match = Regex.Match(Name, indexPattern);
            if (match.Success)
            {
                var indexDateUtc = new DateTime(Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value), 1);
                var indexAge = (int)(DateTime.UtcNow - indexDateUtc).TotalDays;

                Age = indexAge;
            }
        }

        public void Delete()
        {
            var requestnNodesUri = new RestRequest($"{this.Server}/{Name}");
            var client = new RestClient(Server);
            var response = client.Delete(requestnNodesUri);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Delete index request failed. Data: {response.ErrorException}");
            }
        }
    }
}