using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Project1
{
    public class Bullet
    {
        #region Fields

        private Texture2D _texture;
        private Vector2 _position;
        private float _speed;

        #endregion

        #region Properties

        public Vector2 Position => _position;

        #endregion

        #region Constructor

        public Bullet(Texture2D texture, Vector2 startPosition, float speed = 10f)
        {
            _texture = texture;
            _position = startPosition;
            _speed = speed;
        }

        #endregion

        #region Methods

        public void Update(GameTime gameTime)
        {
            _position.Y -= _speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }

        public bool IsOffScreen()
        {
            return _position.Y < 0;
        }

        #endregion
    }
}
