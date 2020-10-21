using chapter_11.Engine.Objects;
using chapter_11.Engine.States;
using chapter_11.States.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace chapter_11.Objects
{
    public class TurretSprite : BaseGameObject
    {
        private Texture2D _baseTexture;
        private Texture2D _cannonTexture;

        private float _moveSpeed;

        // with an angle of zero, the turret points up, so track offset for calculations when tracking player
        private const float AngleOffset = MathHelper.Pi / 2;
        private const float Scale = 0.3f;
        private const float AngleSpeed = 0.02f;
        private const int BulletsPerShot = 3;
        private const float CannonCenterPosY = 158;

        private int _hitAt = 100;
        private int _life = 50;

        private Vector2 _baseCenterPosition;
        private Vector2 _cannonCenterPosition;
        private float _baseTextureWidth;
        private float _baseTextureHeight;
        private bool _isShootingBullets;
        private TimeSpan _lastBulletShotAt;
        private int _bulletsRemaining;
        private bool _attackMode;

        public bool Active { get; set; }

        public event EventHandler<GameplayEvents.TurretShoots> OnTurretShoots;

        public TurretSprite(Texture2D baseTexture, Texture2D cannonTexture, float moveSpeed)
        {
            _isShootingBullets = false;
            _moveSpeed = moveSpeed;
            _baseTexture = baseTexture;
            _cannonTexture = cannonTexture;
            _angle = MathHelper.Pi;  // point down by default
            _bulletsRemaining = BulletsPerShot;
            _attackMode = false;
            Active = false;

            _direction = CalculateDirection(AngleOffset);

            _baseTextureWidth = _baseTexture.Width * Scale;
            _baseTextureHeight = _baseTexture.Height * Scale;

            _baseCenterPosition = new Vector2(_baseTextureWidth / 2f, _baseTextureHeight / 2f);
            _cannonCenterPosition = new Vector2(_cannonTexture.Width / 2f, CannonCenterPosY);

            AddBoundingBox(new Engine.Objects.BoundingBox(new Vector2(0, 0), _baseTexture.Width * Scale, _baseTexture.Height * Scale));
        }
        
        public void Update(GameTime gameTime, Vector2 currentPlayerCenter)
        {
            // move turret down
            Position = Vector2.Add(_position, new Vector2(0, _moveSpeed));

            // if turret is not active, it cannot spin or shoot
            if (!Active)
            {
                return;
            }

            // can either attack and shoot 3 bullets or move. Not both
            if (_attackMode && _bulletsRemaining > 0)
            {
                Shoot(gameTime);
            }
            else
            {
                // compare angle between turretDirection and vector from center of cannon to center of player
                var centerOfCannon = Vector2.Add(_position, _cannonCenterPosition * Scale);
                var playerVector = Vector2.Subtract(currentPlayerCenter, centerOfCannon);
                playerVector.Normalize();

                var angleTurret = Math.Atan2(_direction.Y, _direction.X);
                var anglePlayer = Math.Atan2(playerVector.Y, playerVector.X);
                var angleDiff = angleTurret - anglePlayer;

                var tolerance = 0.1f;

                if (angleDiff > tolerance)
                {
                    MoveLeft();
                }
                else if (angleDiff < -tolerance)
                {
                    MoveRight();
                }

                if (angleTurret >= anglePlayer - tolerance && angleTurret <= anglePlayer + tolerance)
                {
                    _attackMode = true;
                    Shoot(gameTime);
                }
            }

            if (_bulletsRemaining <= 0)
            {
                _attackMode = false;
            }

            // prevent firing bullets too quickly
            if (_lastBulletShotAt != null && gameTime.TotalGameTime - _lastBulletShotAt > TimeSpan.FromSeconds(0.3))
            {
                _isShootingBullets = false;
            }

            // reload bullets every 2 seconds
            if (gameTime.TotalGameTime - _lastBulletShotAt > TimeSpan.FromSeconds(2))
            {
                _bulletsRemaining = BulletsPerShot;
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            // if the turret was just hit and is flashing, Color should alternate between OrangeRed and White
            var color = GetColor();

            var cannonPosX = _position.X + _baseCenterPosition.X;
            var cannonPosY = _position.Y + _baseCenterPosition.Y;
            var cannonPosition = new Vector2(cannonPosX, cannonPosY);

            spriteBatch.Draw(_baseTexture, _position, _baseTexture.Bounds, color, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(_cannonTexture, cannonPosition, _cannonTexture.Bounds, Color.White, _angle, _cannonCenterPosition, Scale, SpriteEffects.None, 0f);
        }

        public void MoveLeft()
        {
            _angle -= AngleSpeed;
            _direction = CalculateDirection(AngleOffset);
        }

        public void MoveRight()
        {
            _angle += AngleSpeed;
            _direction = CalculateDirection(AngleOffset);
        }

        public void Shoot(GameTime gameTime)
        {
            if (!_isShootingBullets && _bulletsRemaining > 0)
            {
                var centerOfCannon = Vector2.Add(_position, _baseCenterPosition);

                // find perpendicular vectors to position bullets left and right of the center of the cannon
                var perpendicularClockwiseDirection = new Vector2(_direction.Y, -_direction.X);
                var perpendicularCounterClockwiseDirection = new Vector2(-_direction.Y, _direction.X);

                var bullet1Pos = Vector2.Add(centerOfCannon, perpendicularClockwiseDirection * 10);
                var bullet2Pos = Vector2.Add(centerOfCannon, perpendicularCounterClockwiseDirection * 10);

                var bulletInfo = new GameplayEvents.TurretShoots(bullet1Pos, bullet2Pos, _angle, _direction);

                _bulletsRemaining--;
                _isShootingBullets = true;
                _lastBulletShotAt = gameTime.TotalGameTime;

                OnTurretShoots?.Invoke(this, bulletInfo);
            }
        }

        public override void OnNotify(BaseGameStateEvent gameEvent)
        {
            switch (gameEvent)
            {
                case GameplayEvents.ObjectHitBy m:
                    JustHit(m.HitBy);
                    SendEvent(new GameplayEvents.ObjectLostLife(_life));
                    break;
            }
        }

        private void JustHit(IGameObjectWithDamage o)
        {
            _hitAt = 0;
            _life -= o.Damage;
        }

        private Color GetColor()
        {
            var color = Color.White;
            foreach (var flashStartEndFrames in GetFlashStartEndFrames())
            {
                if (_hitAt >= flashStartEndFrames.Item1 && _hitAt < flashStartEndFrames.Item2)
                {
                    color = Color.OrangeRed;
                }    
            }

            _hitAt++;
            return color;
        }

        private List<(int, int)> GetFlashStartEndFrames()
        {
            return new List<(int, int)>
            {
                (0, 3),
                (10, 13)
            };
        }
    }
}
