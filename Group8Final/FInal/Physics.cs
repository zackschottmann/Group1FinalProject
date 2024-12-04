using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FInal
{

    public enum AnimationState
    {
        Idle,      
        Moving,    
        Firing     
    }

    public class Player
    {
        public Vector2 Position { get; set; }
        public Rectangle BoundingBox { get; private set; }
        public SpriteEffects Facing { get; set; } 
        public bool IsFiring { get; set; } 
        public AnimationState AnimationState { get; set; } 

        private Texture2D _playerSprite; 


        public Player(Texture2D playerSprite, Vector2 position)
        {
            _playerSprite = playerSprite;
            Position = position;


            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, _playerSprite.Width, _playerSprite.Height);
        }

        public void Update(GameTime gameTime)
        {
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, _playerSprite.Width, _playerSprite.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _playerSprite,
                Position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                Facing,
                0f
            );
        }

    }
}
