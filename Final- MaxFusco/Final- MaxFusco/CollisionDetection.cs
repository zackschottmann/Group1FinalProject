using Microsoft.Xna.Framework;

namespace FinalMax
{
    internal class CollisionDetection
    {
        public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Intersects(rect2);
        }
    }
}