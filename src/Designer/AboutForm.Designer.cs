using System.Drawing;
using System.Windows.Forms;
using MultiMonitorCapture.Properties;

namespace MultiMonitorCapture.Designer
{
    // 정보 보기 창 디자이너 코드
    partial class AboutForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label nameLabel;
        private Label versionLabel;
        private Label dateLabel;
        private Label developerLabel;
        private Label nameValue;
        private Label versionValue;
        private Label dateValue;
        private Label developerValue;
        private Label cautionValue;
        private Button okButton;

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
            this.nameLabel = new Label();
            this.versionLabel = new Label();
            this.dateLabel = new Label();
            this.developerLabel = new Label();
            this.nameValue = new Label();
            this.versionValue = new Label();
            this.dateValue = new Label();
            this.developerValue = new Label();
            this.cautionValue = new Label();
            this.okButton = new Button();
            this.SuspendLayout();

            // 항목 이름 라벨들
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new Point(20, 20);
            this.nameLabel.Text = AppStrings.Cur.AboutLabelProgram;

            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new Point(20, 48);
            this.versionLabel.Text = AppStrings.Cur.AboutLabelVersion;

            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new Point(20, 76);
            this.dateLabel.Text = AppStrings.Cur.AboutLabelBuildDate;

            this.developerLabel.AutoSize = true;
            this.developerLabel.Location = new Point(20, 104);
            this.developerLabel.Text = AppStrings.Cur.AboutLabelDeveloper;

            // 항목 값 라벨들
            this.nameValue.AutoSize = true;
            this.nameValue.Location = new Point(120, 20);
            this.nameValue.Text = "-";

            this.versionValue.AutoSize = true;
            this.versionValue.Location = new Point(120, 48);
            this.versionValue.Text = "-";

            this.dateValue.AutoSize = true;
            this.dateValue.Location = new Point(120, 76);
            this.dateValue.Text = "-";

            this.developerValue.AutoSize = true;
            this.developerValue.Location = new Point(120, 104);
            this.developerValue.Text = "-";

            // 취급 주의 문구 (여러 줄)
            this.cautionValue.Location = new Point(20, 140);
            this.cautionValue.Size = new Size(400, 80);
            this.cautionValue.ForeColor = Color.DarkRed;
            this.cautionValue.Text = "-";

            // 확인 버튼
            this.okButton.Location = new Point(340, 232);
            this.okButton.Size = new Size(80, 26);
            this.okButton.Text = AppStrings.Cur.AboutButtonOk;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);

            // 폼
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(440, 274);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.developerLabel);
            this.Controls.Add(this.nameValue);
            this.Controls.Add(this.versionValue);
            this.Controls.Add(this.dateValue);
            this.Controls.Add(this.developerValue);
            this.Controls.Add(this.cautionValue);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Name = "AboutForm";
            this.Text = AppStrings.Cur.AboutTitleFallback;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
