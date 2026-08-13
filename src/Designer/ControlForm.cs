using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MultiMonitorCapture.Presentation.Views;

namespace MultiMonitorCapture.Designer
{
    // 컨트롤창. 캡처 타일을 격자로 담고, 닫기 시 종료 대신 백그라운드로 숨는다.
    public partial class ControlForm : Form, IControlView
    {
        // 창 바탕 우클릭 메뉴의 주 모니터 설정 요청 이벤트
        public event EventHandler PrimaryMonitorSetupRequested;

        public ControlForm()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            this.Text = title;
        }

        public void RenderTiles(IList<CaptureTile> tiles)
        {
            gridPanel.SuspendLayout();

            // 기존 타일을 정리(Dispose)해 자원 누수를 막는다
            for (int i = gridPanel.Controls.Count - 1; i >= 0; i--)
            {
                Control c = gridPanel.Controls[i];
                gridPanel.Controls.RemoveAt(i);
                c.Dispose();
            }
            gridPanel.ColumnStyles.Clear();
            gridPanel.RowStyles.Clear();

            int n = tiles.Count;
            if (n == 0)
            {
                gridPanel.ColumnCount = 1;
                gridPanel.RowCount = 1;
                gridPanel.ResumeLayout();
                emptyLabel.Visible = true;
                emptyLabel.BringToFront();
                return;
            }

            emptyLabel.Visible = false;

            // 타일 개수에 맞춘 정사각형에 가까운 격자 구성
            int cols = (int)Math.Ceiling(Math.Sqrt(n));
            int rows = (int)Math.Ceiling((double)n / cols);
            gridPanel.ColumnCount = cols;
            gridPanel.RowCount = rows;
            for (int c = 0; c < cols; c++)
            {
                gridPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));
            }
            for (int r = 0; r < rows; r++)
            {
                gridPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
            }

            for (int i = 0; i < n; i++)
            {
                CaptureTile tile = tiles[i];
                tile.Dock = DockStyle.Fill;
                tile.Margin = new Padding(3);
                // 타일 위에서 우클릭해도 주 모니터 설정 메뉴가 뜨도록 공유한다
                tile.ContextMenuStrip = primaryMenu;
                gridPanel.Controls.Add(tile, i % cols, i / cols);
            }

            gridPanel.ResumeLayout();
        }

        public bool TryRunOnUi(Action action)
        {
            try
            {
                if (!IsHandleCreated)
                {
                    return false;
                }
                if (InvokeRequired)
                {
                    BeginInvoke(action);
                }
                else
                {
                    action();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void setPrimaryMenuItem_Click(object sender, EventArgs e)
        {
            EventHandler handler = PrimaryMonitorSetupRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 사용자가 X 로 닫으면 종료하지 않고 숨긴다 (트레이 상주)
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            base.OnFormClosing(e);
        }
    }
}
