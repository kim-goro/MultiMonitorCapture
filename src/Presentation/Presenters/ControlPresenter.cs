using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using MultiMonitorCapture.Api;
using MultiMonitorCapture.Designer;
using MultiMonitorCapture.Domain.Models;
using MultiMonitorCapture.Presentation.Views;

namespace MultiMonitorCapture.Presentation.Presenters
{
    // 컨트롤창과 캡처 서비스를 잇는 프레젠터 (MVP). 타일 구성과 프레임 반영을 담당한다.
    public sealed class ControlPresenter
    {
        private readonly IAppFacade _facade;
        private readonly IControlView _view;
        private readonly CaptureTileFactory _factory;

        private readonly Dictionary<int, ICaptureTileView> _tiles = new Dictionary<int, ICaptureTileView>();
        private readonly Dictionary<int, MonitorInfo> _monitorByNumber = new Dictionary<int, MonitorInfo>();

        public ControlPresenter(IAppFacade facade, IControlView view, CaptureTileFactory factory)
        {
            _facade = facade;
            _view = view;
            _factory = factory;
        }

        // 초기 구성 후 캡처를 시작한다
        public void Initialize()
        {
            _view.SetTitle(_facade.Info.ToTitleText());
            BuildTiles();
            _facade.FrameCaptured += OnFrameCaptured;
            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
            _facade.StartCapture();
        }

        // 모니터 구성이 바뀌었을 때 타일을 다시 구성한다
        public void Rebuild()
        {
            BuildTiles();
            _facade.RefreshTargets();
        }

        public void Dispose()
        {
            _facade.FrameCaptured -= OnFrameCaptured;
            SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;
        }

        private void BuildTiles()
        {
            _tiles.Clear();
            _monitorByNumber.Clear();

            IList<MonitorInfo> targets = _facade.GetCaptureTargets();
            IList<CaptureTile> tiles = _factory.CreateTiles(targets, _facade.Settings.ClickMarkerMs, OnTileSettingsRequested);

            foreach (CaptureTile tile in tiles)
            {
                _tiles[tile.MonitorNumber] = tile;
            }
            foreach (MonitorInfo m in targets)
            {
                _monitorByNumber[m.Number] = m;
            }

            _view.RenderTiles(tiles);
        }

        private void OnFrameCaptured(CaptureFrame frame)
        {
            // 캡처 스레드에서 호출된다. UI 스레드로 넘겨 타일에 반영한다.
            Bitmap image = frame.Image;
            int number = frame.MonitorNumber;

            Action apply = delegate
            {
                ICaptureTileView tile;
                if (_tiles.TryGetValue(number, out tile))
                {
                    tile.SetImage(image);
                }
                else
                {
                    image.Dispose();
                }
            };

            if (!_view.TryRunOnUi(apply))
            {
                // UI 로 넘기지 못하면 이미지를 해제해 누수를 막는다
                image.Dispose();
            }
        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            _view.TryRunOnUi(delegate { Rebuild(); });
        }

        private void OnTileSettingsRequested(object sender, EventArgs e)
        {
            CaptureTile tile = sender as CaptureTile;
            if (tile == null)
            {
                return;
            }

            MonitorInfo m;
            if (_monitorByNumber.TryGetValue(tile.MonitorNumber, out m))
            {
                string msg = string.Format(
                    "모니터 {0}\n이름: {1}\n해상도: {2} x {3}\n위치: ({4}, {5})",
                    m.Number, m.DeviceName, m.Bounds.Width, m.Bounds.Height, m.Bounds.X, m.Bounds.Y);
                MessageBox.Show(msg, "모니터 정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
