using CommandLine;

namespace esix
{
    public class BaseOptions
    {
        [Option('s', "server", Required = true, HelpText = "ElasticSearch server host")]
        public string Server { get; set; }

        //hourly, daily, monthly
        [Option('i', "interval", Required = true, HelpText = "Index interval")]
        public string IndexInterval { get; set; }

        [Option('p', "prefix", Required = true, HelpText = "Index prefix")]
        public string IndexPrefix { get; set; }

        //30d, 24h
        [Option('a', "age", Required = true, HelpText = "Index age")]
        public string IndexAge { get; set; }
    }
}