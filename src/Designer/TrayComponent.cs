using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MultiMonitorCapture.Infrastructure.Platform;

namespace MultiMonitorCapture.Designer
{
    // 시스템 트레이 아이콘과 우클릭 메뉴를 캡슐화한다. 아이콘 색으로 캡처 동작 상태를 표시한다.
    public sealed class TrayComponent : IDisposable
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenuStrip _menu;
        private readonly ToolStripMenuItem _infoItem;
        private readonly ToolStripMenuItem _showItem;
        private readonly ToolStripMenuItem _primaryItem;
        private readonly ToolStripMenuItem _backgroundItem;
        private readonly ToolStripMenuItem _exitItem;

        private readonly Icon _activeIcon;
        private readonly Icon _inactiveIcon;

        // 트레이 메뉴 이벤트
        public event EventHandler ShowInfoRequested;
        public event EventHandler ShowControlRequested;
        public event EventHandler SetPrimaryRequested;
        public event EventHandler ExitRequested;
        public event Action<bool> BackgroundToggled;

        public TrayComponent(string tooltip, bool backgroundEnabled)
        {
            _activeIcon = CreateStateIcon(Color.FromArgb(0, 120, 215));
            _inactiveIcon = CreateStateIcon(Color.FromArgb(130, 130, 130));

            _menu = new ContextMenuStrip();
            _infoItem = new ToolStripMenuItem("정보 보기");
            _showItem = new ToolStripMenuItem("컨트롤창 띄우기");
            _primaryItem = new ToolStripMenuItem("메인 모니터 설정하기");
            _backgroundItem = new ToolStripMenuItem("백그라운드 캡처");
            _backgroundItem.CheckOnClick = true;
            _backgroundItem.Checked = backgroundEnabled;
            _exitItem = new ToolStripMenuItem("프로그램 종료");

            _menu.Items.AddRange(new ToolStripItem[]
            {
                _infoItem,
                _showItem,
                _primaryItem,
                _backgroundItem,
                new ToolStripSeparator(),
                _exitItem
            });

            _infoItem.Click += delegate { Raise(ShowInfoRequested); };
            _showItem.Click += delegate { Raise(ShowControlRequested); };
            _primaryItem.Click += delegate { Raise(SetPrimaryRequested); };
            _exitItem.Click += delegate { Raise(ExitRequested); };
            _backgroundItem.CheckedChanged += delegate
            {
                Action<bool> handler = BackgroundToggled;
                if (handler != null) handler(_backgroundItem.Checked);
            };

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = Truncate(tooltip, 63);
            _notifyIcon.Icon = _activeIcon;
            _notifyIcon.ContextMenuStrip = _menu;
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += delegate { Raise(ShowControlRequested); };
        }

        // 캡처 동작 상태에 따라 아이콘 색을 바꾼다 (동작 중 컬러, 정지 회색)
        public void SetActiveState(bool active)
        {
            _notifyIcon.Icon = active ? _activeIcon : _inactiveIcon;
        }

        // 백그라운드 캡처 체크 상태를 외부에서 반영한다
        public void SetBackgroundChecked(bool value)
        {
            _backgroundItem.Checked = value;
        }

        public void Dispose()
        {
            // 트레이 아이콘을 먼저 숨겨 잔상이 남지 않게 한다
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _menu.Dispose();
            if (_activeIcon != null) _activeIcon.Dispose();
            if (_inactiveIcon != null) _inactiveIcon.Dispose();
        }

        private void Raise(EventHandler handler)
        {
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        // 단색 원 위에 모니터 모양을 그린 아이콘을 만든다. GDI 핸들은 즉시 해제한다.
        private static Icon CreateStateIcon(Color background)
        {
            using (Bitmap bmp = new Bitmap(32, 32))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.Transparent);
                    using (SolidBrush b = new SolidBrush(background))
                    {
                        g.FillEllipse(b, 1, 1, 30, 30);
                    }
                    using (SolidBrush w = new SolidBrush(Color.White))
                    {
                        g.FillRectangle(w, 9, 10, 14, 9);
                        g.FillRectangle(w, 14, 19, 4, 3);
                        g.FillRectangle(w, 11, 22, 10, 2);
                    }
                }

                IntPtr handle = bmp.GetHicon();
                try
                {
                    using (Icon temp = Icon.FromHandle(handle))
                    {
                        return (Icon)temp.Clone();
                    }
                }
                finally
                {
                    // GetHicon 이 만든 핸들을 해제해 GDI 누수를 방지한다
                    NativeMethods.DestroyIcon(handle);
                }
            }
        }

        private static string Truncate(string value, int max)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= max ? value : value.Substring(0, max);
        }
    }
}
