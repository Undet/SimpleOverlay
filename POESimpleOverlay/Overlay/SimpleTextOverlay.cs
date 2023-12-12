using GameOverlay.Drawing;
using GameOverlay.Windows;
using POESimpleOverlay.DataModels;

namespace POESimpleOverlay.Overlay
{
    internal class SimpleTextOverlay : IDisposable, IOverlay
    {
        private readonly GraphicsWindow _window;

        private readonly Dictionary<string, SolidBrush> _brushes;
        private readonly Dictionary<string, Font> _fonts;

        private string[] _text;
        private int _lineIndex = 0;

        private string _header = "test";
        private int _maxChars;

        public SimpleTextOverlay(TextData textData)
        {
            _text = textData.Text.ToArray();
            _brushes = new Dictionary<string, SolidBrush>();
            _fonts = new Dictionary<string, Font>();
            _maxChars = textData.WindowMaxChars;
            var a = textData.WindowSize;


            var gfx = new Graphics()
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true
            };
            //550 1020 825 35

            _window = new GraphicsWindow(a[0], a[1], a[2], a[3], gfx)
            {
                FPS = 60,
                IsTopmost = true,
                IsVisible = true
            };

            _window.DestroyGraphics += _window_DestroyGraphics;
            _window.DrawGraphics += _window_DrawGraphics;
            _window.SetupGraphics += _window_SetupGraphics;
        }

        private void _window_SetupGraphics(object? sender, SetupGraphicsEventArgs e)
        {
            var gfx = e.Graphics;

            if (e.RecreateResources)
            {
                foreach (var pair in _brushes) pair.Value.Dispose();
            }

            _brushes["black"] = gfx.CreateSolidBrush(0, 0, 0);
            _brushes["gray"] = gfx.CreateSolidBrush(40, 40, 40);
            _brushes["white"] = gfx.CreateSolidBrush(255, 255, 255);
            _brushes["background"] = gfx.CreateSolidBrush(40, 40, 40, 0.5f);
            _brushes["darkred"] = gfx.CreateSolidBrush(255, 80, 80);

            _fonts["header"] = gfx.CreateFont("Poppins", 14);
            _fonts["text"] = gfx.CreateFont("Poppins", 16);

        }

        private void _window_DrawGraphics(object? sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            var textLine = _text[_lineIndex];
            gfx.ClearScene(_brushes["background"]);

            if (TextData.IsStringHeader(textLine))
            {
                _header = textLine;
            }

            gfx.DrawText(_fonts["header"],
                _brushes["white"],
                gfx.Width - 70,
                10,
                $"({_lineIndex}/{_text.Length - 1})");


            gfx.DrawText(_fonts["header"], _brushes["darkred"], gfx.Width / 2 , 10, _header);
            gfx.DrawText(_fonts["text"], _brushes["white"], 10, 30, TextData.WrapText(textLine, (int)(_window.Width / _fonts["text"].FontSize * 2) - 10));

        }
        public void GotoPage(int index)
        {
            if (index < _text.Length && index > 0)
            { 
                _lineIndex = index;
            }
        }

        public void NextString()
        {
            if (_lineIndex >= _text.Length - 1)
            {
                return;
            }
            _lineIndex++;
        }
        public void PreviousString()
        {
            if (_lineIndex == 0)
            {
                return;
            }
            _lineIndex--;
        }
        public void ChangeVisability()
        {
            _window.IsVisible ^= true;
        }


        private void _window_DestroyGraphics(object? sender, DestroyGraphicsEventArgs e)
        {
            foreach (var pair in _brushes) pair.Value.Dispose();
            foreach (var pair in _fonts) pair.Value.Dispose();
        }

        public void Run()
        {
            _window.Create();
            _window.Join();
        }
        ~SimpleTextOverlay()
        {
            Dispose(false);
        }

        #region IDisposable Support
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _window.Dispose();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}
