$identityTemplate = ".\identity\src\Identity.Functions\identity-template.yaml"
$identityPackagedTemplate = ".\identity\src\Identity.Functions\packaged-identity.yaml"
$budgetTemplate = ".\budget\src\Budget.Functions\budget-template.yaml"
$budgetPackagedTemplate = ".\budget\src\Budget.Functions\packaged-budget.yaml"
$messangerTemplate = ".\messanger\src\Messanger.Lambda\messanger-template.yaml"
$messangerPackagedTemplate = ".\messanger\src\Messanger.Lambda\packaged-messanger.yaml"
$identityStackName = "onyx-identity"
$budgetStackName = "onyx-budget"
$messangerStackName = "onyx-messanger"
$region = "eu-central-1"
$s3bucket = "onyx-default"

# Configuration section
aws configure set region $region

# Messanger Service
Write-Host "Packaging Messanger service..."
sam build --template $messangerTemplate
sam package --s3-bucket $s3bucket --output-template-file $messangerPackagedTemplate

Write-Host "Deploying Messanger service..."
sam deploy --template-file $messangerPackagedTemplate --stack-name $messangerStackName --capabilities CAPABILITY_IAM

$sendEmailTopicArn = (aws cloudformation describe-stacks --stack-name $messangerStackName --query "Stacks[0].Outputs[?OutputKey=='SendEmailTopicArn'].OutputValue" --output text --region $region)

if (-not $sendEmailTopicArn) {
    Write-Host "Error: Send Email topic Arn could not be retrieved. Exiting."
    exit 1
}

# Identity service
Write-Host "Packaging Identity service..."
sam build --template $identityTemplate
sam package --s3-bucket $s3bucket --output-template-file $identityPackagedTemplate

Write-Host "Deploying Identity service..."
sam deploy --template-file $identityPackagedTemplate --stack-name $identityStackName --capabilities CAPABILITY_IAM --parameter-overrides "SendEmailTopicArn=$sendEmailTopicArn" --region $region

$lambdaAuthorizerArn = (aws cloudformation describe-stacks --stack-name $identityStackName --query "Stacks[0].Outputs[?OutputKey=='LambdaAuthorizerArn'].OutputValue" --output text --region $region)

if (-not $lambdaAuthorizerArn) {
    Write-Host "Error: LambdaAuthorizerArn could not be retrieved. Exiting."
    exit 1
}

# Budget Service
Write-Host "Packaging Budget service..."
sam build --template $budgetTemplate
sam package --s3-bucket $s3bucket --output-template-file $budgetPackagedTemplate

Write-Host "Deploying Budget service..."
sam deploy --template-file $budgetPackagedTemplate --stack-name $budgetStackName --capabilities CAPABILITY_IAM --parameter-overrides "LambdaAuthorizerArn=$lambdaAuthorizerArn" --region $region

Write-Host "Deployment complete."
