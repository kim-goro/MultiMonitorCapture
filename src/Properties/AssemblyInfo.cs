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
