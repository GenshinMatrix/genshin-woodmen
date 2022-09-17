!define PRODUCT_NAME                     "App"
!define PRODUCT_VERSION                  "1.0.0.0"
!define PRODUCT_PUBLISHER                "ema"
!define PRODUCT_WEB_SITE                 "https://github.com/emako"
!define PRODUCT_LEGAL                    "Licensed under MIT"

VIProductVersion                        "${PRODUCT_VERSION}"
VIAddVersionKey "ProductVersion"        "${PRODUCT_VERSION}"
VIAddVersionKey "ProductName"           "${PRODUCT_NAME}"
VIAddVersionKey "CompanyName"           "${PRODUCT_PUBLISHER}"
VIAddVersionKey "FileVersion"           "${PRODUCT_VERSION}"
VIAddVersionKey "InternalName"          "${PRODUCT_NAME}"
VIAddVersionKey "FileDescription"       "${PRODUCT_NAME}_Setup"
VIAddVersionKey "Comments"              "${PRODUCT_WEB_SITE}"
VIAddVersionKey "LegalCopyright"        "${PRODUCT_LEGAL}"

!addplugindir plugins

Icon "favicon.ico"
Name "${PRODUCT_NAME}"
OutFile "..\${PRODUCT_NAME}_Setup.exe"
RequestExecutionLevel admin
Page custom MsixSetup

Function MsixSetup
	Call CheckMutex
	InitPluginsDir
	SetOutPath "$PLUGINSDIR"
	File "tools\certmgr.exe"
	File "..\app.cer"
	nsExec::ExecToLog 'certmgr.exe -add app.cer -s -r localMachine AuthRoot'
	File "tools\msixexec.exe"
	File "..\app.msixbundle"
	nsExec::ExecToLog 'msixexec.exe app.msixbundle'
FunctionEnd

Function CheckMutex
	System::Call 'kernel32::CreateMutexA(i 0, i 0, t "${PRODUCT_NAME}_SetupMutex") i .r1 ?e'
	Pop $R0
	StrCmp $R0 0 +3
	MessageBox MB_OK|MB_ICONEXCLAMATION "Setup is already running."
	Abort
FunctionEnd

Section
SectionEnd
