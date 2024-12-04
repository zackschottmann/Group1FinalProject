using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FInal
{


    public class Alien
    {
        public Vector2 Position { get; set; }
        public BoundingRectangle BoundingBox { get; private set; }
        public SpriteEffects Facing { get; set; }
        public bool IsFiring { get; set; }

        public Alien(Vector2 position)
        {
            Position = position;
            BoundingBox = new BoundingRectangle(0, 0, 32, 32);
        }

        public void Update(GameTime gameTime)
        {
            // Move the alien horizontally
            Velocity += Facing switch { SpriteEffects.FlipHorizontally => new Vector2(-10f, 0), _ => new Vector2(10f, 0) };
        }
    }
}
