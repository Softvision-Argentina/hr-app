##Master builder##

$root_folder = Split-Path -Path $PSScriptRoot
$migrations_folder_path = (Join-Path $root_folder "\migrations")
$migrations_sql_files = Get-ChildItem $migrations_folder_path -Filter *.sql
$master_file_path = (Join-Path $root_folder "master.sql")
$newline = "`n"

Write-Host $newline
Write-Host "################ Start SQL Joiner ################" -ForegroundColor cyan
Write-Host $newline
Write-Host "/migrations folder path: " $migrations_folder_path -ForegroundColor green
Write-Host "/migrations sql files: "  $migrations_sql_files -ForegroundColor green
Write-Host "/master file path: "  $master_file_path -ForegroundColor green
Write-Host $newline

Write-Host "######## Start: Joining SQL files: " `n -ForegroundColor cyan

Clear-Content $master_file_path
Write-Host "Deleted content of master.sql" `n -ForegroundColor green

Add-Content -Path $master_file_path -Value "--Start: $(Get-Date -format 'u')"
Write-Host "Added start log" `n -ForegroundColor green

$migrations_sql_files | ForEach-Object {
    
    $migration_content = (Get-Content $_.FullName)
    $migration_title_start = $newline + "----Start:" + $_.Name + " / migration" + $newline
    $migration_title_end = $newline + "----End:" + $_.Name + " / migration" + $newline

    Add-Content -Path $master_file_path -Value $migration_title_start
    Add-Content -Path $master_file_path -Value $migration_content
    Add-Content -Path $master_file_path -Value $migration_title_end
    
    Write-Host "Wrote " $_.Name "migration in master.sql" `n -ForegroundColor green
}

Add-Content -Path $master_file_path -Value "--End: $(Get-Date -format 'u')"
Write-Host "Added end log" `n -ForegroundColor green

Write-Host "######## End: Joining SQL files: " `n -ForegroundColor cyan
