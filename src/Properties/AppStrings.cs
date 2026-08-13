using System.Globalization;

namespace MultiMonitorCapture.Properties
{
    // 화면에 표시되는 모든 문구의 단일 관리 지점 (Single Source of Truth).
    // 새 문구를 추가하거나 문구를 바꿀 때는 이 파일만 수정한다.
    //
    // 위성 어셈블리(.resx + 언어별 dll)를 쓰지 않는 이유
    // 언어별 dll이 생기면 "단일 exe 배포" 원칙이 깨진다.
    // 대신 모든 언어를 문자열 상수로 exe 안에 함께 담고 실행 시점에 고른다.
    //
    // Ko/En 은 const 이므로 AssemblyInfo.cs 의 어셈블리 특성에도 사용할 수 있다
    // (특성 인자는 컴파일 타임 상수만 허용되기 때문). 단, exe 파일 속성 자체는
    // 실행 후 바꿀 수 없으므로 AppMetadata 는 항상 Ko 값을 고정으로 사용한다.
    internal static class AppStrings
    {
        internal static class Ko
        {
            public const string DisplayName = "멀티모니터캡처";
            public const string Description = "다중 모니터 실시간 캡처 도구";

            public const string TitleFormat = "{0} v{1} (수정일 {2})";

            public const string Caution =
                "본 프로그램은 화면을 실시간으로 캡처합니다. 민감 정보 노출에 주의하십시오.\r\n" +
                "캡처 영상은 로컬 메모리에만 존재하며 저장하거나 외부로 전송하지 않습니다.";

            // 트레이 메뉴
            public const string MenuShowInfo = "정보 보기";
            public const string MenuShowControl = "컨트롤창 띄우기";
            public const string MenuSetPrimary = "메인 모니터 설정하기";
            public const string MenuBackgroundCapture = "백그라운드 캡처";
            public const string MenuExit = "프로그램 종료";

            // 정보 보기(About) 창
            public const string AboutLabelProgram = "프로그램:";
            public const string AboutLabelVersion = "버전:";
            public const string AboutLabelBuildDate = "최종 수정일:";
            public const string AboutLabelDeveloper = "개발자:";
            public const string AboutButtonOk = "확인";
            public const string AboutTitleFallback = "정보";
            public const string AboutTitleSuffix = " 정보";

            // 주 모니터 선택 창
            public const string PickerPrompt = "주 모니터로 설정할 모니터를 선택하십시오.";
            public const string PickerButtonSet = "설정";
            public const string PickerButtonCancel = "취소";
            public const string PickerWindowTitle = "주 모니터 설정";
            public const string PickerPrimaryMark = " (현재 주 모니터)";
            public const string PickerMonitorFormat = "모니터 {0}: {1} [{2}x{3}]{4}";

            // 컨트롤창
            public const string ContextMenuSetPrimary = "주 모니터 설정";
            public const string EmptyMonitorMessage = "표시할 보조 모니터가 없습니다.";

            // 모니터 정보 팝업
            public const string MonitorInfoFormat = "모니터 {0}\n이름: {1}\n해상도: {2} x {3}\n위치: ({4}, {5})";
            public const string MonitorInfoTitle = "모니터 정보";

            // 주 모니터 변경 결과 안내
            public const string AlreadyPrimaryMessage = "이미 주 모니터입니다.";
            public const string SetPrimaryFailedMessage = "주 모니터를 변경하지 못했습니다. 현재 환경에서 지원되지 않을 수 있습니다.";

            // 프로그램 시작/오류 안내
            public const string AlreadyRunningMessage = "멀티모니터캡처가 이미 실행 중입니다.";
            public const string UnknownErrorMessage = "알 수 없는 오류가 발생했습니다.";
            public const string ErrorPrefix = "오류가 발생했습니다: ";
        }

        internal static class En
        {
            public const string DisplayName = "MultiMonitorCapture";
            public const string Description = "Multi-monitor real-time capture tool";

