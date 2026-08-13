namespace MultiMonitorCapture.Properties
{
    // 프로그램 정체성 정보의 단일 관리 지점. 버전을 올릴 때는 이 파일의 Version 만 고친다.
    // 화면 문구는 AppStrings.cs, 빌드 일자/커밋은 BuildInfo.Generated.cs(자동 생성) 를 본다.
    internal static class AppMetadata
    {
        // 코드/파일용 영문 제품명
        public const string ProductName = "MultiMonitorCapture";

        // 개발자 또는 조직 표기
        public const string Developer = "KimJeongWoo";

        // 저작권 표기
        public const string Copyright = "Copyright (c) 2026 " + Developer;

        // 표시 버전 (Major.Minor.Patch). 릴리스할 때 사람이 직접 올린다.
        public const string Version = "1.0.0";

        // 어셈블리 버전 (Major.Minor.Build.Revision), Version 과 일치시킨다
        public const string AssemblyVersion = Version + ".0";

        // exe 파일 속성(Windows 탐색기 속성 창)은 실행 중 언어 전환이 불가능하므로
        // 항상 기본 언어(한국어) 값을 고정으로 사용한다. AppStrings.Ko 를 그대로 재사용한다.
        public const string DisplayName = AppStrings.Ko.DisplayName;
        public const string Description = AppStrings.Ko.Description;
    }
}
