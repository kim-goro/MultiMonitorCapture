using System;
using System.Collections.Generic;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Designer
{
    // 캡처 타일 생성을 캡슐화하는 팩토리. 메인 스크립트에서 컨트롤을 즉석 생성하지 않도록 한다.
    public sealed class CaptureTileFactory
    {
        // 대상 모니터 목록으로 타일을 생성한다. 배치는 뷰(폼)가 담당한다.
        public IList<CaptureTile> CreateTiles(IEnumerable<MonitorInfo> targets, int clickMarkerMs, EventHandler settingsHandler)
        {
            List<CaptureTile> tiles = new List<CaptureTile>();
            foreach (MonitorInfo m in targets)
            {
                CaptureTile tile = new CaptureTile();
                tile.Configure(m.Number, m.DeviceName, clickMarkerMs);
                if (settingsHandler != null)
                {
                    tile.SettingsRequested += settingsHandler;
                }
                tiles.Add(tile);
            }
            return tiles;
        }
    }
}
