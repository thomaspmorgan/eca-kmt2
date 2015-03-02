..\packages\OpenCover.4.5.3723\OpenCover.Console.exe -register:user -target:"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\mstest.exe" -targetargs:"/noisolation /testcontainer:.\bin\Debug\ECA.Core.DynamicLinq.Test.dll" -filter:"+[ECA.Core.DynamicLinq*]* -[ECA.Core.DynamicLinq.Test*]*" -mergebyhash -output:results.xml
..\packages\ReportGenerator.2.1.1.0\ReportGenerator.exe "-reports:results.xml" "-targetdir:.\coverage"

pause