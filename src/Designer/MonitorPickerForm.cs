using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MultiMonitorCapture.Domain.Models;
using MultiMonitorCapture.Properties;

namespace MultiMonitorCapture.Designer
{
    // 주 모니터로 설정할 대상을 고르는 선택 창.
    public partial class MonitorPickerForm : Form
    {
        private readonly IList<MonitorInfo> _monitors;

        public MonitorPickerForm(IList<MonitorInfo> monitors)
        {
            InitializeComponent();
            _monitors = monitors;
            PopulateList();
        }

        // 선택된 모니터. 취소 시 null.
        public MonitorInfo SelectedMonitor { get; private set; }

        private void PopulateList()
        {
            monitorList.Items.Clear();
            foreach (MonitorInfo m in _monitors)
            {
                string primaryMark = m.IsPrimary ? AppStrings.Cur.PickerPrimaryMark : string.Empty;
                string text = string.Format(AppStrings.Cur.PickerMonitorFormat,
                    m.Number, m.DeviceName, m.Bounds.Width, m.Bounds.Height, primaryMark);
                monitorList.Items.Add(text);
            }
            if (monitorList.Items.Count > 0)
            {
                monitorList.SelectedIndex = 0;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            int index = monitorList.SelectedIndex;
            if (index >= 0 && index < _monitors.Count)
            {
                SelectedMonitor = _monitors[index];
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // 모니터 선택 대화상자를 띄우고 선택 결과를 반환한다. 취소 시 null.
        public static MonitorInfo Pick(IList<MonitorInfo> monitors, IWin32Window owner)
        {
            using (MonitorPickerForm form = new MonitorPickerForm(monitors))
            {
                DialogResult result = owner != null ? form.ShowDialog(owner) : form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return form.SelectedMonitor;
                }
                return null;
            }
        }
    }
}
