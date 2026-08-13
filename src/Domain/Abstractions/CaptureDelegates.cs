using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Domain.Abstractions
{
    // 캡처 프레임 전달용 델리게이트. EventArgs 제약을 피하려고 별도 델리게이트로 정의한다.
    public delegate void FrameCapturedHandler(CaptureFrame frame);
}
