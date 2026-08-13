namespace MultiMonitorCapture.Designer
{
    // 디자이너 코드. 초기 구성과 자원 해제를 담당한다.
    partial class CaptureTile
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();

                // 렌더링 자원과 이미지, 타이머를 해제해 GDI 누수를 방지한다
                if (_image != null) { _image.Dispose(); _image = null; }
                if (_clickTimer != null) { _clickTimer.Dispose(); _clickTimer = null; }
                if (_numberFont != null) { _numberFont.Dispose(); _numberFont = null; }
                if (_barFont != null) { _barFont.Dispose(); _barFont = null; }
                if (_overlayBrush != null) { _overlayBrush.Dispose(); _overlayBrush = null; }
                if (_barBrush != null) { _barBrush.Dispose(); _barBrush = null; }
                if (_hoverPen != null) { _hoverPen.Dispose(); _hoverPen = null; }
                if (_clickPen != null) { _clickPen.Dispose(); _clickPen = null; }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
            this.Name = "CaptureTile";
            this.Size = new System.Drawing.Size(240, 160);
            this.ResumeLayout(false);
        }
    }
}
