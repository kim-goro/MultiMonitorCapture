using System;
using System.Runtime.InteropServices;

namespace MultiMonitorCapture.Infrastructure.Platform
{
    // P/Invoke 선언을 한곳에 격리한다. 여기 외의 코드에서 직접 Win32 API 를 호출하지 않는다.
    internal static class NativeMethods
    {
        // 현재 설정을 조회할 때 사용하는 모드 번호
        public const int ENUM_CURRENT_SETTINGS = -1;

        // ChangeDisplaySettingsEx 플래그
        public const int CDS_UPDATEREGISTRY = 0x00000001;
        public const int CDS_SET_PRIMARY = 0x00000010;
        public const int CDS_NORESET = 0x10000000;

        // DEVMODE dmFields 플래그 (위치 지정)
        public const int DM_POSITION = 0x00000020;

        // ChangeDisplaySettingsEx 반환값
        public const int DISP_CHANGE_SUCCESSFUL = 0;

        // DISPLAY_DEVICE StateFlags
        public const int DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x00000001;

        // 디스플레이용 DEVMODE 구조체 (dmPosition 을 포함하는 표준 레이아웃)
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        // 디스플레이 장치 정보 구조체
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DISPLAY_DEVICE
        {
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            public int StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        // 연결된 디스플레이 장치를 열거한다
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        // 지정 장치의 현재 표시 설정을 조회한다
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        // 장치의 표시 설정을 변경한다 (DEVMODE 전달)
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

        // 누적된 변경을 실제 적용한다 (DEVMODE 를 전달하지 않는 형태)
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, IntPtr lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

        // GetHicon 으로 만든 아이콘 핸들을 해제한다 (GDI 누수 방지)
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);
    }
}
