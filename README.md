# Sales Event Processor Function App

A serverless order-processing API built with **C#, Azure Functions, and SQL Server**. The application receives sales orders through an HTTP POST request, deserializes and validates the incoming JSON payload, and stores the order details in a SQL Server database using Dapper.

## Features

- **HTTP-triggered Azure Function:** Accepts sales orders through a POST endpoint.
- **JSON processing:** Deserializes incoming request bodies into a `SalesOrder` model.
- **Input validation:** Returns HTTP 400 when the order payload is invalid or cannot be deserialized into an order object.
- **SQL Server integration:** Inserts sales order details using Microsoft.Data.SqlClient and Dapper.
- **Asynchronous processing:** Uses async/await for request handling and database operations.
- **Logging and error handling:** Records successful processing and exceptions using `ILogger`.
- **HTTP responses:** Returns appropriate success, validation-error, or server-error responses.

## Technologies Used

- C#
- .NET and Azure Functions Isolated Worker
- Microsoft Azure Functions
- Microsoft SQL Server
- Microsoft.Data.SqlClient
- Dapper
- System.Text.Json
- Microsoft.Extensions.Logging

## How It Works

1. A client sends an HTTP POST request containing sales order data in JSON format.
2. The function reads the request body and deserializes it into a `SalesOrder` object.
3. If the order object is invalid, the function returns a `400 Bad Request` response.
4. The function retrieves the SQL connection string from the `SqlConnectionString` environment variable.
5. Dapper executes a parameterized SQL INSERT statement to save the order's customer name, total amount, and timestamp.
6. The function logs the successful operation and returns an HTTP 200 response.
7. If an exception occurs, the function logs the error and returns an HTTP 500 response.

## Example Request

**Method:** `POST`

**Content-Type:** `application/json`

Example JSON payload:

```json
{
  "customerName": "John Smith",
  "totalAmount": 1250.50,
  "timestamp": "2026-10-21T10:30:00Z"
}
```

The payload must match the properties and data types defined in the `SalesOrder` model.

## Database Setup

Create a SQL Server database and a `SalesOrders` table that matches the properties used by the function.

Example table definition:

```sql
CREATE TABLE SalesOrders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName NVARCHAR(200) NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    Timestamp DATETIME2 NOT NULL
);
```

Adjust the table definition if your `SalesOrder` model uses different data types or constraints.

## Configuration

Configure the database connection string using the `SqlConnectionString` environment variable.

For local development, add the setting to your local Azure Functions configuration file, `local.settings.json`, under `Values`:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "SqlConnectionString": "YOUR_SQL_CONNECTION_STRING"
  }
}
```

Replace the placeholder with your own connection string. Do not commit real credentials, passwords, or production connection strings to GitHub.

## Running Locally

1. Clone this repository:

   ```bash
   git clone https://github.com/YOUR_USERNAME/SalesEventProcessorFunctionApp.git
   ```

2. Open the solution in Visual Studio or your preferred .NET IDE.
3. Ensure the required .NET SDK and Azure Functions Core Tools are installed.
4. Configure the SQL connection string and local settings.
5. Ensure the `SalesOrders` table exists in the target database.
6. Run the Azure Functions project.
7. Send a POST request to the generated `ProcessOrder` endpoint using Postman, curl, or another HTTP client.

## Example Response

Successful request:

```text
Order for John Smith processed successfully.
```

Invalid order:

```text
Invalid order data.
```

Unexpected server error:

```text
Internal server error.
```

## Potential Improvements

- Add required-field validation and business rules for order amounts and timestamps.
- Return HTTP 201 Created when a new order is successfully inserted.
- Add duplicate-order prevention and idempotency handling.
- Use Azure Key Vault or managed identity for secure database access.
- Add automated unit and integration tests.
- Introduce structured logging, monitoring, and retry policies where appropriate.

## Purpose

This project demonstrates serverless API development, JSON request handling, asynchronous database access, SQL integration, configuration management, and exception handling using C# and Azure Functions.
