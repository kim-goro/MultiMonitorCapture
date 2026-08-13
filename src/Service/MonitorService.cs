using System.Collections.Generic;
using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Service
{
    // 모니터 목록 관련 업무 로직. 캡처 대상(주 모니터 제외) 산출을 담당한다.
    public sealed class MonitorService
    {
        private readonly IMonitorProvider _provider;

        public MonitorService(IMonitorProvider provider)
        {
            _provider = provider;
        }

        // 연결된 모든 모니터를 반환한다
        public IList<MonitorInfo> GetAllMonitors()
        {
            return _provider.GetMonitors();
        }

        // 주 모니터를 제외한 캡처 대상 모니터만 반환한다
        public IList<MonitorInfo> GetCaptureTargets()
        {
            List<MonitorInfo> targets = new List<MonitorInfo>();
            foreach (MonitorInfo m in _provider.GetMonitors())
            {
                if (!m.IsPrimary)
                {
                    targets.Add(m);
                }
            }
            return targets;
        }
    }
}
