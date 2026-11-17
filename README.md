# StorageApi

A simple product inventory REST API built with ASP.NET Core (.NET 10), Entity Framework Core, and SQL Server LocalDB. Scalar provides interactive API documentation.

## Getting started

1. Configure `ConnectionStrings:SqlServer` in `StorageApi/appsettings.Development.json` to use your LocalDB instance.
2. In Visual Studio's Package Manager Console, select `StorageApi` as the default project and run `Update-Database` to apply migrations.
3. Run the project with F5. Scalar opens at `/scalar`.

## Endpoints

| Method | Route | Description |
| --- | --- | --- |
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get a product |
| POST | `/api/products` | Create a product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

Use Postman or Scalar to test the endpoints. When creating a product, omit `id`; the database generates it automatically.
