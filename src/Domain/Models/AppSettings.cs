namespace MultiMonitorCapture.Domain.Models
{
    // 사용자 설정 값. 손상된 값이 들어와도 안전하도록 범위를 강제한다.
    public sealed class AppSettings
    {
        // 허용 범위 상수
        public const int MinFps = 1;
        public const int MaxFps = 15;
        public const int MinClickMarkerMs = 100;
        public const int MaxClickMarkerMs = 2000;

        private int _captureFps = 5;
        private int _clickMarkerMs = 400;

        // 캡처 갱신 주기 (프레임/초). 기본 5.
        public int CaptureFps
        {
            get { return _captureFps; }
            set { _captureFps = Clamp(value, MinFps, MaxFps); }
        }

        // 클릭 표시 원의 유지 시간 (밀리초). 기본 400.
        public int ClickMarkerMs
        {
            get { return _clickMarkerMs; }
            set { _clickMarkerMs = Clamp(value, MinClickMarkerMs, MaxClickMarkerMs); }
        }

        // 컨트롤창이 닫힌 상태에서의 백그라운드 캡처 사용 여부. 기본 사용.
        public bool BackgroundCaptureEnabled { get; set; }

        public AppSettings()
        {
            BackgroundCaptureEnabled = true;
        }

        // 한 틱 간격을 밀리초로 계산한다
        public int GetIntervalMs()
        {
            return 1000 / _captureFps;
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
