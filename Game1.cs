using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Numerics;
using System;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Linq;

namespace FInal
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _playerSprite;
        private Texture2D _alienSprite;
        private Texture2D _bulletSprite;
        private List<Player> _players = new List<Player>();
        
        private List<Bullet> _bullets = new List<Bullet>();
        private List<Alien> _aliens = new List<Alien>();
        private Texture2D _alienTexture;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load player sprite
            _playerSprite = Content.Load<Texture2D>("player");

            // Load alien sprite
            _alienSprite = Content.Load<Texture2D>("alien");

            // Load bullet sprite
            _bulletSprite = Content.Load<Texture2D>("bullet");

            _alienTexture = Content.Load<Texture2D>("alienSprite");

            // Add alien instances with varying positions and velocities.
            _aliens.Add(new Alien(_alienTexture, new Vector2(100, 50), new Vector2(60f, 0), _graphics.PreferredBackBufferWidth));
            _aliens.Add(new Alien(_alienTexture, new Vector2(300, 150), new Vector2(80f, 0), _graphics.PreferredBackBufferWidth));
            _aliens.Add(new Alien(_alienTexture, new Vector2(500, 100), new Vector2(70f, 0), _graphics.PreferredBackBufferWidth));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            HandlePlayerInput();
            UpdateAliens(gameTime);
            UpdateBullets();

            base.Update(gameTime);
        }

        private void HandlePlayerInput()
        {
            // Get the current player input
            PlayerIndex playerIndex = PlayerIndex.One;
            GamePadState gamePadState = GamePad.GetState(playerIndex);

            // Move the players
            foreach (var player in _players)
            {
                if (gamePadState.DPadLeft == ButtonState.Pressed)
                    player.Position += new Vector2(-10, 0);
                else if (gamePadState.DPadRight == ButtonState.Pressed)
                    player.Position += new Vector2(10, 0);

                // Shoot bullets
                if (gamePadState.Buttons.A == ButtonState.Pressed && !player.IsFiring)
                {
                    _bullets.Add(new Bullet(player.Position, player.Facing));
                    player.IsFiring = true;
                }
            }

            // Update player animation
            foreach (var player in _players)
            {
                if (player.Velocity != Vector2.Zero)
                    player.AnimationState = AnimationState.Moving;
                else
                    player.AnimationState = AnimationState.Idle;
            }
        }
        public void Gameover() {
            Exit();
        }
        private void UpdateAliens(GameTime gameTime)
        {
            // Move the aliens and handle firing logic
            foreach (var alien in _aliens)
            {
                if (!alien.IsAlive)
                    continue;

                // Move the alien horizontally
                alien.Position += alien.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Reverse direction at screen boundaries
                if (alien.Position.X <= 0 || alien.Position.X + alien.BoundingBox.Width >= _graphics.PreferredBackBufferWidth)
                {
                    alien.Velocity = new Vector2(-alien.Velocity.X, alien.Velocity.Y); // Reverse direction
                    alien.Position += alien.Velocity; // Adjust to avoid boundary clipping
                }

                // Handle alien shooting bullets
                if (Rand.NextDouble() < 0.01f) // Adjust probability as needed
                {
                    _bullets.Add(new Bullet(alien.Position + new Vector2(alien.BoundingBox.Width / 2, alien.BoundingBox.Height), new Vector2(0, 10)));
                }
            }

            // Check for collisions with the player
            foreach (var alien in _aliens)
            {
                if (alien.IsAlive && CollisionDetection.CheckCollision(_players[0].BoundingBox, alien.BoundingBox))
                {
                    Gameover(); // Trigger game-over logic
                }
            }
        }
        private void UpdateBullets()
        {
            // Move the bullets horizontally
            foreach (var bullet in _bullets)
            {
                bullet.Position += bullet.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Check if the bullet is out of bounds or has hit an alien
                if (!bullet.BoundingBox.Intersects(_players[0].BoundingBox))
                {
                    _bullets.Remove(bullet);
                }
                else if (_aliens.Any(alien => CollisionDetection.CheckCollision(bullet.BoundingBox, alien.BoundingBox)))
                {
                    Gameover();
                }
            }

            // Remove fired bullets
            foreach (var bullet in _bullets)
            {
                if (!bullet.IsMoving)
                    _bullets.Remove(bullet);
            }
        }

        private void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the player
            foreach (var player in _players)
            {
                SpriteEffects effect = new SpriteEffects();
                if (player.AnimationState == AnimationState.Moving)
                    effect = SpriteEffects.FlipHorizontally;

                _spriteBatch.Draw(_playerSprite, player.Position, null, Color.White, 0f, Vector2.Zero, effect);
            }

            // Draw the aliens
            foreach (var alien in _aliens)
            {
                _spriteBatch.Draw(_alienSprite, alien.Position, null, Color.White);
            }

            // Draw the bullets
            foreach (var bullet in _bullets)
            {
                _spriteBatch.Draw(_bulletSprite, bullet.Position, null, Color.White);
            }

            base.Draw(gameTime);
        }
    }
}