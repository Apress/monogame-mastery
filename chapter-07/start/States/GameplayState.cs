using chapter_07.Enum;
using chapter_07.Input;
using chapter_07.Input.Base;
using chapter_07.Objects;
using chapter_07.States.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace chapter_07.States
{
    public class GameplayState : BaseGameState
    {
        private const string BackgroundTexture = "Barren";
        private const string PlayerFighter = "fighter";
        private const string BulletTexture = "bullet";

        private PlayerSprite _playerSprite;
        private Texture2D _bulletTexture;
        private bool _isShooting;
        private TimeSpan _lastShotAt;

        private List<BulletSprite> _bulletList;

        public override void LoadContent()
        {
            _playerSprite = new PlayerSprite(LoadTexture(PlayerFighter));
            _bulletTexture = LoadTexture(BulletTexture);
            _bulletList = new List<BulletSprite>();

            AddGameObject(new TerrainBackground(LoadTexture(BackgroundTexture)));
            AddGameObject(_playerSprite);

            // position the player in the middle of the screen, at the bottom, leaving a slight gap at the bottom
            var playerXPos = _viewportWidth / 2 - _playerSprite.Width / 2;
            var playerYPos = _viewportHeight - _playerSprite.Height - 30;
            _playerSprite.Position = new Vector2(playerXPos, playerYPos);
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    NotifyEvent(Events.GAME_QUIT);
                }

                if (cmd is GameplayInputCommand.PlayerMoveLeft)
                {
                    _playerSprite.MoveLeft();
                    KeepPlayerInBounds();
                }

                if (cmd is GameplayInputCommand.PlayerMoveRight)
                {
                    _playerSprite.MoveRight();
                    KeepPlayerInBounds();
                }

                if (cmd is GameplayInputCommand.PlayerShoots)
                {
                    Shoot(gameTime);
                }
            });
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var bullet in _bulletList)
            {
                bullet.MoveUp();
            }

            // can't shoot more than every 0.2 seconds
            if (_lastShotAt != null && gameTime.TotalGameTime - _lastShotAt > TimeSpan.FromSeconds(0.2))
            {
                _isShooting = false;
            }

            // get rid of bullets that have gone out of view
            var newBulletList = new List<BulletSprite>();
            foreach (var bullet in _bulletList)
            {
                var bulletStillOnScreen = bullet.Position.Y > -30;

                if (bulletStillOnScreen)
                {
                    newBulletList.Add(bullet);
                }
                else
                {
                    RemoveGameObject(bullet);
                }
            }

            _bulletList = newBulletList;
        }

        private void Shoot(GameTime gameTime)
        {
            if (!_isShooting)
            {
                CreateBullets();
                _isShooting = true;
                _lastShotAt = gameTime.TotalGameTime;
            }
        }

        private void CreateBullets()
        {
            var bulletSpriteLeft = new BulletSprite(_bulletTexture);
            var bulletSpriteRight = new BulletSprite(_bulletTexture);

            var bulletY = _playerSprite.Position.Y + 30;
            var bulletLeftX = _playerSprite.Position.X + _playerSprite.Width / 2 - 40;
            var bulletRightX = _playerSprite.Position.X + _playerSprite.Width / 2 + 10;

            bulletSpriteLeft.Position = new Vector2(bulletLeftX, bulletY);
            bulletSpriteRight.Position = new Vector2(bulletRightX, bulletY);

            _bulletList.Add(bulletSpriteLeft);
            _bulletList.Add(bulletSpriteRight);

            AddGameObject(bulletSpriteLeft);
            AddGameObject(bulletSpriteRight);
        }

        private void KeepPlayerInBounds()
        {
            if (_playerSprite.Position.X < 0)
            {
                _playerSprite.Position = new Vector2(0, _playerSprite.Position.Y);
            }

            if (_playerSprite.Position.X > _viewportWidth - _playerSprite.Width)
            {
                _playerSprite.Position = new Vector2(_viewportWidth - _playerSprite.Width, _playerSprite.Position.Y);
            }

            if (_playerSprite.Position.Y < 0)
            {
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, 0);
            }

            if (_playerSprite.Position.Y > _viewportHeight - _playerSprite.Height)
            {
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, _viewportHeight - _playerSprite.Height);
            }
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }
    }
}