using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalMax
{
    internal class Enemy
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle BoundingBox { get; private set; }
        public bool IsAlive { get; set; } = true;
        public float Scale { get; set; } = 1.0f;

        public Enemy(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
        }

        public void Update(GameTime gameTime, float screenWidth, float screenHeight)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X < 0 || Position.X > screenWidth - Texture.Width * Scale || Position.Y > screenHeight)
            {
                IsAlive = false;
            }

            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            }
        }
    }
}