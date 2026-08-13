using System.Drawing;

namespace MultiMonitorCapture.Domain.Abstractions
{
    // 화면 영역을 캡처하는 최소 계약. 구현은 GDI 등으로 교체 가능하다 (Strategy).
    public interface IScreenCapturer
    {
        // 지정한 화면 영역을 비트맵으로 캡처한다. 반환된 비트맵은 호출자가 Dispose 한다.
        Bitmap Capture(Rectangle bounds);
    }
}
