param(
    [Parameter(Mandatory=$False, Position=0)]
    [System.String]
    $Migration
)

Write-Host "Begin: Updating databases" `n -ForegroundColor cyan

$ProjectName = "ApiServer"
$DevelopmentEnvironment = "Development"
$IntegrationEnvironment = "IntegrationTest"
$FunctionalEnvironment = "FunctionalTest"
$EnvArray = $DevelopmentEnvironment, $IntegrationEnvironment, $FunctionalEnvironment

foreach ($Env in $EnvArray){  
    $env:ASPNETCORE_ENVIRONMENT = $Env
    Write-Host "ASPNETCORE_ENVIRONMENT is now: "$Env `n -ForegroundColor cyan
    Write-Host "Updating database with environment: " $Env `n -ForegroundColor cyan            
    update-database $Migration
}

$env:ASPNETCORE_ENVIRONMENT = ""
Write-Host "End: Updatin databases" `n -ForegroundColor green
