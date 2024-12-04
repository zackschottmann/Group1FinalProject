using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalMax
{


    public class Alien
    {
        public Vector2 Position { get;  set; }
        public Texture2D Texture { get;  set; }
        public bool IsAlive { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public Vector2 Velocity; // Horizontal movement speed.
        private int ScreenWidth;

        public Alien(Texture2D texture, Vector2 startPosition, Vector2 velocity, int screenWidth)
        {
            Texture = texture;
            Position = startPosition;
            Velocity = velocity;
            ScreenWidth = screenWidth;
            IsAlive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsAlive) return;

            // Move horizontally.
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Reverse direction on screen edge hit.
            if (Position.X <= 0 || Position.X + Texture.Width >= ScreenWidth)
            {
                Velocity.X = -Velocity.X;
                Position += Velocity; // Adjust position to prevent sticking.
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
        }
    }
}
