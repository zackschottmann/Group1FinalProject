using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FInal
{

    public class Bullet
    {
        public Vector2 Position { get; set; }
        public BoundingRectangle BoundingBox { get; private set; }
        public SpriteEffects Facing { get; set; }

        public bool IsMoving { get; set; }

        public Bullet(Vector2 position, Vector2 velocity)
        {
            Vector2 Position = position;
            Vector2 Velocity = velocity;

            // Initialize the bounding box
            BoundingBox = new BoundingRectangle(0, 0, 10, 10);
        }
    }
}
