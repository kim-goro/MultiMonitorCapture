namespace MultiMonitorCapture.Domain.Enums
{
    // 캡처 엔진의 동작 상태
    public enum CaptureState
    {
        // 정지 (스레드 없음)
        Stopped = 0,

        // 캡처 진행 중
        Running = 1,

        // 일시정지 (스레드는 유지, 캡처만 중단)
        Paused = 2
    }
}
