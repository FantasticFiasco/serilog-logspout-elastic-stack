using System.Threading;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using SerilogExample.Generators;

namespace SerilogExample
{
    public class Program
    {
        static void Main()
        {
            // NOTE: Use of ElasticsearchJsonFormatter is optional (but recommended as it produces
            // 'idiomatic' json). If you don't want to take a dependency on
            // Serilog.Formatting.Elasticsearch package you can also use other json formatters
            // such as Serilog.Formatting.Json.JsonFormatter.

            // Console sink send logs to stdout which will then be read by logspout
            ILogger logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(new ElasticsearchJsonFormatter())
                .CreateLogger()
                .ForContext<Program>();

            var customerGenerator = new CustomerGenerator();
            var orderGenerator = new OrderGenerator();
            var exceptionGenerator = new ExceptionGenerator();

            while (true)
            {
                var customer = customerGenerator.Generate();
                var order = orderGenerator.Generate();

                logger.Information("{@customer} placed {@order}", customer, order);

                var exception = exceptionGenerator.Generate();
                if (exception != null)
                {
                    logger.Error(exception, "Problem with {@order} placed by {@customer}", order, customer);
                }

                Thread.Sleep(1000);
            }
        }
    }
}
