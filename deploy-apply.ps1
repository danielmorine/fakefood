# Apply shared configurations
kubectl apply -f k8s/shared/configmap.yml

# Apply SQL Server configurations
kubectl apply -f k8s/mssql/persistenceVolume.yml
kubectl apply -f k8s/mssql/persistenceVolumeClaim.yml
kubectl apply -f k8s/mssql/deployment.yml
kubectl apply -f k8s/mssql/service.yml

# Wait for SQL Server to be ready
Write-Host "Waiting for SQL Server to be ready..."
kubectl wait --for=condition=available --timeout=600s deployment/mssql

# Display current date and time
Write-Host "Waiting 10 seconds for database to be UP: $(Get-Date)"

# Wait for 10 seconds to ensure SQL Server is fully up
Start-Sleep -Seconds 10

# Execute SQL script to create database structure
Write-Host "Creating database structure $(Get-Date)..."
Invoke-Sqlcmd -ConnectionString "Server=localhost,1433;Database=master;User ID=sa;Password=sql@123456;TrustServerCertificate=True" -InputFile ".\k8s\create-database.sql"

