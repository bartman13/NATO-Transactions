# 💼 Interview Task – Transactions API with PostgreSQL  

This project is an **interview task** that demonstrates building a clean ASP.NET Core Web API with **PostgreSQL** running inside a Docker container.  
Entity Framework Core is used for migrations and database schema updates.  

## 🐳 Docker Database Setup


```powershell
docker pull postgres:15

docker run -d --name transactions-postgres `
  -e POSTGRES_USER=admin `
  -e POSTGRES_PASSWORD=YourStrongPass123 `
  -e POSTGRES_DB=TransactionsDb `
  -p 5432:5432 `
  postgres:15

🔗 Connection String

Configured in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=TransactionsDb;Username=admin;Password=YourStrongPass123"
}

