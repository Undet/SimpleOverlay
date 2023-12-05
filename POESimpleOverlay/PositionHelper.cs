using System.Numerics;

namespace POESimpleOverlay
{
    internal class PositionHelper
    {
        public enum ItemPosition
        {
            TopLeft = 1,
            TopCenter = 2,
            TopRight = 3,
            CenterLeft = 4,
            CenterCenter = 5,
            CenterRight = 6,
            BottomLeft = 7,
            BottomCenter = 8,
            BottomRight = 9
        }

        public static Vector4 CalculateElementBounds(Vector2 windowSize, Vector2 elementSize, float sizeMultiplier, ItemPosition itemPosition)
        {
            Vector4 result;
            Vector2 elementPoint1 = CalculetePoint1(windowSize, elementSize * sizeMultiplier, itemPosition);
            Vector2 elementPoint2 = CalculetePoint2(elementPoint1, elementSize * sizeMultiplier);

            result = new Vector4(
                elementPoint1.X,
                elementPoint1.Y,
                elementPoint2.X,
                elementPoint2.Y);

            return result;
        }

        private static Vector2 CalculetePoint2(Vector2 elementPoint1, Vector2 elementSize)
        {
            return new Vector2(elementPoint1.X + elementSize.X, elementPoint1.Y + elementSize.Y);
        }

        private static Vector2 CalculetePoint1(Vector2 windowSize, Vector2 elementSize, ItemPosition itemPosition)
        {
            Vector2 point1;
            switch (itemPosition)
            {
                case ItemPosition.TopLeft:
                    point1 = new Vector2(0, 0); break;
                case ItemPosition.TopCenter:
                    point1 = new Vector2(windowSize.X / 2 - elementSize.X / 2, 0); break;
                case ItemPosition.TopRight:
                    point1 = new Vector2(windowSize.X - elementSize.X, 0); break;


                case ItemPosition.CenterLeft:
                    point1 = new Vector2(0, windowSize.Y / 2 - elementSize.Y / 2); break;
                case ItemPosition.CenterCenter:
                    point1 = new Vector2(windowSize.X / 2 - elementSize.X / 2, windowSize.Y / 2 - elementSize.Y / 2); break;
                case ItemPosition.CenterRight:
                    point1 = new Vector2(windowSize.X - elementSize.X, windowSize.Y / 2 - elementSize.Y / 2); break;


                case ItemPosition.BottomLeft:
                    point1 = new Vector2(0, windowSize.Y - elementSize.Y); break;
                case ItemPosition.BottomCenter:
                    point1 = new Vector2(windowSize.X / 2 - elementSize.X / 2, windowSize.Y - elementSize.Y); break;
                case ItemPosition.BottomRight:
                    point1 = new Vector2(windowSize.X - elementSize.X, windowSize.Y - elementSize.Y); break;

                default:
                    point1 = new Vector2(0, 0); break;
            }

            return point1;
        }
    }
}
