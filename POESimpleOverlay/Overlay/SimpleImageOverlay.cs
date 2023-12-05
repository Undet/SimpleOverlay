using GameOverlay.Drawing;
using GameOverlay.Windows;
using POESimpleOverlay.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace POESimpleOverlay.Overlay
{
    internal class SimpleImageOverlay : IDisposable, IOverlay
    {
        private readonly GraphicsWindow _window;

        private readonly Dictionary<string, SolidBrush> _brushes;
        private readonly Dictionary<string, Font> _fonts;

        private string _path;
        private PositionHelper.ItemPosition _itemPosition;
        private float _sizeMultiplier;
        private float _opacity;

        private Vector4 _position;
        private Image _image;
        

        public SimpleImageOverlay(ImageData imageData)
        {
            _brushes = new Dictionary<string, SolidBrush>();
            _fonts = new Dictionary<string, Font>();

            _path = imageData.Path;
            _itemPosition = imageData.Position;
            _sizeMultiplier = imageData.SizeMultiplier;
            _opacity = imageData.Opacity;

            var gfx = new Graphics()
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true
            };

            //550 1020 825 35

            _window = new GraphicsWindow(0, 0, 1920, 1080, gfx)
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

            _brushes["background"] = gfx.CreateSolidBrush(0, 0, 0, 0.0f);
            _image = new Image(gfx, _path);


            _position = PositionHelper.CalculateElementBounds(
                new Vector2(_window.Width, _window.Height),
                new Vector2(_image.Width, _image.Height),
                _sizeMultiplier,
                _itemPosition
                );

            if (e.RecreateResources) return;
        }

        private void _window_DrawGraphics(object? sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;

            gfx.ClearScene(_brushes["background"]);
            gfx.DrawImage(
                _image,
                _position.X,
                _position.Y,
                _position.Z,
                _position.W,
                _opacity);
        }

        private void _window_DestroyGraphics(object? sender, DestroyGraphicsEventArgs e)
        {
            foreach (var pair in _brushes) pair.Value.Dispose();
            foreach (var pair in _fonts) pair.Value.Dispose();
        }
        public void ChangeVisability()
        {
            _window.IsVisible ^= true;
        }

        public void Run()
        {
            _window.Create();
            _window.Join();
        }
        ~SimpleImageOverlay()
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
