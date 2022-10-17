; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Alcatraz Launcher"
#define MyAppVersion "1.5"
#define MyAppPublisher "Inspiration Byte"
#define MyAppURL "http://alcatraz.drivermadness.net/"
#define MyAppExeName "AlcatrazLauncher.exe"

[Setup]
; SignTool=MsSign $f
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{67DF25AB-7EF6-42C6-91BD-A0FC2513B6B8}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
InfoAfterFile=afterinstall.txt
; Uncomment the following line to run in non administrative install mode (install for current user only.)
PrivilegesRequired=lowest
OutputBaseFilename=AlcatrazLauncherSetup
Compression=zip/1
;SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkedonce

[Files]
Source: "..\AlcatrazLauncher\bin\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\AlcatrazLauncher.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\AlcatrazLauncher_SkipGameSearch.bat"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\AlcatrazLauncher_Uninstall.bat"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\Newtonsoft.Json.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\RestSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\RestSharp.Serializers.NewtonsoftJson.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\RestSharp.Serializers.NewtonsoftJson.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\RestSharp.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\AlcatrazLauncher\bin\Release\System.ComponentModel.Annotations.dll"; DestDir: "{app}"; Flags: ignoreversion

Source: "{code:GetGameDataDir}\ubiorbitapi_r2_loader.dll"; DestDir: "{code:GetGameDataDir}\backup"; Flags: external skipifsourcedoesntexist uninsneveruninstall
Source: "..\AlcatrazLoader\bin\Release\ubiorbitapi_r2_loader.dll"; DestDir: "{code:GetGameDataDir}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: runascurrentuser nowait postinstall skipifsilent

[Registry]
Root: HKCU32; Subkey: "Software\Alcatraz"; Flags: uninsdeletekeyifempty
Root: HKCU32; Subkey: "Software\Alcatraz\Launcher"; Flags: uninsdeletekey
Root: HKCU32; Subkey: "Software\Alcatraz\Launcher"; ValueType: string; ValueName: "GameLocation"; ValueData: "{code:GetGameDataDir}"

[Code]

var
  InstallationPath: string;
  ProgramFilesFolder: string;

function GetGameInstallationPath(): string;
begin
  { Detected path is cached, as this gets called multiple times }
  if InstallationPath = '' then
  begin
    if RegQueryStringValue(HKLM32, 'Software\Ubisoft\Driver San Francisco', 'GameLocation', InstallationPath) then
      begin
        Log('Detected installation: ' + InstallationPath);
      end
    else if RegQueryStringValue(HKLM32, 'Software\Ubisoft\Launcher\Installs\13', 'InstallDir', InstallationPath) then
      begin
        Log('Detected UbiLauncher installation: ' + InstallationPath);
      end
    else if RegQueryStringValue(HKCU32, 'Software\Alcatraz\Launcher', 'GameLocation', InstallationPath) then
      begin
        Log('Detected existing Alcatraz installation: ' + InstallationPath);
      end
    else
      begin
        InstallationPath := 'Please locate Driver San Francisco folder';
        Log('No installation detected, using the default path: ' + InstallationPath);
      end;
  end;
  Result := InstallationPath;
end;

var
  DataDirPage: TInputDirWizardPage;
procedure InitializeWizard;
begin
  { Create the page }

  DataDirPage := CreateInputDirPage(wpSelectDir,
    'Select Driver San Francisco install directory', 'Driver San Francisco location',
    'Select the folder where Driver San Franciso is located, ' +
      'then click Next.',
    False, '');
  DataDirPage.Add('');

  DataDirPage.Values[0] := GetGameInstallationPath()
end;

procedure RegisterPreviousData(PreviousDataKey: Integer);
begin
  { Store the selected folder for further reinstall/upgrade }
  SetPreviousData(PreviousDataKey, 'DataDir', DataDirPage.Values[0]);
end;

function GetGameDataDir(Param: String): String;
begin
  { Return the selected DataDir }
  if (DataDirPage = nil) then begin
    Result := '';
    exit;
  end;
  Result := DataDirPage.Values[0];
end;

function NextButtonClick(PageId: Integer): Boolean;
begin
  Result := True;
  { Alcatraz folder should not be same as DSF folder }
  if (PageId = wpSelectDir) and FileExists(RemoveBackslashUnlessRoot(WizardForm.DirEdit.Text) + '\Driver.exe') then begin
      MsgBox('It is not recommended to install Alcatraz into the game folder. Please select the another folder.', mbError, MB_OK);
      Result := False;
      exit;
  end
  else if (PageId = DataDirPage.ID) and not FileExists(RemoveBackslashUnlessRoot(GetGameDataDir('')) + '\Driver.exe') then begin
      MsgBox('Driver San Francisco executable is not found. Please select valid folder.', mbError, MB_OK);
      Result := False;
      exit;
  end;
end;

function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  S: String;
begin
  { Fill the 'Ready Memo' with the normal settings and the custom settings }
  S := '';

  S := S + MemoDirInfo + NewLine + NewLine;

  S := S + 'Driver San Francisco installation location:' + NewLine;
  S := S + Space + DataDirPage.Values[0] + NewLine;

  Result := S;
end;
