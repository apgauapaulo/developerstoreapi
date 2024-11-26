
# Sales API

This project is an API for managing sales, including creating, updating, and canceling sales. The API supports CRUD operations and applies business rules like calculating discounts and restricting the number of items in a sale.

## Prerequisites

To run this project, you will need the following installed on your machine:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 7.0 or higher)
- [Postman](https://www.postman.com/downloads/) (optional, for testing the API)
- A PostgreSQL database (for connecting to the default context)

## Setup

### Clone the Repository

1. Clone this repository to your local machine:
   ```bash
   git clone https://github.com/apgauapaulo/Ambev.DeveloperEvaluation.git
   ```

### Configure the Database

1. Create a PostgreSQL database instance.
2. Update the connection string in the `appsettings.json` file:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Username=yourusername;Password=yourpassword;Database=yourdatabase"
   }
   ```

### Run Migrations

Run the following command to apply database migrations:
   ```bash
   dotnet ef database update
   ```

### Run the Application

To start the application, run the following command:
   ```bash
   dotnet run
   ```

This will start the API on `https://localhost:5001` (or a different port if configured).

## Testing the API

You can test the API using [Swagger UI](https://localhost:5001/swagger) or [Postman](https://www.postman.com/downloads/).

### Example Endpoints

Here are some example API endpoints that you can use to test the application:

#### **POST /api/Vendas**

Create a new sale. Sample JSON body:
```json
{
  "customer": "Customer A",
  "branch": "Branch A",
  "items": [
    { "productName": "Product 1", "quantity": 2, "totalAmount": 50 }
  ]
}
```

#### **GET /api/Vendas/{id}**

Get a sale by ID.

#### **PUT /api/Vendas/{id}**

Update a sale. Sample JSON body:
```json
{
  "customer": "Customer B",
  "branch": "Branch B",
  "items": [
    { "productName": "Product 2", "quantity": 3, "totalAmount": 100 }
  ]
}
```

#### **DELETE /api/Vendas/{id}**

Cancel a sale by ID.

### Testing with Swagger UI

1. Run the project with the command `dotnet run`.
2. Open your browser and navigate to [Swagger UI](https://localhost:5001/swagger).
3. You'll see all available endpoints listed. You can interact with each endpoint by clicking "Try it out" and sending requests directly from the UI.

### Testing with Postman

1. Open [Postman](https://www.postman.com/downloads/).
2. Create a new request with the appropriate HTTP method (GET, POST, PUT, DELETE).
3. For POST and PUT requests, include a JSON body with the necessary parameters, as shown in the examples above.
4. Click "Send" to see the response from the API.

## Error Handling

- The API returns standard HTTP status codes to indicate the status of requests:
  - `200 OK`: The request was successful.
  - `201 Created`: The resource was successfully created.
  - `400 Bad Request`: The request was invalid (e.g., missing or incorrect parameters).
  - `404 Not Found`: The resource was not found.
  - `500 Internal Server Error`: An error occurred on the server.