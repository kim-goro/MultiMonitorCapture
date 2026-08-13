using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MultiMonitorCapture.Api;
using MultiMonitorCapture.Designer;
using MultiMonitorCapture.Domain.Models;
using MultiMonitorCapture.Presentation.Presenters;

namespace MultiMonitorCapture
{
    // 애플리케이션 수명주기를 담당한다. 컨트롤창, 트레이, 프레젠터를 배선하고 종료를 관리한다.
    // 컨트롤창을 닫아도 프로세스가 유지되도록 ApplicationContext 를 사용한다.
    public sealed class AppRunner : ApplicationContext
    {
        private readonly IAppFacade _facade;
        private readonly ControlForm _controlForm;
        private readonly TrayComponent _tray;
        private readonly ControlPresenter _presenter;
        private bool _exiting;

        public AppRunner(IAppFacade facade, ControlForm controlForm, TrayComponent tray, ControlPresenter presenter)
        {
            _facade = facade;
            _controlForm = controlForm;
            _tray = tray;
            _presenter = presenter;

            // 이벤트 배선
            _controlForm.VisibleChanged += delegate { UpdateCaptureState(); };
            _controlForm.PrimaryMonitorSetupRequested += delegate { HandleSetPrimary(); };
            _tray.ShowInfoRequested += delegate { HandleShowInfo(); };
            _tray.ShowControlRequested += delegate { ShowControl(); };
            _tray.SetPrimaryRequested += delegate { HandleSetPrimary(); };
            _tray.BackgroundToggled += HandleBackgroundToggled;
            _tray.ExitRequested += delegate { ExitApp(); };

            // 프레젠터 초기화 후 컨트롤창을 표시한다
            _presenter.Initialize();
            _controlForm.Show();
            UpdateCaptureState();
        }

        // 컨트롤창을 복원하고 앞으로 가져온다
        private void ShowControl()
        {
            if (!_controlForm.Visible)
            {
                _controlForm.Show();
            }
            if (_controlForm.WindowState == FormWindowState.Minimized)
            {
                _controlForm.WindowState = FormWindowState.Normal;
            }
            _controlForm.Activate();
            _controlForm.BringToFront();
            UpdateCaptureState();
        }

        // 캡처 동작 정책을 적용한다.
        // 컨트롤창이 보이면 항상 캡처, 숨김 상태면 백그라운드 캡처 설정을 따른다.
        private void UpdateCaptureState()
        {
            bool shouldCapture = _controlForm.Visible || _facade.Settings.BackgroundCaptureEnabled;
            _facade.SetPaused(!shouldCapture);
            _tray.SetActiveState(shouldCapture);
        }

        private void HandleBackgroundToggled(bool enabled)
        {
            _facade.Settings.BackgroundCaptureEnabled = enabled;
            _facade.SaveSettings();
            UpdateCaptureState();
        }

        private void HandleShowInfo()
        {
            using (AboutForm about = new AboutForm(_facade.Info))
            {
                about.ShowDialog();
            }
        }

        private void HandleSetPrimary()
        {
            IList<MonitorInfo> monitors = _facade.GetAllMonitors();
            IWin32Window owner = _controlForm.Visible ? (IWin32Window)_controlForm : null;
            MonitorInfo chosen = MonitorPickerForm.Pick(monitors, owner);
            if (chosen == null)
            {
                return;
            }

            if (chosen.IsPrimary)
            {
                MessageBox.Show("이미 주 모니터입니다.", "주 모니터 설정",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool ok = _facade.SetPrimaryMonitor(chosen);
            if (ok)
            {
                // 주 모니터가 바뀌면 캡처 대상이 달라지므로 타일을 다시 구성한다
                _presenter.Rebuild();
            }
            else
            {
                MessageBox.Show("주 모니터를 변경하지 못했습니다. 현재 환경에서 지원되지 않을 수 있습니다.",
                    "주 모니터 설정", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExitApp()
        {
            if (_exiting)
            {
                return;
            }
            _exiting = true;

            try
            {
                _presenter.Dispose();
                _facade.StopCapture();
                _facade.SaveSettings();
            }
            catch
            {
                // 종료 과정의 예외는 무시한다
            }

            try { _tray.Dispose(); } catch { }
            try { _controlForm.Dispose(); } catch { }

            ExitThread();
        }
    }
}
