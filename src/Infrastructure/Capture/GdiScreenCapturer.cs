using System;
using System.Drawing;
using System.Drawing.Imaging;
using MultiMonitorCapture.Domain.Abstractions;

namespace MultiMonitorCapture.Infrastructure.Capture
{
    // GDI BitBlt 기반 화면 캡처 구현. CopyFromScreen 은 내부적으로 BitBlt 를 사용하며 WinXP 부터 지원된다.
    public sealed class GdiScreenCapturer : IScreenCapturer
    {
        public Bitmap Capture(Rectangle bounds)
        {
            // 잘못된 영역 방어
            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                throw new ArgumentException("캡처 영역 크기가 올바르지 않습니다.");
            }

            // 32비트 비트맵에 화면 내용을 복사한다. 실패 시 비트맵을 해제하고 예외를 전달한다.
            Bitmap bmp = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            try
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
                }
                return bmp;
            }
            catch
            {
                bmp.Dispose();
                throw;
            }
        }
    }
}
