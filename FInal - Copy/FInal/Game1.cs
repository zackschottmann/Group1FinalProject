using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media; // For background music
using Microsoft.Xna.Framework.Audio; // For sound effects
using System.Collections.Generic;
using System;

namespace FInal
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Zack: Updated to use your custom assets
        private Texture2D _backgroundTexture; // Scrolling background
        private Texture2D _playerTexture; // Player sprite (SpaceShip.png)
        private Texture2D _enemyTexture; // Enemy sprite (enemy.png)
        private Texture2D _bulletTexture; // Bullet sprite (Bullet.png)

        private SoundEffect _shootSound; // Shooting sound
        private SoundEffect _explosionSound; // Explosion sound
        private Song _backgroundMusic; // Background music

        private Vector2 _playerPosition; // Player position
        private float _playerSpeed = 200f; // Player movement speed

        private Vector2 _backgroundPosition1; // Background position 1
        private Vector2 _backgroundPosition2; // Background position 2

        private List<Enemy> _enemies = new List<Enemy>(); // Enemy list
        private List<Bullet> _bullets = new List<Bullet>(); // Bullet list

        private static Random Rand = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set the game window size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            // Zack: Initialize background and player position
            _backgroundPosition1 = Vector2.Zero;
            _backgroundPosition2 = new Vector2(0, -_graphics.PreferredBackBufferHeight);
            _playerPosition = new Vector2(400, 500); // Centered at the bottom of the screen

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Zack: Load custom assets
            _backgroundTexture = Content.Load<Texture2D>("Graphics/Backround");
            _playerTexture = Content.Load<Texture2D>("Graphics/SpaceShip");
            _enemyTexture = Content.Load<Texture2D>("Graphics/enemy");
            _bulletTexture = Content.Load<Texture2D>("Graphics/Bullet");

            // Zack: Load audio
            _shootSound = Content.Load<SoundEffect>("Audio/invaderkilled");
            _backgroundMusic = Content.Load<Song>("Audio/space-station-247790");


            // Zack: Play background music in a loop
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic);

            // Zack: Spawn enemies at random positions
            for (int i = 0; i < 5; i++)
            {
                _enemies.Add(new Enemy(_enemyTexture, new Vector2(Rand.Next(0, 750), Rand.Next(0, 300)), new Vector2(50f, 0)));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            HandlePlayerInput(gameTime);

          
            UpdateBackground(gameTime);

           
            UpdateEnemies(gameTime);
            UpdateBullets(gameTime);

            base.Update(gameTime);
        }

        private void HandlePlayerInput(GameTime gameTime)
        {
         
            KeyboardState state = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (state.IsKeyDown(Keys.Left) && _playerPosition.X > 0)
                _playerPosition.X -= _playerSpeed * deltaTime;

            if (state.IsKeyDown(Keys.Right) && _playerPosition.X < _graphics.PreferredBackBufferWidth - _playerTexture.Width)
                _playerPosition.X += _playerSpeed * deltaTime;

            if (state.IsKeyDown(Keys.Up) && _playerPosition.Y > 0)
                _playerPosition.Y -= _playerSpeed * deltaTime;

            if (state.IsKeyDown(Keys.Down) && _playerPosition.Y < _graphics.PreferredBackBufferHeight - _playerTexture.Height)
                _playerPosition.Y += _playerSpeed * deltaTime;

  
            if (state.IsKeyDown(Keys.Space))
            {
                ShootBullet();
            }
        }

        private void ShootBullet()
        {
            Vector2 bulletPosition = new Vector2(_playerPosition.X + _playerTexture.Width / 2 - _bulletTexture.Width / 2, _playerPosition.Y);
            _bullets.Add(new Bullet(_bulletTexture, bulletPosition, new Vector2(0, -400)));

            // Zack: Play shooting sound
            _shootSound.Play();
        }

        private void UpdateBackground(GameTime gameTime)
        {
            // Zack: Scroll the background downwards
            float scrollSpeed = 100f;
            _backgroundPosition1.Y += scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _backgroundPosition2.Y += scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Reset positions when the background moves out of view
            if (_backgroundPosition1.Y >= _graphics.PreferredBackBufferHeight)
                _backgroundPosition1.Y = _backgroundPosition2.Y - _graphics.PreferredBackBufferHeight;

            if (_backgroundPosition2.Y >= _graphics.PreferredBackBufferHeight)
                _backgroundPosition2.Y = _backgroundPosition1.Y - _graphics.PreferredBackBufferHeight;
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            foreach (var enemy in _enemies.ToArray())
            {
                enemy.Update(gameTime);

                // Zack: Check collision with the player
                if (CollisionDetection.CheckCollision(new Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, _playerTexture.Width, _playerTexture.Height), enemy.BoundingBox))
                {
                    // Zack: Play explosion sound and exit game
                    _explosionSound.Play();
                    Exit();
                }
            }
        }

        private void UpdateBullets(GameTime gameTime)
        {
            foreach (var bullet in _bullets.ToArray())
            {
                bullet.Update(gameTime);

                // Zack: Remove bullets out of bounds
                if (bullet.Position.Y < 0)
                    _bullets.Remove(bullet);

                // Zack: Check collision with enemies
                foreach (var enemy in _enemies.ToArray())
                {
                    if (CollisionDetection.CheckCollision(bullet.BoundingBox, enemy.BoundingBox))
                    {
                        _explosionSound.Play();
                        _bullets.Remove(bullet);
                        _enemies.Remove(enemy);
                        break;
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Zack: Draw scrolling background
            _spriteBatch.Draw(_backgroundTexture, _backgroundPosition1, Color.White);
            _spriteBatch.Draw(_backgroundTexture, _backgroundPosition2, Color.White);

            // Zack: Draw player
            _spriteBatch.Draw(_playerTexture, _playerPosition, Color.White);

            // Zack: Draw enemies
            foreach (var enemy in _enemies)
            {
                _spriteBatch.Draw(_enemyTexture, enemy.Position, Color.White);
            }

            // Zack: Draw bullets
            foreach (var bullet in _bullets)
            {
                _spriteBatch.Draw(_bulletTexture, bullet.Position, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
