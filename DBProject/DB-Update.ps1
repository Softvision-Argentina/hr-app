param(
    [Parameter(Mandatory=$False, Position=0)]
    [System.String]
    $Migration
)

Write-Host "Begin: updating databases" `n -ForegroundColor cyan

$ProjectName = "ApiServer"
$DevelopmentEnvironment = "Development"
$IntegrationEnvironment = "IntegrationTest"
$EnvArray = $DevelopmentEnvironment, $IntegrationEnvironment

foreach ($Env in $EnvArray){  
    $env:ASPNETCORE_ENVIRONMENT = $Env
    Write-Host "ASPNETCORE_ENVIRONMENT is now: "$Env `n -ForegroundColor cyan
    Write-Host "Updating database with environment : " $Env `n -ForegroundColor cyan            
    update-database $Migration
}

$env:ASPNETCORE_ENVIRONMENT = $DevelopmentEnvironment
Write-Host "End: Updated databases, ASPNETCORE_ENVIRONMENT is $env:ASPNETCORE_ENVIRONMENT" `n -ForegroundColor green
