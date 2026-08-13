using System.Drawing;

namespace MultiMonitorCapture.Domain.Models
{
    // 물리 모니터 1개의 정보를 나타내는 순수 모델
    public sealed class MonitorInfo
    {
        // 화면에 표시할 안정적 모니터 번호 (1부터 시작)
        public int Number { get; private set; }

        // OS 장치 이름 (예: \\.\DISPLAY1)
        public string DeviceName { get; private set; }

        // 모니터의 가상 데스크톱 좌표 영역
        public Rectangle Bounds { get; private set; }

        // OS 주 모니터 여부
        public bool IsPrimary { get; private set; }

        public MonitorInfo(int number, string deviceName, Rectangle bounds, bool isPrimary)
        {
            Number = number;
            DeviceName = deviceName;
            Bounds = bounds;
            IsPrimary = isPrimary;
        }
    }
}
