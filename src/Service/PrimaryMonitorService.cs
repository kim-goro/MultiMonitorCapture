using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Service
{
    // 주 모니터 변경 업무 로직. 실제 변경은 인프라 구현에 위임한다.
    public sealed class PrimaryMonitorService
    {
        private readonly IPrimaryMonitorController _controller;

        public PrimaryMonitorService(IPrimaryMonitorController controller)
        {
            _controller = controller;
        }

        // 지정 모니터를 OS 주 모니터로 설정한다. 성공 여부를 반환한다.
        public bool SetPrimary(MonitorInfo monitor)
        {
            return _controller.SetPrimary(monitor);
        }
    }
}
