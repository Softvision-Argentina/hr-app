cd "_recru-db\Sql"
git status
echo !$(Build.SourceBranchName)
git checkout develop
git add .
git commit -m "Save changes for build in last commit: $(Build.SourceVersion)"
git push
