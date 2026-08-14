# 빌드 정보 자동 생성 스크립트.
# CI/CD 서버 없이도 로컬 dotnet build 시 MSBuild 타겟이 이 스크립트를 호출해
# 빌드 날짜와 커밋 해시를 담은 소스 파일을 매번 새로 만든다.
# 결과 파일(BuildInfo.Generated.cs)은 손으로 고치지 않으며 git 추적 대상도 아니다.

param(
    [Parameter(Mandatory = $true)]
    [string]$OutputPath
)

# 실제 오류(파일 쓰기 실패 등)는 조용히 넘기지 않고 빌드를 실패시킨다.
# git 이 없거나 실패하는 경우만 아래에서 개별적으로 허용한다.
$ErrorActionPreference = 'Stop'

$commit = $null
try {
    $commit = (git rev-parse --short HEAD 2>$null)
} catch {
    $commit = $null
}
if ([string]::IsNullOrWhiteSpace($commit)) {
    $commit = 'unknown'
}

$date = Get-Date -Format 'yyyy-MM-dd'

# 주의: 이 here-string 안에는 한글을 넣지 않는다.
# Windows PowerShell 5.1 이 .ps1 파일을 시스템 코드페이지로 파싱할 때
# BOM 없는 UTF-8 소스의 한글 리터럴이 깨지는 문제가 있다 (출력 인코딩과 무관).
$content = @"
// Auto-generated file. Do not edit by hand. Regenerated on every dotnet build.
namespace MultiMonitorCapture.Properties
{
    internal static class BuildInfo
    {
        // Last build date (yyyy-MM-dd)
        public const string BuildDate = "$date";

        // Short git commit hash at build time. "unknown" if git info is unavailable.
        public const string GitCommit = "$commit";
    }
}
"@

$dir = Split-Path -Path $OutputPath -Parent
if (-not (Test-Path $dir)) {
    New-Item -ItemType Directory -Path $dir -Force | Out-Null
}

# 내용이 실제로 바뀐 경우에만 다시 쓴다.
# (매번 다시 쓰면 mtime 이 항상 갱신되어, 아무것도 안 바뀐 재빌드에서도
#  CoreCompile 이 매번 다시 돈다)
# 줄끝 문자 비교는 하지 않는다: Set-Content 가 파일 끝에 추가하는
# 개행과 here-string 의 개행이 달라 항상 "다름"으로 판정되는 문제가 있었다.
$existing = $null
if (Test-Path $OutputPath) {
    $existing = Get-Content -Path $OutputPath -Raw
}
if ($existing -eq $null -or $existing.Trim() -ne $content.Trim()) {
    Set-Content -Path $OutputPath -Value $content -Encoding UTF8
}
