using System.Drawing;

namespace MultiMonitorCapture.Domain.Models
{
    // 한 번의 캡처 결과. Image 의 소유권은 수신자에게 이전되며 수신자가 Dispose 한다.
    public sealed class CaptureFrame
    {
        // 대상 모니터 번호
        public int MonitorNumber { get; private set; }

        // 캡처된 비트맵 (수신자가 사용 후 해제 책임)
        public Bitmap Image { get; private set; }

        public CaptureFrame(int monitorNumber, Bitmap image)
        {
            MonitorNumber = monitorNumber;
            Image = image;
        }
    }
}
