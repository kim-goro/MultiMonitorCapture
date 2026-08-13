namespace MultiMonitorCapture.Domain.Models
{
    // 프로그램 메타 정보를 담는 읽기 전용 모델. 제목표시줄과 정보창에서 사용한다.
    public sealed class AppInfo
    {
        // 표시명 (한글)
        public string DisplayName { get; private set; }

        // 버전 문자열 (예: 1.0.0)
        public string Version { get; private set; }

        // 최종 수정일자 (yyyy-MM-dd)
        public string BuildDate { get; private set; }

        // 개발자 또는 조직 표기
        public string Developer { get; private set; }

        // 취급 주의 안내 문구
        public string Caution { get; private set; }

        public AppInfo(string displayName, string version, string buildDate, string developer, string caution)
        {
            DisplayName = displayName;
            Version = version;
            BuildDate = buildDate;
            Developer = developer;
            Caution = caution;
        }

        // 제목표시줄용 문자열을 만든다 (예: 멀티모니터캡처 v1.0.0 (수정일 2026-08-13))
        public string ToTitleText()
        {
            return string.Format("{0} v{1} (수정일 {2})", DisplayName, Version, BuildDate);
        }
    }
}
