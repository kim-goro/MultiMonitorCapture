using System;
using System.Drawing;

namespace MultiMonitorCapture.Presentation.Views
{
    // 캡처 타일 뷰 계약. 프레젠터는 이 계약으로만 타일을 다룬다.
    public interface ICaptureTileView
    {
        // 이 타일이 담당하는 모니터 번호
        int MonitorNumber { get; }

        // 캡처 이미지를 설정한다. 이전 이미지는 내부에서 해제한다.
        void SetImage(Bitmap image);

        // 하단 설정 버튼(...) 클릭 이벤트
        event EventHandler SettingsRequested;
    }
}
