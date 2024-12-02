using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FInal
{

    public enum AnimationState
    {
        Idle,      // Player is not moving
        Moving,    // Player is moving
        Firing     // Player is firing bullets
    }

    public class Player
    {
        public Vector2 Position { get; set; } // Player position
        public Rectangle BoundingBox { get; private set; } // Updated to Rectangle
        public SpriteEffects Facing { get; set; } // Direction player is facing
        public bool IsFiring { get; set; } // Whether the player is currently firing
        public AnimationState AnimationState { get; set; } // Player animation state

        private Texture2D _playerSprite; // Player texture


        // Zack: Constructor to initialize the player
        public Player(Texture2D playerSprite, Vector2 position)
        {
            _playerSprite = playerSprite;
            Position = position;

            // Initialize the bounding box to match the player size
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, _playerSprite.Width, _playerSprite.Height);
        }

        public void Update(GameTime gameTime)
        {
            // Zack: Update the player's bounding box to match its position
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, _playerSprite.Width, _playerSprite.Height);

            // Zack: Optional - Handle movement or other updates here if necessary
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
