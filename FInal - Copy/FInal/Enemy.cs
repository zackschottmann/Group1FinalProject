using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FInal
{
    internal class Enemy
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle BoundingBox { get; private set; } // Changed to Rectangle
        public bool IsAlive { get; set; } = true;

        public Enemy(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;

            // Initialize the bounding box
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the bounding box
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
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
