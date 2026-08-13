using System.Drawing;
using System.Windows.Forms;

namespace MultiMonitorCapture.Designer
{
    // 컨트롤창 디자이너 코드. 컨트롤 정의는 여기서 하고 동적 생성은 팩토리에 맡긴다.
    partial class ControlForm
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel gridPanel;
        private ContextMenuStrip primaryMenu;
        private ToolStripMenuItem setPrimaryMenuItem;
        private Label emptyLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.gridPanel = new TableLayoutPanel();
            this.primaryMenu = new ContextMenuStrip(this.components);
            this.setPrimaryMenuItem = new ToolStripMenuItem();
            this.emptyLabel = new Label();

            this.primaryMenu.SuspendLayout();
            this.SuspendLayout();

            // 격자 패널
            this.gridPanel.Dock = DockStyle.Fill;
            this.gridPanel.BackColor = Color.FromArgb(24, 24, 24);
            this.gridPanel.Padding = new Padding(4);
            this.gridPanel.Name = "gridPanel";
            this.gridPanel.ContextMenuStrip = this.primaryMenu;

            // 우클릭 메뉴 (주 모니터 설정)
            this.setPrimaryMenuItem.Name = "setPrimaryMenuItem";
            this.setPrimaryMenuItem.Text = "주 모니터 설정";
            this.setPrimaryMenuItem.Click += new System.EventHandler(this.setPrimaryMenuItem_Click);
            this.primaryMenu.Items.Add(this.setPrimaryMenuItem);
            this.primaryMenu.Name = "primaryMenu";

            // 보조 모니터가 없을 때 표시하는 안내 라벨
            this.emptyLabel.Dock = DockStyle.Fill;
            this.emptyLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.emptyLabel.ForeColor = Color.Gainsboro;
            this.emptyLabel.BackColor = Color.FromArgb(24, 24, 24);
            this.emptyLabel.Text = "표시할 보조 모니터가 없습니다.";
            this.emptyLabel.Name = "emptyLabel";
            this.emptyLabel.Visible = false;

            // 폼
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(760, 520);
            this.MinimumSize = new Size(420, 320);
            this.Controls.Add(this.gridPanel);
            this.Controls.Add(this.emptyLabel);
            this.ContextMenuStrip = this.primaryMenu;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Name = "ControlForm";
            this.Text = "멀티모니터캡처";

            this.primaryMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
