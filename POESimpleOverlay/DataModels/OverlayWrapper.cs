using POESimpleOverlay.Overlay;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POESimpleOverlay.DataModels
{
    internal class OverlayWrapper
    {
        private static int _ID = 0;
        public IOverlay Overlay { get;set; }
        public OverlayType Type { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Path { get; set; }
        public enum OverlayType
        {
            Image,
            Text
        }


        public OverlayWrapper(IOverlay overlay, OverlayType overlayType, string name, string path)
        {
            this.Id = System.Threading.Interlocked.Increment(ref _ID);
            Overlay = overlay;
            Type = overlayType;
            Name = name;
            Path = path;
        }

        public static List<SimpleImageOverlay> GetImagesOverlays(List<OverlayWrapper> overlays)
        {
            List<SimpleImageOverlay> simpleImageOverlays = new List<SimpleImageOverlay>();
            foreach (var overlay in overlays)
            {
                if (overlay.Type == OverlayType.Image)
                {
                    simpleImageOverlays.Add((SimpleImageOverlay)overlay.Overlay);
                }
            }
            return simpleImageOverlays;
        }

        public static List<SimpleTextOverlay> GetTextOverlays(List<OverlayWrapper> overlays)
        {
            List<SimpleTextOverlay> simpleImageOverlays = new List<SimpleTextOverlay>();
            foreach (var overlay in overlays)
            {
                if (overlay.Type == OverlayType.Text)
                {
                    simpleImageOverlays.Add((SimpleTextOverlay)overlay.Overlay);
                }
            }
            return simpleImageOverlays;
        }
        
        public override string? ToString()
        {
            return $"Name: {Name}, Id: {Id}, Type: {Type}";
        }
    }
}
