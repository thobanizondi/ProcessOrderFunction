using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.IO;
using ProcessOrderFunction.Models;

namespace SalesEventProcessorFunctionApp
{
    public class ProcessOrderFunction
    {
        private readonly ILogger<ProcessOrderFunction> _logger;

        public ProcessOrderFunction(ILogger<ProcessOrderFunction> logger)
        {
            _logger = logger;
        }

        [Function("ProcessOrder")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            try
            {
                string body = await new StreamReader(req.Body).ReadToEndAsync();
                var order = JsonSerializer.Deserialize<SalesOrder>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (order == null)
                {
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("Invalid order data.");
                    return badResponse;
                }

                string connStr = Environment.GetEnvironmentVariable("SqlConnectionString");
                using var conn = new SqlConnection(connStr);

            
                const string sql = @"INSERT INTO SalesOrders (CustomerName, TotalAmount, Timestamp)
                                     VALUES (@CustomerName, @TotalAmount, @Timestamp);";

                await conn.ExecuteAsync(sql, new
                {
                    order.CustomerName,
                    order.TotalAmount,
                    order.Timestamp
                });

                _logger.LogInformation($"Order for {order.CustomerName} processed successfully.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync($"Order for {order.CustomerName} processed successfully.");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing order.");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Internal server error.");
                return errorResponse;
            }
        }
    }
}