@echo OFF
echo :
echo : XCOPY Batch Process Started
echo :
xcopy ..\zeptotrail\Assets\zeptolib\ . /E /D /Y
echo :
xcopy .\ ..\zeptotrail\Assets\zeptolib\ /E /D /Y
rmdir ..\zeptotrail\Assets\zeptolib\bin /s /q
rmdir ..\zeptotrail\Assets\zeptolib\obj /s /q
rmdir ..\zeptotrail\Assets\zeptolib\.vscode /s /q
del ..\zeptotrail\Assets\zeptolib\zeptolib.csproj
del ..\zeptotrail\Assets\zeptolib\.gitignore
del ..\zeptotrail\Assets\zeptolib\*.bat
del *.meta
echo :
echo : XCOPY Batch Process Complete
echo :