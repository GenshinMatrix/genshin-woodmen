rd /s /q ..\src\GenshinWoodmen\bin\x64\Release\net8.0-windows10.0.19041.0\publish\win-x64\GenshinWoodmen
mkdir ..\src\GenshinWoodmen\bin\x64\Release\net8.0-windows10.0.19041.0\publish\win-x64\GenshinWoodmen
copy ..\src\GenshinWoodmen\bin\x64\Release\net8.0-windows10.0.19041.0\publish\win-x64\* ..\src\GenshinWoodmen\bin\x64\Release\net8.0-windows10.0.19041.0\publish\win-x64\GenshinWoodmen
7z a -mx9 GenshinWoodmen.7z ..\src\GenshinWoodmen\bin\x64\Release\net8.0-windows10.0.19041.0\publish\win-x64\GenshinWoodmen
@pause
