using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Infrastructure.Monitors
{
    // System.Windows.Forms.Screen 을 이용해 모니터 목록을 제공한다. WinXP/10/11 공통 동작.
    public sealed class WinFormsMonitorProvider : IMonitorProvider
    {
        public IList<MonitorInfo> GetMonitors()
        {
            Screen[] screens = Screen.AllScreens;

            // 장치 이름 기준으로 정렬하여 실행마다 안정적인 번호를 부여한다
            Array.Sort(screens, delegate(Screen a, Screen b)
            {
                return string.CompareOrdinal(a.DeviceName, b.DeviceName);
            });

            List<MonitorInfo> result = new List<MonitorInfo>();
            int number = 1;
            foreach (Screen s in screens)
            {
                result.Add(new MonitorInfo(number, s.DeviceName, s.Bounds, s.Primary));
                number++;
            }
            return result;
        }
    }
}
