$current_folder_path = $PSScriptRoot
$in_folder_path = (Join-Path $current_folder_path "\in")
$out_folder_path = (Join-Path $current_folder_path "\out")
$history_folder_path = (Join-Path $current_folder_path "\history")
$in_sql_files = Get-ChildItem $in_folder_path -Filter *.sql
$master_file_path = (Join-Path $current_folder_path "master.sql")
$master_history_name = $history_folder_path + "\master-" + (Get-Date -f ddMMyyyy-HH-mm) + ".sql"
$newline = "`n"

Write-Host $newline
Write-Host "################ Start Migration cleanup ################" -ForegroundColor cyan
Write-Host $newline
Write-Host "sql-migrator/in folder path: " $in_folder_path -ForegroundColor green
Write-Host "sql-migrator/out folder path: "  $out_folder_path -ForegroundColor green
Write-Host "sql-migrator/in sql files: "  $in_sql_files -ForegroundColor green
Write-Host "sql-migrator/master file path: "  $master_file_path -ForegroundColor green
Write-Host $newline

Write-Host "######## Start Migration cleanup: ########" `n -ForegroundColor cyan
Move-Item -Path (Join-Path $in_folder_path "\*.sql") -Destination $out_folder_path
Write-Host "Moved items from $($in_folder_path) to $($out_folder_path)" -ForegroundColor green
Copy-Item -Path $master_file_path -Destination $master_history_name
Write-Host "Copy master.sql to history\$($master_history_name)" -ForegroundColor green
Remove-Item (Join-Path $in_folder_path "\*.sql")
Write-Host "Deleted all sql from in folder" -ForegroundColor green
Clear-Content $master_file_path
Write-Host "Clean master file" -ForegroundColor green
Write-Host "######## End Migration cleanup: ########" `n -ForegroundColor cyan