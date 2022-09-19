@REM rd /s /q C:\Users\{UserName}\.nuget\packages\vccli.net
@REM dotnet nuget locals all --clear
cd pack
dotnet restore
dotnet pack -c Release -o ../
@pause
