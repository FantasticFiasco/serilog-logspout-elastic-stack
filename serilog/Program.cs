using System;
using System.Threading;
using Serilog;
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
                .WriteTo.Console(new ElasticsearchJsonFormatter())
                .CreateLogger()
                .ForContext<Program>();

            var customerGenerator = new CustomerGenerator();
            var orderGenerator = new OrderGenerator();

            while (true)
            {
                var customer = customerGenerator.Generate();
                var order = orderGenerator.Generate();

                logger.Information("{@customer} placed {@order}", customer, order);

                Thread.Sleep(1000);
            }
        }
    }
}
