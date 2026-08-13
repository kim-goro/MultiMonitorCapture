using System.Collections.Generic;
using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Api
{
    // UI 가 의존하는 단일 진입 계약 (Facade). 내부 서비스 구성을 UI 로부터 숨긴다.
    public interface IAppFacade
    {
        // 프로그램 메타 정보
        AppInfo Info { get; }

        // 현재 설정
        AppSettings Settings { get; }

        // 프레임 준비 이벤트 (구독자가 이미지 소유권을 가진다)
        event FrameCapturedHandler FrameCaptured;

        // 연결된 모든 모니터
        IList<MonitorInfo> GetAllMonitors();

        // 주 모니터를 제외한 캡처 대상 모니터
        IList<MonitorInfo> GetCaptureTargets();

        // 현재 대상과 설정으로 캡처를 시작한다
        void StartCapture();

        // 캡처를 정지한다
        void StopCapture();

        // 캡처 대상을 다시 계산해 반영한다. 갱신된 대상 목록을 반환한다.
        IList<MonitorInfo> RefreshTargets();

        // 캡처 일시정지 여부를 설정한다
        void SetPaused(bool paused);

        // OS 주 모니터를 변경한다. 성공 여부를 반환한다.
        bool SetPrimaryMonitor(MonitorInfo monitor);

        // 현재 설정을 저장한다
        void SaveSettings();
    }
}
