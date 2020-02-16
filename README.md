# DontPoisonMySource
**Simple tool to check Visual Studio project files for Exec, PreBuildEvent and PostBuildEvent**

## Usage
Using command line arguments:

    DontPoisonMySource.exe file1.csproj file2.vcxproj ...
The tool will check the files for Exec, PreBuildEvent and PostBuildEvent parameters using Regex Match

    <(.*)Exec(.*?)>
    <PreBuildEvent>(.*?)</PreBuildEvent>
    <PostBuildEvent>(.*?)</PostBuildEvent>
   Any match will be printed in the console like this
   ![enter image description here](https://i.imgur.com/D6rg9Pg.png)
   Format: 

      1. [!] {parameter} ⯈ {filepath}
      2.  ↳ {content of parameter}

Files can be opened via drag onto the executable.

## Dependencies
- [Colorful.Console](https://github.com/tomakita/Colorful.Console)
- [Costura](https://github.com/Fody/Costura)
