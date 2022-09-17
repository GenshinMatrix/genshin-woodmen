cd /d %~dp0
rename GenshinWoodmenSetup_*_x64.cer App.cer
rename GenshinWoodmenSetup_*_x64.msixbundle App.msixbundle
del App_Setup.exe
nsis\tools\makensis .\nsis\setup.nsi
del GenshinWoodmenSetup.exe
rename App_Setup.exe GenshinWoodmenSetup.exe
@pause
