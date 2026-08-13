using MultiMonitorCapture.Api;
using MultiMonitorCapture.Designer;
using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Models;
using MultiMonitorCapture.Infrastructure.Capture;
using MultiMonitorCapture.Infrastructure.Monitors;
using MultiMonitorCapture.Infrastructure.Persistence;
using MultiMonitorCapture.Infrastructure.Platform;
using MultiMonitorCapture.Presentation.Presenters;
using MultiMonitorCapture.Properties;
using MultiMonitorCapture.Service;

namespace MultiMonitorCapture
{
    // 합성 루트. 외부 DI 컨테이너 없이 의존성을 수동으로 조립한다.
    internal static class Bootstrapper
    {
        public static AppRunner Build()
        {
            // 인프라 구현 (OS/파일 의존)
            IScreenCapturer capturer = new GdiScreenCapturer();
            IMonitorProvider monitorProvider = new WinFormsMonitorProvider();
            ISettingsRepository settingsRepository = new IniSettingsRepository();
            IPrimaryMonitorController primaryController = new PrimaryMonitorController();

            // 서비스 (업무 로직)
            MonitorService monitorService = new MonitorService(monitorProvider);
            CaptureService captureService = new CaptureService(capturer);
            SettingsService settingsService = new SettingsService(settingsRepository);
            PrimaryMonitorService primaryService = new PrimaryMonitorService(primaryController);

            // 프로그램 정보. 표기 문구는 AppStrings, 정체성은 AppMetadata,
            // 빌드일자는 BuildInfo(빌드마다 자동 생성)에서 각각 가져온다.
            AppInfo info = new AppInfo(
                displayName: AppStrings.Cur.DisplayName,
                version: AppMetadata.Version,
                buildDate: BuildInfo.BuildDate,
                developer: AppMetadata.Developer,
                caution: AppStrings.Cur.Caution,
                titleFormat: AppStrings.Cur.TitleFormat);

            // 파사드 (UI 진입점)
            IAppFacade facade = new AppFacade(info, monitorService, captureService, settingsService, primaryService);

            // 화면과 프레젠터
            ControlForm controlForm = new ControlForm();
            CaptureTileFactory tileFactory = new CaptureTileFactory();
            ControlPresenter presenter = new ControlPresenter(facade, controlForm, tileFactory);
            TrayComponent tray = new TrayComponent(AppStrings.Cur.DisplayName + " v" + AppMetadata.Version, facade.Settings.BackgroundCaptureEnabled);

            return new AppRunner(facade, controlForm, tray, presenter);
        }
    }
}
