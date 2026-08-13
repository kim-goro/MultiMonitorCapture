# 멀티모니터캡처 (MultiMonitorCapture)

로컬에 연결된 다중 모니터의 화면을 실시간으로 캡처하여 격자로 보여주는 Windows 데스크톱 도구입니다.
인터넷이 차단된 설비PC 환경을 우선 대상으로 하며, 단일 실행 파일(exe)로 동작합니다.

## 지원 환경
- Windows XP SP3 / Windows 10 / Windows 11 (동일 exe)
- 실행 요구: .NET Framework 4.0 (Windows 10/11 은 대부분 내장, XP 는 별도 설치 필요할 수 있음)

## 주요 기능
- 주 모니터를 제외한 모든 모니터를 격자로 실시간 캡처 표시
- 각 캡처 타일에 모니터 번호(좌상단), 모니터 이름과 설정 버튼(하단) 표시
- 마우스가 올라간 타일을 노란색 테두리로 강조
- 타일 안에서 클릭 시 노란색 원을 일시 표시
- 컨트롤창 닫기는 종료가 아닌 백그라운드 숨김, 시스템 트레이로 상주
- 트레이 메뉴: 정보 보기 / 컨트롤창 띄우기 / 메인 모니터 설정 / 백그라운드 캡처 on-off / 종료
- 트레이에서 Windows 주 모니터를 실제로 변경(디스플레이 설정과 동일 동작)

## 취급 주의
본 프로그램은 화면을 실시간으로 캡처합니다. 민감 정보 노출에 유의하십시오.
캡처 영상은 로컬 메모리에만 존재하며 디스크 저장이나 외부 전송을 하지 않습니다.

## 빌드
- 개발 환경: VSCode + C# 확장 + .NET SDK(dotnet)
- 대상: .NET Framework 4.0 단일 exe (런타임 종속 DLL 없음)
- 빌드 전용 참조 어셈블리: Microsoft.NETFramework.ReferenceAssemblies.net40
  (PrivateAssets=all, exe 에 포함되지 않는 개발용 참조. 최초 1회 인터넷 복원 필요, 이후 캐시로 오프라인 빌드)
- 빌드 명령: build/build.ps1 실행, 또는 `dotnet build src/MultiMonitorCapture.csproj -c Release`

## 무결성 확인
릴리스로 배포되는 exe 는 코드 서명 대신 SHA-256 해시값을 함께 공개합니다.
아래 명령으로 계산한 값이 릴리스 노트의 값과 일치하는지 확인하십시오.
```
Get-FileHash .\MultiMonitorCapture.exe -Algorithm SHA256
```

## 설계 문서
- docs/00_개발프롬프트.md : 개발 지시서
- docs/01_기능명세서.md : 기능 상세 명세
- docs/02_아키텍처설계.md : 계층/패턴/폴더구조
- docs/03_보안요구사항체크리스트.md : 보안 점검 체크리스트
- docs/04_깃허브배포가이드.md : 업로드/공개배포 가이드

## 라이선스
MIT License. 자세한 내용은 LICENSE 파일을 참조하십시오.
