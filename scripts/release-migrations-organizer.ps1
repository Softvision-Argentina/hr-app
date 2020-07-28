
#folder paths
$root_folder = Split-Path -Path $PSScriptRoot
$migrations_folder_path = (Join-Path $root_folder "\migrations")
$history_masters_folder_path = (Join-Path $root_folder "history\masters_applied")
$history_migrations_folder_path = (Join-Path $root_folder "history\migrations_applied")
$master_file_path = (Join-Path $root_folder "master.sql")

#names
$master_history_name = $history_masters_folder_path + "\master-" + (Get-Date -f ddMMyyyy-HH-mm) + ".sql"

#utils
$newline = "`n"

Write-Host "################ Start RELEASE MIGRATION ORGANIZER ################" -ForegroundColor cyan
Write-Host $newline

Write-Host "\Root folder path: " $root_folder -ForegroundColor green
Write-Host "\migrations folder path: " $migrations_folder_path -ForegroundColor green
Write-Host "\history\masters_applied folder path: " $history_masters_folder_path -ForegroundColor green
Write-Host "\history\migrations_applied folder path: " $history_masters_folder_path -ForegroundColor green
Write-Host "\master.sql file path: " $master_file_path -ForegroundColor green
Write-Host $newline

Write-Host "######## Start Migration cleanup: ########" `n -ForegroundColor cyan

Copy-Item -Path (Join-Path $migrations_folder_path "\*.sql") -Destination $history_migrations_folder_path
Write-Host "Copy migrations to $history_migrations_folder_path" -ForegroundColor green

Copy-Item -Path $master_file_path -Destination $master_history_name
Write-Host "Copy master.sql to $history_masters_folder_path\$($master_history_name)" -ForegroundColor green

Remove-Item (Join-Path $migrations_folder_path "\*.sql")
Write-Host "Deleted all sql from in folder" -ForegroundColor green

Clear-Content $master_file_path
Write-Host "Clean master file" -ForegroundColor green
Write-Host $newline

Write-Host "######## End Migration cleanup: ########" `n -ForegroundColor cyan
