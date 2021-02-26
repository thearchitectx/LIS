@ECHO off
for /r %%v in (*.xml) do ( d:\App\libxml2-2.7.8.win32\bin\xmllint.exe --noout --schema Cutscene.xsd "%%v" )