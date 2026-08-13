# 릴리스 빌드 스크립트
# .NET SDK 의 dotnet 로 빌드한다. VSCode(C# 확장)와 동일한 툴체인이라 인텔리센스와 일관된다.
# 산출물은 .NET Framework 4.0 단일 exe 이며 런타임 종속 DLL 을 포함하지 않는다.
# 빌드 전용 참조 어셈블리 패키지는 최초 1회 인터넷 복원이 필요하며 이후 캐시로 오프라인 빌드된다.

$ErrorActionPreference = 'Stop'

$projPath = Join-Path $PSScriptRoot '..\src\MultiMonitorCapture.csproj'
if (-not (Test-Path $projPath)) {
    Write-Error "프로젝트 파일을 찾을 수 없습니다: $projPath"
}

# dotnet 실행 파일 탐색
$dotnet = (Get-Command dotnet -ErrorAction SilentlyContinue).Source
if (-not $dotnet) {
    $dotnet = 'C:\Program Files\dotnet\dotnet.exe'
}
if (-not (Test-Path $dotnet)) {
    Write-Error ".NET SDK(dotnet) 를 찾을 수 없습니다. https://dotnet.microsoft.com 에서 SDK 를 설치하십시오."
}

$config = 'Release'
Write-Host "빌드 시작: $config"
& $dotnet build $projPath -c $config -v minimal
if ($LASTEXITCODE -ne 0) {
    Write-Error "빌드 실패 (exit $LASTEXITCODE)"
}

$exe = Join-Path $PSScriptRoot '..\src\bin\Release\MultiMonitorCapture.exe'
if (Test-Path $exe) {
    Write-Host "빌드 성공: $exe"
    Write-Host "SHA-256 해시 (릴리스 노트에 함께 공개):"
    Get-FileHash $exe -Algorithm SHA256 | Format-List Algorithm, Hash, Path
} else {
    Write-Error "산출물 exe 가 생성되지 않았습니다: $exe"
}
