$androidPassword=$args[0]
$thumbPrint=$args[1]

write-host "Publish for android:"
write-host "-------------------------------"
dotnet publish src\MauiForKimai.App\MauiForKimai.App.csproj -f:net7.0-android -c:Release /p:AndroidSigningKeyPass=$androidPassword /p:AndroidSigningStorePass=$androidPassword
write-host "Publish for windows:"
write-host "-------------------------------"


dotnet publish src\MauiForKimai.App\MauiForKimai.App.csproj -f net7.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64 -p:PackageCertificateThumbprint=$thumbPrint
exit $LASTEXITCODE;