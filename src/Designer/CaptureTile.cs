using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MultiMonitorCapture.Presentation.Views;

namespace MultiMonitorCapture.Designer
{
    // 모니터 1개를 표시하는 캡처 타일. 자식 컨트롤 없이 직접 그려서 호버/클릭 처리를 단순화한다.
    // 전역 마우스 훅을 쓰지 않고 컨트롤 자체 이벤트만 사용한다.
    public partial class CaptureTile : UserControl, ICaptureTileView
    {
        private const int BottomBarHeight = 22;
        private const int SettingsButtonWidth = 30;
        private const int ClickRadius = 16;

        private Bitmap _image;
        private int _monitorNumber;
        private string _monitorName = string.Empty;
        private bool _hover;
        private bool _showClick;
        private Point _clickPoint;

        private Timer _clickTimer;
        private Font _numberFont;
        private Font _barFont;
        private SolidBrush _overlayBrush;
        private SolidBrush _barBrush;
        private Pen _hoverPen;
        private Pen _clickPen;

        // 하단 설정 버튼(...) 클릭 이벤트
        public event EventHandler SettingsRequested;

        public CaptureTile()
        {
            InitializeComponent();

            // 깜빡임 없는 직접 렌더링 설정
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

            BackColor = Color.Black;

            _clickTimer = new Timer();
            _clickTimer.Interval = 400;
            _clickTimer.Tick += ClickTimer_Tick;

            _numberFont = new Font(FontFamily.GenericSansSerif, 11f, FontStyle.Bold);
            _barFont = new Font(FontFamily.GenericSansSerif, 8.5f, FontStyle.Regular);
            _overlayBrush = new SolidBrush(Color.FromArgb(140, 0, 0, 0));
            _barBrush = new SolidBrush(Color.FromArgb(210, 30, 30, 30));
            _hoverPen = new Pen(Color.Yellow, 3f);
            _clickPen = new Pen(Color.Yellow, 3f);
        }

        // 담당 모니터 번호
        public int MonitorNumber
        {
            get { return _monitorNumber; }
        }

        // 타일의 표시 정보를 설정한다
        public void Configure(int number, string name, int clickMarkerMs)
        {
            _monitorNumber = number;
            _monitorName = name ?? string.Empty;
            _clickTimer.Interval = clickMarkerMs < 1 ? 1 : clickMarkerMs;
            Invalidate();
        }

        // 캡처 이미지를 교체한다. 이전 이미지는 해제한다.
        public void SetImage(Bitmap image)
        {
            Bitmap old = _image;
            _image = image;
            if (old != null)
            {
                old.Dispose();
            }
            Invalidate(GetImageRect());
        }

        private Rectangle GetImageRect()
        {
            int h = Height - BottomBarHeight;
            if (h < 0) h = 0;
            return new Rectangle(0, 0, Width, h);
        }

        private Rectangle GetBottomBarRect()
        {
            return new Rectangle(0, Height - BottomBarHeight, Width, BottomBarHeight);
        }

        private Rectangle GetSettingsButtonRect()
        {
            return new Rectangle(Width - SettingsButtonWidth, Height - BottomBarHeight, SettingsButtonWidth, BottomBarHeight);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            DrawImageArea(g);
            DrawNumberOverlay(g);
            DrawBottomBar(g);

            if (_showClick)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawEllipse(_clickPen, _clickPoint.X - ClickRadius, _clickPoint.Y - ClickRadius, ClickRadius * 2, ClickRadius * 2);
                g.SmoothingMode = SmoothingMode.Default;
            }

            if (_hover)
            {
                g.DrawRectangle(_hoverPen, new Rectangle(1, 1, Width - 3, Height - 3));
            }
        }

        private void DrawImageArea(Graphics g)
        {
            Rectangle rect = GetImageRect();
            if (_image == null || rect.Width <= 0 || rect.Height <= 0)
            {
                return;
            }

            // 원본 비율을 유지한 채 영역 안에 맞춰 축소(레터박스)한다
            double scale = Math.Min((double)rect.Width / _image.Width, (double)rect.Height / _image.Height);
            int w = (int)(_image.Width * scale);
            int h = (int)(_image.Height * scale);
            int x = rect.X + (rect.Width - w) / 2;
            int y = rect.Y + (rect.Height - h) / 2;

            g.InterpolationMode = InterpolationMode.Low;
            g.DrawImage(_image, new Rectangle(x, y, w, h));
        }

        private void DrawNumberOverlay(Graphics g)
        {
            string text = _monitorNumber.ToString();
            Size sz = TextRenderer.MeasureText(text, _numberFont);
            Rectangle box = new Rectangle(4, 4, sz.Width + 8, sz.Height + 4);
            g.FillRectangle(_overlayBrush, box);
            TextRenderer.DrawText(g, text, _numberFont, box, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawBottomBar(Graphics g)
        {
            Rectangle bar = GetBottomBarRect();
            g.FillRectangle(_barBrush, bar);

            Rectangle nameRect = new Rectangle(bar.X + 6, bar.Y, bar.Width - SettingsButtonWidth - 10, bar.Height);
            TextRenderer.DrawText(g, _monitorName, _barFont, nameRect, Color.Gainsboro,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            Rectangle btn = GetSettingsButtonRect();
            TextRenderer.DrawText(g, "...", _barFont, btn, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // 설정 버튼 영역이면 설정 요청 이벤트만 발생시킨다
            if (GetSettingsButtonRect().Contains(e.Location))
            {
                EventHandler handler = SettingsRequested;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
                return;
            }

            // 캡처 이미지 영역 안에서 클릭하면 노란 원을 일시 표시한다
            if (GetImageRect().Contains(e.Location))
            {
                _clickPoint = e.Location;
                _showClick = true;
                _clickTimer.Stop();
                _clickTimer.Start();
                Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            SetHover(true);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            SetHover(false);
        }

        private void SetHover(bool value)
        {
            if (_hover != value)
            {
                _hover = value;
                Invalidate();
            }
        }

        private void ClickTimer_Tick(object sender, EventArgs e)
        {
            _clickTimer.Stop();
            _showClick = false;
            Invalidate();
        }
    }
}
