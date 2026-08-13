using System.Collections.Generic;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Domain.Abstractions
{
    // 현재 연결된 물리 모니터 목록을 제공하는 계약
    public interface IMonitorProvider
    {
        // 연결된 모든 모니터 정보를 안정적 번호 순서로 반환한다
        IList<MonitorInfo> GetMonitors();
    }
}
