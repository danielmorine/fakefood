# Função Inline para verificar e deletar recursos
Write-Host "Starting resource cleanup..."

# Função para verificar e deletar um recurso baseado no arquivo YAML
function Delete-IfExists {
    param ([string]$YamlFilePath)
    if (Test-Path $YamlFilePath) {
        $resourceExists = kubectl get -f $YamlFilePath -o name > $null 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Deleting resource from: $YamlFilePath"
            kubectl delete -f $YamlFilePath
        } else {
            Write-Host "Resource not found: $YamlFilePath"
        }
    } else {
        Write-Host "YAML file not found: $YamlFilePath"
    }
}

# Deletar Configurações do SQL Server
Write-Host "Deleting SQL Server configurations..."
Delete-IfExists "k8s/mssql/service.yml"
Delete-IfExists "k8s/mssql/deployment.yml"
Delete-IfExists "k8s/mssql/persistenceVolumeClaim.yml"
Delete-IfExists "k8s/mssql/persistenceVolume.yml"


Write-Host "Resource cleanup completed."
