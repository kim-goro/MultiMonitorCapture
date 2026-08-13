namespace MultiMonitorCapture.Properties
{
    // 프로그램 식별 정보의 단일 관리 지점 (Single Source of Truth).
    // 버전을 올리거나 표기를 바꿀 때는 이 파일만 수정한다.
    // AssemblyInfo.cs(exe 속성)와 Bootstrapper.cs(화면 표시)가 모두 이 값을 참조한다.
    //
    // const 를 사용하는 이유
    // 1. 어셈블리 특성은 컴파일 타임 상수만 허용하므로 외부 파일(JSON 등)로 대체할 수 없다
    // 2. 외부 설정 파일을 두면 단일 exe 배포 원칙이 깨지고 변조 위험이 생긴다
    // 3. .NET Framework 4.0 에는 내장 JSON 파서가 없어 추가 의존성이 필요하다
    internal static class AppMetadata
    {
        // 화면에 표시하는 프로그램명 (한글)
        public const string DisplayName = "멀티모니터캡처";

        // 코드/파일용 영문 제품명
        public const string ProductName = "MultiMonitorCapture";

        // 프로그램 설명
        public const string Description = "다중 모니터 실시간 캡처 도구";

        // 개발자 또는 조직 표기
        public const string Developer = "KimJeongWoo";

        // 저작권 표기
        public const string Copyright = "Copyright (c) 2026 " + Developer;

        // 표시 버전 (Major.Minor.Patch)
        public const string Version = "1.0.0";

        // 어셈블리 버전 (Major.Minor.Build.Revision), Version 과 일치시킨다
        public const string AssemblyVersion = Version + ".0";

        // 최종 수정일자 (yyyy-MM-dd). 릴리스마다 갱신한다.
        public const string BuildDate = "2026-08-14";

        // 취급 주의 안내 문구 (정보 보기 창에 표시)
        public const string Caution =
            "본 프로그램은 화면을 실시간으로 캡처합니다. 민감 정보 노출에 주의하십시오.\r\n" +
            "캡처 영상은 로컬 메모리에만 존재하며 저장하거나 외부로 전송하지 않습니다.";
    }
}
