using System;
using System.Collections.Generic;
using MultiMonitorCapture.Designer;

namespace MultiMonitorCapture.Presentation.Views
{
    // 컨트롤창 뷰 계약. 프레젠터는 이 계약을 통해 화면을 갱신한다.
    public interface IControlView
    {
        // 제목표시줄 문자열을 설정한다
        void SetTitle(string title);

        // 타일 목록을 격자로 배치한다 (기존 타일은 정리한다)
        void RenderTiles(IList<CaptureTile> tiles);

        // UI 스레드에서 작업을 실행한다. 실행 예약에 성공하면 true 를 반환한다.
        bool TryRunOnUi(Action action);

        // 창 바탕 우클릭 메뉴의 주 모니터 설정 요청 이벤트
        event EventHandler PrimaryMonitorSetupRequested;
    }
}
