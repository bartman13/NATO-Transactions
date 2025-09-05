# ============================
# CONFIGURATION
# ============================
$containerName = "transactions-postgres"
$dbUser = "admin"
$dbPassword = "YourStrongPass123"
$dbName = "TransactionsDb"
$dbPortMapping = "5432:5432"

# ============================
# 1. REMOVE EXISTING CONTAINER (if exists)
# ============================
if ($(docker ps -a -q -f name=$containerName)) {
    Write-Host "Stopping and removing existing container $containerName..."
    docker stop $containerName | Out-Null
    docker rm $containerName | Out-Null
}

# ============================
# 2. RUN NEW POSTGRES CONTAINER
# ============================q
Write-Host "=== Starting fresh PostgreSQL container ==="

docker pull postgres:15

docker run -d --name $containerName -e POSTGRES_USER=$dbUser -e POSTGRES_PASSWORD=$dbPassword  -e POSTGRES_DB=$dbName  -p $dbPortMapping  postgres:15

# ============================
# 3. WAIT FOR DATABASE TO INITIALIZE
# ============================
Write-Host "=== Waiting for PostgreSQL to initialize... ==="
Start-Sleep -Seconds 5

# ============================
# 4. APPLY EF CORE MIGRATIONS (create fresh schema)
# ============================
Write-Host "=== Applying EF Core migrations to create database ==="
dotnet ef database update `
    --project .\src\Infrastructure\Infrastructure `
    --startup-project .\src\API\Transactions.csproj