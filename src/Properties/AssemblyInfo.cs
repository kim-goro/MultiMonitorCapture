using System.Reflection;
using System.Runtime.InteropServices;
using MultiMonitorCapture.Properties;

// 어셈블리 기본 정보. 값은 AppMetadata 한곳에서만 관리한다.
[assembly: AssemblyTitle(AppMetadata.DisplayName)]
[assembly: AssemblyDescription(AppMetadata.Description)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(AppMetadata.Developer)]
[assembly: AssemblyProduct(AppMetadata.ProductName)]
[assembly: AssemblyCopyright(AppMetadata.Copyright)]
[assembly: AssemblyTrademark("")]

// COM 노출 안 함 (공격 표면 최소화)
[assembly: ComVisible(false)]

// 버전 정보 (Major.Minor.Build.Revision)
[assembly: AssemblyVersion(AppMetadata.AssemblyVersion)]
[assembly: AssemblyFileVersion(AppMetadata.AssemblyVersion)]

// 제품 버전에 빌드 시점 커밋 해시를 함께 표기한다 (exe 속성 창의 "제품 버전"에 표시).
// BuildInfo 는 dotnet build 시 GenerateBuildInfo 타겟이 매번 새로 생성한다.
[assembly: AssemblyInformationalVersion(AppMetadata.Version + "+" + BuildInfo.GitCommit)]
