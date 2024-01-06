rmdir /Q /S Template\Libraries

mkdir Template\Libraries
xcopy src\RedHerring\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Alexandria\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Assets\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Clues\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Deduction\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Fingerprint\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.ImGui\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Incident\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Infusion\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Motive\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Numbers\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Platforms\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Render\bin\Release\net8.0\*.dll Template\Libraries\ /Y
xcopy src\RedHerring.Serialization\bin\Release\net8.0\*.dll Template\Libraries\ /Y

@echo ================ DONE ===================
timeout /T 3