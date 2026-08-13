using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Enums;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Service
{
    // 캡처 루프를 관리하는 서비스. 백그라운드 스레드에서 캡처하고 프레임을 이벤트로 발행한다 (Observer).
    public sealed class CaptureService
    {
        private readonly IScreenCapturer _capturer;
        private readonly object _sync = new object();

        private Thread _thread;
        private volatile bool _running;
        private volatile bool _paused;
        private int _intervalMs = 200;
        private List<MonitorInfo> _targets = new List<MonitorInfo>();

        // 프레임이 준비되면 발행된다. 구독자가 프레임 이미지의 소유권을 가진다.
        public event FrameCapturedHandler FrameCaptured;

        public CaptureService(IScreenCapturer capturer)
        {
            _capturer = capturer;
        }

        // 현재 상태를 반환한다
        public CaptureState State
        {
            get
            {
                if (!_running) return CaptureState.Stopped;
                return _paused ? CaptureState.Paused : CaptureState.Running;
            }
        }

        // 캡처 대상과 간격을 지정하고 루프를 시작한다
        public void Start(IEnumerable<MonitorInfo> targets, int intervalMs)
        {
            lock (_sync)
            {
                _targets = new List<MonitorInfo>(targets);
                _intervalMs = intervalMs < 1 ? 1 : intervalMs;
                if (_running)
                {
                    return;
                }
                _running = true;
                _paused = false;
            }

            _thread = new Thread(Loop);
            _thread.IsBackground = true;
            _thread.Name = "CaptureLoop";
            _thread.Start();
        }

        // 루프를 정지하고 스레드를 정리한다
        public void Stop()
        {
            _running = false;
            Thread t = _thread;
            if (t != null && t.IsAlive)
            {
                t.Join(1000);
            }
            _thread = null;
        }

        // 캡처 대상을 교체한다 (모니터 구성 변경 시)
        public void UpdateTargets(IEnumerable<MonitorInfo> targets)
        {
            lock (_sync)
            {
                _targets = new List<MonitorInfo>(targets);
            }
        }

        // 캡처 간격(밀리초)을 변경한다
        public void SetIntervalMs(int intervalMs)
        {
            lock (_sync)
            {
                _intervalMs = intervalMs < 1 ? 1 : intervalMs;
            }
        }

        // 일시정지 여부를 설정한다
        public void SetPaused(bool paused)
        {
            _paused = paused;
        }

        private void Loop()
        {
            while (_running)
            {
                int interval;
                List<MonitorInfo> snapshot;
                lock (_sync)
                {
                    interval = _intervalMs;
                    snapshot = new List<MonitorInfo>(_targets);
                }

                if (!_paused)
                {
                    CaptureOnce(snapshot);
                }

                Thread.Sleep(interval);
            }
        }

        private void CaptureOnce(List<MonitorInfo> snapshot)
        {
            foreach (MonitorInfo m in snapshot)
            {
                if (!_running)
                {
                    break;
                }

                Bitmap bmp = null;
                try
                {
                    bmp = _capturer.Capture(m.Bounds);
                    FrameCapturedHandler handler = FrameCaptured;
                    if (handler != null)
                    {
                        // 소유권을 구독자에게 넘긴다
                        handler(new CaptureFrame(m.Number, bmp));
                        bmp = null;
                    }
                }
                catch
                {
                    // 개별 모니터 캡처 실패는 격리하고 다음 모니터로 넘어간다
                }
                finally
                {
                    // 구독자가 없거나 예외가 난 경우 비트맵을 해제해 누수를 막는다
                    if (bmp != null)
                    {
                        bmp.Dispose();
                    }
                }
            }
        }
    }
}
