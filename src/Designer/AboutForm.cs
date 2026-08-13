using System;
using System.Windows.Forms;
using MultiMonitorCapture.Domain.Models;
using MultiMonitorCapture.Properties;

namespace MultiMonitorCapture.Designer
{
    // 정보 보기 창. 프로그램 메타 정보와 취급 주의 문구를 표시한다.
    public partial class AboutForm : Form
    {
        public AboutForm(AppInfo info)
        {
            InitializeComponent();

            if (info != null)
            {
                nameValue.Text = info.DisplayName;
                versionValue.Text = info.Version;
                dateValue.Text = info.BuildDate;
                developerValue.Text = info.Developer;
                cautionValue.Text = info.Caution;
                this.Text = info.DisplayName + AppStrings.Cur.AboutTitleSuffix;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
