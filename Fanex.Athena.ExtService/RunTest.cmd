dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,cobertura\" /p:ExcludeByFile=\"../Fanex.Athena.ExtService.BetList/Models/DTO/*.cs,../Fanex.Athena.ExtService.Resources/*.cs,../Fanex.Athena.ExtService/**/*.cs\"
 /p:ExcludeByAttribute=\"Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute\"
dotnet ".\reportgenerator\4.2.19\tools\netcoreapp2.1\ReportGenerator.dll" "-reports:.\**\*.opencover.xml" "-targetdir:..\TestReports"
