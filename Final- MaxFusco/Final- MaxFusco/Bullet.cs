using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalMax
{
    public class Bullet
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Rectangle BoundingBox { get; private set; }
        public Texture2D Texture { get; set; }
        public bool IsMoving { get; set; }


        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            IsMoving = true;


            BoundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }


        public void Update(GameTime gameTime)
        {
            if (IsMoving)
            {
                Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;


                BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);


                if (Position.Y < 0 || Position.Y > 600)
                    IsMoving = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsMoving)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
        }
    }
}