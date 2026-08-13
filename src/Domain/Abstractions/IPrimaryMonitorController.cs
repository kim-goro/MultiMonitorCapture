using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Domain.Abstractions
{
    // OS 주 모니터 변경 계약. 사용자가 명시적으로 실행할 때만 호출한다.
    public interface IPrimaryMonitorController
    {
        // 지정 모니터를 OS 주 모니터로 설정한다. 성공하면 true 를 반환한다.
        bool SetPrimary(MonitorInfo monitor);
    }
}