            public const string TitleFormat = "{0} v{1} (Updated {2})";

            public const string Caution =
                "This program captures your screen in real time. Be careful not to expose sensitive information.\r\n" +
                "Captured video exists only in local memory and is never saved or transmitted externally.";

            public const string MenuShowInfo = "Show Info";
            public const string MenuShowControl = "Show Control Window";
            public const string MenuSetPrimary = "Set Primary Monitor";
            public const string MenuBackgroundCapture = "Background Capture";
            public const string MenuExit = "Exit";

            public const string AboutLabelProgram = "Program:";
            public const string AboutLabelVersion = "Version:";
            public const string AboutLabelBuildDate = "Last Updated:";
            public const string AboutLabelDeveloper = "Developer:";
            public const string AboutButtonOk = "OK";
            public const string AboutTitleFallback = "About";
            public const string AboutTitleSuffix = " Info";

            public const string PickerPrompt = "Select the monitor to set as primary.";
            public const string PickerButtonSet = "Set";
            public const string PickerButtonCancel = "Cancel";
            public const string PickerWindowTitle = "Set Primary Monitor";
            public const string PickerPrimaryMark = " (current primary)";
            public const string PickerMonitorFormat = "Monitor {0}: {1} [{2}x{3}]{4}";

            public const string ContextMenuSetPrimary = "Set Primary Monitor";
            public const string EmptyMonitorMessage = "No secondary monitors to display.";

            public const string MonitorInfoFormat = "Monitor {0}\nName: {1}\nResolution: {2} x {3}\nPosition: ({4}, {5})";
            public const string MonitorInfoTitle = "Monitor Info";

            public const string AlreadyPrimaryMessage = "This is already the primary monitor.";
            public const string SetPrimaryFailedMessage = "Failed to change the primary monitor. This may not be supported in the current environment.";

            public const string AlreadyRunningMessage = "MultiMonitorCapture is already running.";
            public const string UnknownErrorMessage = "An unknown error occurred.";
            public const string ErrorPrefix = "An error occurred: ";
        }

        // 화면 코드가 실제로 사용하는 접근점. 예: AppStrings.Cur.MenuShowInfo
        public static readonly Set Cur = BuildCurrent();

        // 런타임에 값이 채워지는 일반 클래스 (const 아님. 언어 전환 시 재계산 대상)
        public sealed class Set
        {
            public string DisplayName;
            public string Description;
            public string TitleFormat;
            public string Caution;
            public string MenuShowInfo;
            public string MenuShowControl;
            public string MenuSetPrimary;
            public string MenuBackgroundCapture;
            public string MenuExit;
            public string AboutLabelProgram;
            public string AboutLabelVersion;
            public string AboutLabelBuildDate;
            public string AboutLabelDeveloper;
            public string AboutButtonOk;
            public string AboutTitleFallback;
            public string AboutTitleSuffix;
            public string PickerPrompt;
            public string PickerButtonSet;
            public string PickerButtonCancel;
            public string PickerWindowTitle;
            public string PickerPrimaryMark;
            public string PickerMonitorFormat;
            public string ContextMenuSetPrimary;
            public string EmptyMonitorMessage;
            public string MonitorInfoFormat;
            public string MonitorInfoTitle;
            public string AlreadyPrimaryMessage;
            public string SetPrimaryFailedMessage;
            public string AlreadyRunningMessage;
            public string UnknownErrorMessage;
            public string ErrorPrefix;
        }

        // 현재는 OS 로캘 기준으로 한 번 결정한다 (프로그램 시작 시 고정).
        // 추후 설정 화면에서 언어를 바꾸는 기능을 추가하려면
        // 이 메서드를 다시 호출해 Cur 의 필드 값을 갱신하면 된다.
        private static Set BuildCurrent()
        {
            bool useEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en";
            return useEnglish ? FromEnglish() : FromKorean();
        }

