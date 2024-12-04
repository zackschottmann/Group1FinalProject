using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;

namespace FInal
{


    public class Player
    {
        public Vector2 Position { get; set; }
        public BoundingRectangle BoundingBox { get; private set; }
        public SpriteEffects Facing { get; set; }
        public bool IsFiring { get; set; }
        public AnimationState AnimationState { get; set; }

        public Player(Vector2 position)
        {
            Position = position;
            BoundingBox = new BoundingRectangle(0, 0, 32, 32);
        }

        public void Update(GameTime gameTime)
        {
            // Update the player's velocity
            if (AnimationState == AnimationState.Moving)
                Velocity += Facing switch { SpriteEffects.FlipHorizontally => new Vector2(-10f, 0), _ => new Vector2(10f, 0) };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the player sprite
            spriteBatch.Draw(_playerSprite, Position, null, Color.White, 0f, Vector2.Zero, Facing);
        }
    }
}
