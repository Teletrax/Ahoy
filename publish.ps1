Get-ChildItem . -Filter project.json -Recurse | 
    ForEach-Object{dotnet pack $_.FullName}
    
Get-ChildItem . -Filter Swashbuckle.AspNetCore.*.nupkg -Exclude *.symbols.nupkg,*Test* -Recurse | 
    ForEach-Object{Invoke-WebRequest -Method PUT -Uri "http://artifactory-01.teletrax.com/nuget-release-local/3dparty/$($_.Name.Substring(0,$_.Name.Length - 16))/$($_.Name)" -UseBasicParsing -InFile $_.FullName}