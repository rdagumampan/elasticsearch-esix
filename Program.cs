using CommandLine;

namespace esix
{
    public class Program
    {
        static int Main(string[] args)
        {
            // Initializes the command line arguments
            return Parser.Default.ParseArguments<DeleteOptions>(args)
              .MapResult(
                RunDeleteAndReturnExitCode,
                errs => 1);
        }

        private static int RunDeleteAndReturnExitCode(DeleteOptions opts)
        {
            var client = new EsSimpleClient(opts.Server);
            var indices = client.GetIndices(opts.IndexPrefix, opts.IndexInterval);

            foreach (var index in indices)
            {
                index.Delete();
            }

            return 1;
        }
    }
}
