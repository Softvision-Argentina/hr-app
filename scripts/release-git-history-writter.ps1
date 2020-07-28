$env:GIT_REDIRECT_STDERR = '2>&1'
#This line prevent task to fail on git warnings

git config --global user.email "rodrigo.ramirez@softvision.com"
git config --global user.name "Rodrigo Ramirez"

cd "_recru-db\Sql"
git status
git checkout develop
git add .

$date = Get-Date
$build_number = $env:BUILD_BUILDNUMBER

git commit -m "Automatic commit on date: $date | Sent applied migration to history folder for release triggered by build number: $build_number"
git push
