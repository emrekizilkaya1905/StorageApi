# StorageApi

A simple product inventory REST API built with ASP.NET Core (.NET 10), Entity Framework Core, and SQL Server LocalDB. Swagger UI provides interactive API documentation.

## Getting started

1. Configure `ConnectionStrings:SqlServer` in `StorageApi/appsettings.Development.json` to use your LocalDB instance.
2. In Visual Studio's Package Manager Console, select `StorageApi` as the default project and run `Update-Database` to apply migrations.
3. Run the project with F5. Swagger UI opens at `/swagger`.

## Endpoints

| Method | Route | Description |
| --- | --- | --- |
| GET | `/api/products` | Get products, optionally filtered by category and name |
| GET | `/api/products/stats` | Get product count, total inventory value, and average price |
| GET | `/api/products/{id}` | Get a product |
| POST | `/api/products` | Create a product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

Use Postman or Swagger UI to test the endpoints. When creating a product, omit `id`; the database generates it automatically.
## Filtering and statistics

```http
GET /api/products?category=Electronics
GET /api/products?name=Phone
GET /api/products?category=Electronics&name=Phone
GET /api/products/stats
```

Category uses an exact match; name uses a partial match. Both filters can be combined. Omit filters to return all products.

Statistics include `totalProducts` (number of product records), `totalInventoryValue` (sum of price multiplied by stock count), and `averagePrice` (average product price). An empty inventory returns zeros.

Product responses contain `id`, `name`, `price`, `count`, and the calculated `inventoryValue`. POST and PUT accept product details without an ID in the body; PUT uses the ID in the route. Input validation rejects missing names, negative prices or counts, and text exceeding the configured length limits with HTTP 400.
