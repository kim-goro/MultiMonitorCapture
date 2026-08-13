using System;
using System.Runtime.InteropServices;
using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Infrastructure.Platform
{
    // OS 주 모니터를 실제로 변경한다. Windows 디스플레이 설정과 동일한 결과를 만든다.
    // 관리자 권한 없이 사용자 세션에서 동작하며, 실패하면 원상 유지된다.
    public sealed class PrimaryMonitorController : IPrimaryMonitorController
    {
        public bool SetPrimary(MonitorInfo monitor)
        {
            if (monitor == null || string.IsNullOrEmpty(monitor.DeviceName))
            {
                return false;
            }

            try
            {
                string targetDevice = monitor.DeviceName;

                // 대상 모니터의 현재 위치를 구한다
                NativeMethods.DEVMODE targetMode = CreateDevMode();
                if (!NativeMethods.EnumDisplaySettings(targetDevice, NativeMethods.ENUM_CURRENT_SETTINGS, ref targetMode))
                {
                    return false;
                }

                // 대상이 원점(0,0)으로 오도록 모든 모니터를 평행 이동시킬 값
                int offsetX = -targetMode.dmPositionX;
                int offsetY = -targetMode.dmPositionY;

                // 이미 주 모니터이면 변경 불필요
                if (offsetX == 0 && offsetY == 0)
                {
                    return true;
                }

                // 데스크톱에 연결된 모든 장치의 위치를 재계산하여 레지스트리에 예약한다
                NativeMethods.DISPLAY_DEVICE device = new NativeMethods.DISPLAY_DEVICE();
                device.cb = Marshal.SizeOf(typeof(NativeMethods.DISPLAY_DEVICE));
                uint index = 0;

                while (NativeMethods.EnumDisplayDevices(null, index, ref device, 0))
                {
                    index++;

                    bool attached = (device.StateFlags & NativeMethods.DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) != 0;
                    if (attached)
                    {
                        string deviceName = device.DeviceName;

                        NativeMethods.DEVMODE mode = CreateDevMode();
                        if (NativeMethods.EnumDisplaySettings(deviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref mode))
                        {
                            mode.dmPositionX += offsetX;
                            mode.dmPositionY += offsetY;
                            mode.dmFields |= NativeMethods.DM_POSITION;

                            uint flags = (uint)(NativeMethods.CDS_UPDATEREGISTRY | NativeMethods.CDS_NORESET);
                            bool isTarget = string.Equals(deviceName, targetDevice, StringComparison.OrdinalIgnoreCase);
                            if (isTarget)
                            {
                                flags |= (uint)NativeMethods.CDS_SET_PRIMARY;
                            }

                            NativeMethods.ChangeDisplaySettingsEx(deviceName, ref mode, IntPtr.Zero, flags, IntPtr.Zero);
                        }
                    }

                    // 다음 열거를 위해 구조체를 초기화한다
                    device = new NativeMethods.DISPLAY_DEVICE();
                    device.cb = Marshal.SizeOf(typeof(NativeMethods.DISPLAY_DEVICE));
                }

                // 예약된 변경을 실제로 적용한다
                int result = NativeMethods.ChangeDisplaySettingsEx(null, IntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero);
                return result == NativeMethods.DISP_CHANGE_SUCCESSFUL;
            }
            catch
            {
                // 미지원 환경이나 예외 시 변경 실패로 처리한다 (기존 배치 유지)
                return false;
            }
        }

        private static NativeMethods.DEVMODE CreateDevMode()
        {
            NativeMethods.DEVMODE mode = new NativeMethods.DEVMODE();
            mode.dmDeviceName = new string('\0', 32);
            mode.dmFormName = new string('\0', 32);
            mode.dmSize = (short)Marshal.SizeOf(typeof(NativeMethods.DEVMODE));
            return mode;
        }
    }
}
