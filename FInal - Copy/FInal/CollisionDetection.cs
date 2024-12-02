using Microsoft.Xna.Framework;

namespace FInal
{
    internal class CollisionDetection
    {
        public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            // Check if the rectangles intersect
            return rect1.Intersects(rect2);
        }
    }
}
