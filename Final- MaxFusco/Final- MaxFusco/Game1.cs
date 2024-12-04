using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;

namespace FinalMax
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _backgroundTexture;
        private Texture2D _playerTexture;
        private Texture2D _enemyTexture;
        private Texture2D _bulletTexture;
        private SpriteFont _font;

        private SoundEffect _shootSound;
        private SoundEffect _explosionSound;
        private Song _backgroundMusic;

        private Vector2 _playerPosition;
        private float _playerSpeed = 500f;
        private float _playerScale = 0.8f;

        private Vector2 _backgroundPosition1;
        private Vector2 _backgroundPosition2;
        private float _backgroundScrollSpeed = 100f;

        private List<Enemy> _enemies = new List<Enemy>();
        private List<Bullet> _bullets = new List<Bullet>();

        private double _enemySpawnTimer = 0;
        private double _bulletCooldownTimer = 0;
        private double _gameTimer = 0;

        private int _playerScore = 0;
        private static Random Rand = new Random();

        private bool _isGameOver = false;
        private string _endGameMessage = "";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();

            _playerPosition = new Vector2(512, 700);

            _backgroundPosition1 = Vector2.Zero;
            _backgroundPosition2 = new Vector2(0, -_graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _backgroundTexture = Content.Load<Texture2D>("Graphics/Backround");
            _playerTexture = Content.Load<Texture2D>("Graphics/SpaceShip");
            _enemyTexture = Content.Load<Texture2D>("Graphics/enemy");
            _bulletTexture = Content.Load<Texture2D>("Graphics/Bullet");
            _font = Content.Load<SpriteFont>("Fonts/timerFont");

            _shootSound = Content.Load<SoundEffect>("Audio/invaderkilled");
            _explosionSound = Content.Load<SoundEffect>("Audio/invaderkilled");
            _backgroundMusic = Content.Load<Song>("Audio/space-station-247790");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_isGameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    RestartGame();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
                return;
            }

            _gameTimer += gameTime.ElapsedGameTime.TotalSeconds;

            HandlePlayerInput(gameTime);
            UpdateBackground(gameTime);
            UpdateEnemies(gameTime);
            UpdateBullets(gameTime);
            SpawnEnemies(gameTime);

            CheckWinOrLossConditions();

            base.Update(gameTime);
        }

        private void HandlePlayerInput(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (state.IsKeyDown(Keys.Left) && _playerPosition.X > 0)
                _playerPosition.X -= _playerSpeed * deltaTime;

            if (state.IsKeyDown(Keys.Right) && _playerPosition.X < _graphics.PreferredBackBufferWidth - (_playerTexture.Width * _playerScale))
                _playerPosition.X += _playerSpeed * deltaTime;

            if (state.IsKeyDown(Keys.Up) && _playerPosition.Y > 0)
                _playerPosition.Y -= _playerSpeed * deltaTime;

            if (state.IsKeyDown(Keys.Down) && _playerPosition.Y < _graphics.PreferredBackBufferHeight - (_playerTexture.Height * _playerScale))
                _playerPosition.Y += _playerSpeed * deltaTime;

            if (state.IsKeyDown(Keys.Space))
            {
                if (_bulletCooldownTimer <= 0)
                {
                    ShootBullet();
                    _bulletCooldownTimer = 0.5;
                }
            }

            _bulletCooldownTimer -= deltaTime;
        }

        private void ShootBullet()
        {
            Vector2 bulletPosition = new Vector2(
                _playerPosition.X + (_playerTexture.Width * _playerScale) / 2 - _bulletTexture.Width / 2,
                _playerPosition.Y);
            _bullets.Add(new Bullet(_bulletTexture, bulletPosition, new Vector2(0, -400)));
            _shootSound.Play();
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            foreach (var enemy in _enemies.ToArray())
            {
                enemy.Update(gameTime, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                if (!enemy.IsAlive)
                {
                    _enemies.Remove(enemy);
                }
                else if (CollisionDetection.CheckCollision(
                    new Rectangle(
                        (int)_playerPosition.X,
                        (int)_playerPosition.Y,
                        (int)(_playerTexture.Width * _playerScale),
                        (int)(_playerTexture.Height * _playerScale)),
                    enemy.BoundingBox))
                {
                    EndGame("You blew up!");
                }
            }
        }

        private void UpdateBullets(GameTime gameTime)
        {
            foreach (var bullet in _bullets.ToArray())
            {
                bullet.Update(gameTime);

                if (bullet.Position.Y < 0)
                {
                    _bullets.Remove(bullet);
                }

                foreach (var enemy in _enemies.ToArray())
                {
                    if (CollisionDetection.CheckCollision(bullet.BoundingBox, enemy.BoundingBox))
                    {
                        _explosionSound.Play();
                        _bullets.Remove(bullet);
                        _enemies.Remove(enemy);
                        _playerScore++;
                        break;
                    }
                }
            }
        }

        // Author : Zack Schottmann
        // A method to use two images and have them scroll
        // It repositions images above each other when they move off-screen, ensuring a seamless loop.

        private void UpdateBackground(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _backgroundPosition1.Y += _backgroundScrollSpeed * deltaTime;
            _backgroundPosition2.Y += _backgroundScrollSpeed * deltaTime;

            if (_backgroundPosition1.Y >= _graphics.PreferredBackBufferHeight)
                _backgroundPosition1.Y = _backgroundPosition2.Y - _graphics.PreferredBackBufferHeight;

            if (_backgroundPosition2.Y >= _graphics.PreferredBackBufferHeight)
                _backgroundPosition2.Y = _backgroundPosition1.Y - _graphics.PreferredBackBufferHeight;
        }

        private void SpawnEnemies(GameTime gameTime)
        {
            _enemySpawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_enemySpawnTimer >= 1.0)
            {
                Vector2 position = new Vector2(
                    Rand.Next(0, _graphics.PreferredBackBufferWidth - (int)(_enemyTexture.Width * 0.5f)), 0);
                Vector2 velocity = new Vector2(0, Rand.Next(100, 300));
                Enemy enemy = new Enemy(_enemyTexture, position, velocity) { Scale = 0.5f };
                _enemies.Add(enemy);
                _enemySpawnTimer = 0;
            }
        }

        private void CheckWinOrLossConditions()
        {
            if (_playerScore >= 10)
                EndGame("You Won!");

            if (_gameTimer >= 20)
                EndGame("Mission Over!");
        }

        private void EndGame(string message)
        {
            MediaPlayer.Stop();
            _isGameOver = true;
            _endGameMessage = message;
        }

        private void RestartGame()
        {
            _gameTimer = 0;
            _playerScore = 0;
            _isGameOver = false;
            _endGameMessage = "";
            _playerPosition = new Vector2(512, 700);
            _enemies.Clear();
            _bullets.Clear();
        }

        //Author : Zack Schottmann
        //This method renders all the visual elements of the game

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            // Draws the first background image in a normal oreientation.
            _spriteBatch.Draw(
                _backgroundTexture,
                _backgroundPosition1,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                new Vector2(
                    _graphics.PreferredBackBufferWidth / (float)_backgroundTexture.Width,
                    _graphics.PreferredBackBufferHeight / (float)_backgroundTexture.Height
                ),
                SpriteEffects.None,
                0f
            );

            // Draws the second background image flipped upside down
            _spriteBatch.Draw(
                _backgroundTexture,
                _backgroundPosition2,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                new Vector2(
                    _graphics.PreferredBackBufferWidth / (float)_backgroundTexture.Width,
                    _graphics.PreferredBackBufferHeight / (float)_backgroundTexture.Height
                ),
                SpriteEffects.FlipVertically, // used to flip upside down
                0f
            );

            _spriteBatch.Draw(
                _playerTexture,
                _playerPosition,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                _playerScale,
                SpriteEffects.None,
                0f
            );

            foreach (var bullet in _bullets) bullet.Draw(_spriteBatch);
            foreach (var enemy in _enemies) enemy.Draw(_spriteBatch);


            _spriteBatch.DrawString(_font, $"Time: {_gameTimer:F2}s", new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(_font, $"Score: {_playerScore}", new Vector2(10, 40), Color.White);


            if (_isGameOver)
            {
                var messageSize = _font.MeasureString(_endGameMessage);
                _spriteBatch.DrawString(_font, _endGameMessage,
                    new Vector2((_graphics.PreferredBackBufferWidth - messageSize.X) / 2,
                                (_graphics.PreferredBackBufferHeight - messageSize.Y) / 2), Color.Red);
                _spriteBatch.DrawString(_font, "Press R to Restart or ESC to Exit",
                    new Vector2(10, _graphics.PreferredBackBufferHeight - 40), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}