        private static Set FromKorean()
        {
            return new Set
            {
                DisplayName = Ko.DisplayName,
                Description = Ko.Description,
                TitleFormat = Ko.TitleFormat,
                Caution = Ko.Caution,
                MenuShowInfo = Ko.MenuShowInfo,
                MenuShowControl = Ko.MenuShowControl,
                MenuSetPrimary = Ko.MenuSetPrimary,
                MenuBackgroundCapture = Ko.MenuBackgroundCapture,
                MenuExit = Ko.MenuExit,
                AboutLabelProgram = Ko.AboutLabelProgram,
                AboutLabelVersion = Ko.AboutLabelVersion,
                AboutLabelBuildDate = Ko.AboutLabelBuildDate,
                AboutLabelDeveloper = Ko.AboutLabelDeveloper,
                AboutButtonOk = Ko.AboutButtonOk,
                AboutTitleFallback = Ko.AboutTitleFallback,
                AboutTitleSuffix = Ko.AboutTitleSuffix,
                PickerPrompt = Ko.PickerPrompt,
                PickerButtonSet = Ko.PickerButtonSet,
                PickerButtonCancel = Ko.PickerButtonCancel,
                PickerWindowTitle = Ko.PickerWindowTitle,
                PickerPrimaryMark = Ko.PickerPrimaryMark,
                PickerMonitorFormat = Ko.PickerMonitorFormat,
                ContextMenuSetPrimary = Ko.ContextMenuSetPrimary,
                EmptyMonitorMessage = Ko.EmptyMonitorMessage,
                MonitorInfoFormat = Ko.MonitorInfoFormat,
                MonitorInfoTitle = Ko.MonitorInfoTitle,
                AlreadyPrimaryMessage = Ko.AlreadyPrimaryMessage,
                SetPrimaryFailedMessage = Ko.SetPrimaryFailedMessage,
                AlreadyRunningMessage = Ko.AlreadyRunningMessage,
                UnknownErrorMessage = Ko.UnknownErrorMessage,
                ErrorPrefix = Ko.ErrorPrefix
            };
        }

        private static Set FromEnglish()
        {
            return new Set
            {
                DisplayName = En.DisplayName,
                Description = En.Description,
                TitleFormat = En.TitleFormat,
                Caution = En.Caution,
                MenuShowInfo = En.MenuShowInfo,
                MenuShowControl = En.MenuShowControl,
                MenuSetPrimary = En.MenuSetPrimary,
                MenuBackgroundCapture = En.MenuBackgroundCapture,
                MenuExit = En.MenuExit,
                AboutLabelProgram = En.AboutLabelProgram,
                AboutLabelVersion = En.AboutLabelVersion,
                AboutLabelBuildDate = En.AboutLabelBuildDate,
                AboutLabelDeveloper = En.AboutLabelDeveloper,
                AboutButtonOk = En.AboutButtonOk,
                AboutTitleFallback = En.AboutTitleFallback,
                AboutTitleSuffix = En.AboutTitleSuffix,
                PickerPrompt = En.PickerPrompt,
                PickerButtonSet = En.PickerButtonSet,
                PickerButtonCancel = En.PickerButtonCancel,
                PickerWindowTitle = En.PickerWindowTitle,
                PickerPrimaryMark = En.PickerPrimaryMark,
                PickerMonitorFormat = En.PickerMonitorFormat,
                ContextMenuSetPrimary = En.ContextMenuSetPrimary,
                EmptyMonitorMessage = En.EmptyMonitorMessage,
                MonitorInfoFormat = En.MonitorInfoFormat,
                MonitorInfoTitle = En.MonitorInfoTitle,
                AlreadyPrimaryMessage = En.AlreadyPrimaryMessage,
                SetPrimaryFailedMessage = En.SetPrimaryFailedMessage,
                AlreadyRunningMessage = En.AlreadyRunningMessage,
                UnknownErrorMessage = En.UnknownErrorMessage,
                ErrorPrefix = En.ErrorPrefix
            };
        }
    }
}
