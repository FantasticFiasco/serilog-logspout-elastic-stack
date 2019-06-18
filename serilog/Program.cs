using System;
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
            // note: use of ElasticsearchJsonFormatter is optional (but recommended as it produces 'idiomatic' json)
            // If you don't want to take a dependency on 'Serilog.Formatting.Elasticsearch' package
            // you can also other json formatters such as Serilog.Formatting.Json.JsonFormatter

            // Console sink send logs to stdout which will then be read by logspout
            ILogger logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(new ElasticsearchJsonFormatter())
                .CreateLogger()
                .ForContext<Program>();

            var customerGenerator = new CustomerGenerator();
            var orderGenerator = new OrderGenerator();
            var exGenerator = new ExceptionGenerator();

            while (true)
            {
                var customer = customerGenerator.Generate();
                var order = orderGenerator.Generate();

                logger.Information("{@customer} placed {@order}", customer, order);

                var ex = exGenerator.Generate();
                if (ex != null) {
                    logger.Error(ex, "problem with {@order} placed by {@customer}", order, customer);
                }

                Thread.Sleep(1000);
            }
        }
    }
}
