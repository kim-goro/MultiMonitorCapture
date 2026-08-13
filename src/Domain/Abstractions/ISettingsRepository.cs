using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Domain.Abstractions
{
    // 설정 영속화 계약 (Repository). 저장 매체는 구현에서 결정한다.
    public interface ISettingsRepository
    {
        // 설정을 읽는다. 파일이 없거나 손상된 경우 기본값을 반환한다.
        AppSettings Load();

        // 설정을 저장한다. 실패해도 예외로 앱을 중단시키지 않는다.
        void Save(AppSettings settings);
    }
}
