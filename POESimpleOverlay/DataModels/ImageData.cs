using GameOverlay.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POESimpleOverlay.DataModels
{
    internal class ImageData
    {
        public string Path { get; set; }
        public PositionHelper.ItemPosition Position { get; set; }
        public float SizeMultiplier { get; set; }
        public float Opacity { get; set; }

        public ImageData(string path, PositionHelper.ItemPosition position, float sizeMultiplier = 1f, float opacity = 0.7f)
        {
            Path = path;
            Position = position;
            SizeMultiplier = sizeMultiplier;
            Opacity = opacity;
        }

    }
}
