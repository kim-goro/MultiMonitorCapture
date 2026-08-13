using System;
using System.Collections.Generic;
using System.IO;
using MultiMonitorCapture.Domain.Abstractions;
using MultiMonitorCapture.Domain.Models;

namespace MultiMonitorCapture.Infrastructure.Persistence
{
    // 단순 INI(키=값) 파일로 설정을 저장한다. 역직렬화 위험 방식을 쓰지 않는다.
    public sealed class IniSettingsRepository : ISettingsRepository
    {
        private const string KeyFps = "CaptureFps";
        private const string KeyClick = "ClickMarkerMs";
        private const string KeyBackground = "BackgroundCaptureEnabled";

        private readonly string _filePath;

        public IniSettingsRepository()
        {
            // 사용자별 쓰기 가능 위치에 저장한다 (Program Files 등 시스템 폴더에 쓰지 않는다)
            string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appDir = Path.Combine(baseDir, "MultiMonitorCapture");
            _filePath = Path.Combine(appDir, "settings.ini");
        }

        public AppSettings Load()
        {
            AppSettings settings = new AppSettings();
            try
            {
                if (!File.Exists(_filePath))
                {
                    return settings;
                }

                Dictionary<string, string> map = ReadPairs(_filePath);

                int fps;
                if (map.ContainsKey(KeyFps) && int.TryParse(map[KeyFps], out fps))
                {
                    settings.CaptureFps = fps; // setter 가 범위를 강제한다
                }

                int clickMs;
                if (map.ContainsKey(KeyClick) && int.TryParse(map[KeyClick], out clickMs))
                {
                    settings.ClickMarkerMs = clickMs;
                }

                if (map.ContainsKey(KeyBackground))
                {
                    settings.BackgroundCaptureEnabled = ParseBool(map[KeyBackground], true);
                }
            }
            catch
            {
                // 손상된 파일 등 어떤 경우에도 기본값으로 안전하게 기동한다
                return new AppSettings();
            }
            return settings;
        }

        public void Save(AppSettings settings)
        {
            try
            {
                string dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                using (StreamWriter writer = new StreamWriter(_filePath, false))
                {
                    writer.WriteLine(KeyFps + "=" + settings.CaptureFps);
                    writer.WriteLine(KeyClick + "=" + settings.ClickMarkerMs);
                    writer.WriteLine(KeyBackground + "=" + (settings.BackgroundCaptureEnabled ? "true" : "false"));
                }
            }
            catch
            {
                // 저장 실패가 프로그램을 중단시키지 않도록 무시한다 (다음 실행에 기본값 사용)
            }
        }

        private static Dictionary<string, string> ReadPairs(string path)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach (string raw in File.ReadAllLines(path))
            {
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#") || line.StartsWith(";"))
                {
                    continue;
                }
                int eq = line.IndexOf('=');
                if (eq <= 0)
                {
                    continue;
                }
                string key = line.Substring(0, eq).Trim();
                string value = line.Substring(eq + 1).Trim();
                map[key] = value;
            }
            return map;
        }

        private static bool ParseBool(string value, bool fallback)
        {
            if (string.IsNullOrEmpty(value)) return fallback;
            string v = value.Trim().ToLowerInvariant();
            if (v == "true" || v == "1" || v == "yes") return true;
            if (v == "false" || v == "0" || v == "no") return false;
            return fallback;
        }
    }
}
