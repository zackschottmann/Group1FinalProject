using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Project1
{
    public class Player
    {
        #region Fields

        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _speed;
        private KeyboardState _currentKeyState;
        private KeyboardState _previousKeyState;

        #endregion

        #region Properties

        public Vector2 Position => _position;
        public List<Bullet> Bullets { get; private set; } = new List<Bullet>();

        #endregion

        #region Constructor

        public Player(Texture2D texture, Vector2 startPosition, float speed = 5f)
        {
            _texture = texture;
            _position = startPosition;
            _speed = speed;
        }

        #endregion

        #region Methods

        public void Update(GameTime gameTime, Texture2D bulletTexture)
        {
            _currentKeyState = Keyboard.GetState();

            HandleMovement();
            HandleShooting(bulletTexture);

            UpdateBullets(gameTime);

            _previousKeyState = _currentKeyState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);

            foreach (var bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }

        private void HandleMovement()
        {
            Vector2 direction = Vector2.Zero;

            if (_currentKeyState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (_currentKeyState.IsKeyDown(Keys.S))
                direction.Y += 1;
            if (_currentKeyState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (_currentKeyState.IsKeyDown(Keys.D))
                direction.X += 1;

            direction.Normalize();
            _velocity = direction * _speed;
            _position += _velocity;
        }

        private void HandleShooting(Texture2D bulletTexture)
        {
            if (_currentKeyState.IsKeyDown(Keys.Space) && _previousKeyState.IsKeyUp(Keys.Space))
            {
                Bullets.Add(new Bullet(bulletTexture, new Vector2(_position.X + _texture.Width / 2, _position.Y)));
            }
        }

        private void UpdateBullets(GameTime gameTime)
        {
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                Bullets[i].Update(gameTime);
                if (Bullets[i].IsOffScreen())
                {
                    Bullets.RemoveAt(i);
                }
            }
        }

        #endregion
    }
}
