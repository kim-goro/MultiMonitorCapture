using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Service
{
    // 설정 로드와 저장을 담당한다. 현재 설정을 메모리에 보관한다.
    public sealed class SettingsService
    {
        private readonly ISettingsRepository _repository;

        public SettingsService(ISettingsRepository repository)
        {
            _repository = repository;
            Current = _repository.Load();
        }

        // 현재 적용 중인 설정
        public AppSettings Current { get; private set; }

        // 현재 설정을 저장소에 기록한다
        public void Save()
        {
            _repository.Save(Current);
        }
    }
}
