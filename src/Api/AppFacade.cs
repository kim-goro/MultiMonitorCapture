using System.Collections.Generic;
using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Models;
using MultiMonitorCapture.Service;

namespace MultiMonitorCapture.Api
{
    // 여러 서비스를 묶어 UI 에 단순 API 로 제공하는 파사드 구현.
    public sealed class AppFacade : IAppFacade
    {
        private readonly MonitorService _monitorService;
        private readonly CaptureService _captureService;
        private readonly SettingsService _settingsService;
        private readonly PrimaryMonitorService _primaryService;

        public AppFacade(
            AppInfo info,
            MonitorService monitorService,
            CaptureService captureService,
            SettingsService settingsService,
            PrimaryMonitorService primaryService)
        {
            Info = info;
            _monitorService = monitorService;
            _captureService = captureService;
            _settingsService = settingsService;
            _primaryService = primaryService;
        }

        public AppInfo Info { get; private set; }

        public AppSettings Settings
        {
            get { return _settingsService.Current; }
        }

        public event FrameCapturedHandler FrameCaptured
        {
            add { _captureService.FrameCaptured += value; }
            remove { _captureService.FrameCaptured -= value; }
        }

        public IList<MonitorInfo> GetAllMonitors()
        {
            return _monitorService.GetAllMonitors();
        }

        public IList<MonitorInfo> GetCaptureTargets()
        {
            return _monitorService.GetCaptureTargets();
        }

        public void StartCapture()
        {
            _captureService.Start(_monitorService.GetCaptureTargets(), Settings.GetIntervalMs());
        }

        public void StopCapture()
        {
            _captureService.Stop();
        }

        public IList<MonitorInfo> RefreshTargets()
        {
            IList<MonitorInfo> targets = _monitorService.GetCaptureTargets();
            _captureService.UpdateTargets(targets);
            _captureService.SetIntervalMs(Settings.GetIntervalMs());
            return targets;
        }

        public void SetPaused(bool paused)
        {
            _captureService.SetPaused(paused);
        }

        public bool SetPrimaryMonitor(MonitorInfo monitor)
        {
            return _primaryService.SetPrimary(monitor);
        }

        public void SaveSettings()
        {
            _settingsService.Save();
        }
    }
}
