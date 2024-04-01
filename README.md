# Latex4CorelDRAW

This is a addon for CorelDRAW. It allows to add and edit Latex equations or symbols easily. The addon is based on ScintillaNET and supports syntax highlighting, code snippets etc.

The library was tested on Windows 10, CorelDRAW Graphics Suite 2021. 

**Author**: [Jan Bender](http://www.interactive-graphics.de), **License**: MIT

## Build Instructions

The included project file was made with Visual Studio 2019. 

1. Open the file "bonus630.CDRCommon.targets" and change the installation path of the desired version to your installation path.

2. Locate the MSBuild.exe, usually located at `C:\Windows\Microsoft.NET\Framework64\v4.0.30319`.

3. Run the command prompt or PowerShell in the folder as an administrator.

4. Copy the path to the "Latex4CorelDraw.sln" file located in the project folder.

5. In PowerShell, type the command `.\msbuild.exe "<path>" /p:Configuration="<configuration>"`, replacing `<path>` with the copied path and `<configuration>` with the desired installation. The available configurations are:
   - X7 Release
   - X8 Release
   - 2017 Release
   - 2018 Release
   - 2019 Release
   - 2020 Release
   - 2021 Release
   - 2022 Release
   - 2024 Release

6. In the command prompt, remove the initial `.\` from the command.


## Installation

1. Download the latest release and extract the zip into C:\Program Files\Corel\CorelDRAW Graphics Suite 2021\Programs64\Addons to install the addon.
2. Right-click and open "Properties" for each DLL file in the folder. If you see the following text, you have to unblock the file:
  "Security: This file came from another computer and might be blocked to help protect this computer."
3. Activate docker window "Latex" in the menu "Window" -> "Dockers".
4. If you have a better solution for step 2, write me an email ;-)

## Screenshots

<img src="screenshots/screenshot1.jpg" width="320">
