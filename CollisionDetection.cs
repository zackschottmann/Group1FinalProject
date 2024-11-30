using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FInal
{
    internal class CollisionDetection
    {
        public static bool CheckCollision(BoundingRectangle rect1, BoundingRectangle rect2)
        {
            // Check if the rectangles intersect
            return !rect1.Intersects(rect2);
        }
    }
}
