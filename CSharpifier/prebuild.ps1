param(
    [Parameter(Mandatory=$true)]
    [string]
    $SolutionDir
)

echo "[PreBuild Started]"
Write-Host "Solution Dir: $SolutionDir"

$OriginalParserPath = "$SolutionDir..\CPPWithCXParser"
$TargetParserPath = "$SolutionDir\CSharpifier\CPPWithCXParser"

Write-Host "cd to $OriginalParserPath"
Set-Location $OriginalParserPath

Write-Host "Generating C++/CX Parser" -ForegroundColor Yellow
.\gen_parser.ps1
Write-Host "C++/CX Parser Generated" -ForegroundColor Green

Write-Host "cd to $SolutionDir"
Set-Location $SolutionDir

xcopy "$OriginalParserPath\CPPCXLexer.cs" "$TargetParserPath\" /d /y
xcopy "$OriginalParserPath\CPPCXParser.cs" "$TargetParserPath\" /d /y
xcopy "$OriginalParserPath\CPPCXParserBaseListener.cs" "$TargetParserPath\" /d /y
xcopy "$OriginalParserPath\CPPCXParserListener.cs" "$TargetParserPath\" /d /y
xcopy "$OriginalParserPath\CPPCXParserBaseVisitor.cs" "$TargetParserPath\" /d /y
xcopy "$OriginalParserPath\CPPCXParserVisitor.cs" "$TargetParserPath\" /d /y


echo "[PreBuild Completed]"
