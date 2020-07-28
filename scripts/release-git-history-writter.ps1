cd "_recru-db\Sql"
git status
git checkout $(Build.SourceBranchName)
git add .
git commit -m "Save changes for build in last commit: $(Build.SourceVersion)"
git push
