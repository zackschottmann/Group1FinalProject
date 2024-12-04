using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Numerics;
using System;

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
        private List<Alien> _aliens = new List<Alien>();
        private List<Bullet> _bullets = new List<Bullet>();

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            HandlePlayerInput();
            UpdateAliens();
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
        private void UpdateAliens()
        {
            // Move the aliens horizontally
            foreach (var alien in _aliens)
            {
                if (alien.Velocity.X != 0)
                    alien.Position += alien.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Shoot bullets
                if (Rand.NextDouble() < 0.1f && !alien.IsFiring)
                {
                    _bullets.Add(new Bullet(alien.Position, new Vector2(-10, 0)));
                    alien.IsFiring = true;
                }
            }

            // Check for collisions with the player
            foreach (var alien in _aliens)
            {
                if (CollisionDetection.CheckCollision(_players[0].BoundingBox, alien.BoundingBox))
                {
                    GameOver();
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
                    GameOver();
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