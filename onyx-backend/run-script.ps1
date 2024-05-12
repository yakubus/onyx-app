$dockerImage = "onyx-budget-api:latest"
$dockerRegistryUrl = "docker.io"
$containerName = "onyx-budget"
$cosmosdbAccountUri = "https://onyx-cosmos-db.documents.azure.com:443/"
$cosmosdbDatabase = "budget"

do {
    $dockerRegistryUsername = Read-Host -Prompt "Enter your Docker registry username"
    $dockerRegistryPassword = Read-Host -Prompt "Enter your Docker registry password" -AsSecureString

    $dockerRegistryPasswordPlainText = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto([System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($dockerRegistryPassword))

    $loginResult = docker login -u $dockerRegistryUsername -p $dockerRegistryPasswordPlainText $dockerRegistryUrl

    Write-Host $loginResult
    
} until ($loginResult -like "*Login Succeeded*")

do {
    $cosmosdbPrimaryKey = Read-Host -Prompt "Enter Cosmos DB key" -AsSecureString
    $cosmosdbPrimaryKeyPlainText = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto([System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($cosmosdbPrimaryKey))

    if ($cosmosdbPrimaryKeyPlainText.Length -ne 88) {
        Write-Host "Invalid Cosmos DB key"
    }

} until ($cosmosdbPrimaryKeyPlainText.Length -eq 88)

docker run -p 8080:8080 -d `
  --name $containerName `
  -e "CosmosDb__AccountUri=$cosmosdbAccountUri" `
  -e "CosmosDb__PrimaryKey=$cosmosdbPrimaryKeyPlainText" `
  -e "CosmosDb__Database=$cosmosdbDatabase" `
  $dockerImage
