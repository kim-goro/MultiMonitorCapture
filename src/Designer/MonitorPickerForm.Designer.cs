using System.Drawing;
using System.Windows.Forms;

namespace MultiMonitorCapture.Designer
{
    // 주 모니터 선택 창 디자이너 코드
    partial class MonitorPickerForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label titleLabel;
        private ListBox monitorList;
        private Button okButton;
        private Button cancelButton;

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
            this.titleLabel = new Label();
            this.monitorList = new ListBox();
            this.okButton = new Button();
            this.cancelButton = new Button();
            this.SuspendLayout();

            // 안내 라벨
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new Point(14, 12);
            this.titleLabel.Text = "주 모니터로 설정할 모니터를 선택하십시오.";

            // 모니터 목록
            this.monitorList.Location = new Point(14, 38);
            this.monitorList.Size = new Size(440, 160);
            this.monitorList.IntegralHeight = false;

            // 확인 버튼
            this.okButton.Location = new Point(284, 210);
            this.okButton.Size = new Size(80, 28);
            this.okButton.Text = "설정";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);

            // 취소 버튼
            this.cancelButton.Location = new Point(374, 210);
            this.cancelButton.Size = new Size(80, 28);
            this.cancelButton.Text = "취소";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);

            // 폼
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(468, 250);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.monitorList);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;
            this.Name = "MonitorPickerForm";
            this.Text = "주 모니터 설정";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
