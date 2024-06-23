$identityServicePath = ".\identity\src\Identity.Functions"
$identityTemplateFileName = "identity-template.yaml"
$budgetServicePath = ".\budget\src\Budget.Functions"
$budgetTemplateFileName = "budget-template.yaml"
$returnToBasePath = ".\..\..\.."
$identityStackName = "onyx-identity"
$budgetStackName = "onyx-budget"
$region = "eu-central-1"
$s3bucket = "onyx-default"

$identityPackagedTemplateFileName = "packaged-identity.yaml"
$budgetPackagedTemplateFileName = "packaged-budget.yaml"

aws configure set region $region

Set-Location $identityServicePath

Write-Host "Packaging Identity service..."
sam build --template $identityTemplateFileName
sam package --s3-bucket $s3bucket --output-template-file $identityPackagedTemplateFileName

Write-Host "Deploying Identity service..."
sam deploy --template-file $identityPackagedTemplateFileName --stack-name $identityStackName --capabilities CAPABILITY_IAM --region $region

$lambdaAuthorizerArn = (aws cloudformation describe-stacks --stack-name $identityStackName --query "Stacks[0].Outputs[?OutputKey=='LambdaAuthorizerArn'].OutputValue" --output text --region $region)

if (-not $lambdaAuthorizerArn) {
    Write-Host "Error: LambdaAuthorizerArn could not be retrieved. Exiting."
    exit 1
}

Set-Location $returnToBasePath
Set-Location $budgetServicePath

Write-Host "Packaging Budget service..."
sam build --template $budgetTemplateFileName
sam package --s3-bucket $s3bucket --output-template-file $budgetPackagedTemplateFileName

Write-Host "Deploying Budget service..."
sam deploy --template-file $budgetPackagedTemplateFileName --stack-name $budgetStackName --capabilities CAPABILITY_IAM --parameter-overrides "LambdaAuthorizerArn=$lambdaAuthorizerArn" --region $region

Write-Host "Deployment complete."
