using chapter_11.Engine.Objects;
using chapter_11.Engine.Objects.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace chapter_11.Objects
{
    public class PlayerSprite : BaseGameObject
    {
        private const float PlayerHorizontalSpeed = 10.0f;
        private const float PlayerVerticalSpeed = 8.0f;

        private const int BB1PosX = 29;
        private const int BB1PosY = 2;
        private const int BB1Width = 57;
        private const int BB1Height = 147;

        private const int BB2PosX = 2;
        private const int BB2PosY = 77;
        private const int BB2Width = 111;
        private const int BB2Height = 37;

        private Animation _turnLeftAnimation = new Animation(false);
        private Animation _turnRightAnimation = new Animation(false);
        private Animation _leftToCenterAnimation = new Animation(false);
        private Animation _rightToCenterAnimation = new Animation(false);
        private const int AnimationSpeed = 3;
        private const int AnimationCellWidth = 116;
        private const int AnimationCellHeight= 152;

        private Animation _currentAnimation;
        private Rectangle _idleRectangle;

        private bool _movingLeft = false;
        private bool _movingRight = false;

        public override int Height => AnimationCellHeight;
        public override int Width => AnimationCellWidth;

        public Vector2 CenterPosition
        {
            get
            {
                return Vector2.Add(_position, new Vector2(AnimationCellWidth / 2, AnimationCellHeight / 2));
            }
        }

        public PlayerSprite(Texture2D texture)
        {
            _texture = texture;
            AddBoundingBox(new Engine.Objects.BoundingBox(new Vector2(BB1PosX, BB1PosY), BB1Width, BB1Height));
            AddBoundingBox(new Engine.Objects.BoundingBox(new Vector2(BB2PosX, BB2PosY), BB2Width, BB2Height));

            _idleRectangle            = new Rectangle(348, 0, AnimationCellWidth, AnimationCellHeight);
            _turnLeftAnimation.AddFrame(new Rectangle(348, 0, AnimationCellWidth, AnimationCellHeight), AnimationSpeed);
            _turnLeftAnimation.AddFrame(new Rectangle(232, 0, AnimationCellWidth, AnimationCellHeight), AnimationSpeed);
            _turnLeftAnimation.AddFrame(new Rectangle(116, 0, AnimationCellWidth, AnimationCellHeight), AnimationSpeed);
            _turnLeftAnimation.AddFrame(new Rectangle(0,   0, AnimationCellWidth, AnimationCellHeight), AnimationSpeed);

            _turnRightAnimation.AddFrame(new Rectangle(348, 0, AnimationCellWidth, AnimationCellHeight), AnimationSpeed);
            _turnRightAnimation.AddFrame(new Rectangle(464, 0, AnimationCellWidth, AnimationCellHeight), AnimationSpeed);
            _turnRightAnimation.AddFrame(new Rectangle(580, 0, AnimationCellWidth, AnimationCellHeight), AnimationSpeed);
            _turnRightAnimation.AddFrame(new Rectangle(696, 0, AnimationCellWidth, AnimationCellHeight), AnimationSpeed);

            _leftToCenterAnimation = _turnLeftAnimation.ReverseAnimation;
            _rightToCenterAnimation = _turnRightAnimation.ReverseAnimation;
        }

        public void StopMoving()
        {
            if (_movingLeft)
            {
                _currentAnimation = _leftToCenterAnimation;
                _movingLeft = false;
            }

            if (_movingRight)
            {
                _currentAnimation = _rightToCenterAnimation;
                _movingRight = false;
            }
        }

        public void MoveLeft()
        {
            _movingLeft = true;
            _movingRight = false;
            _currentAnimation = _turnLeftAnimation;
            _leftToCenterAnimation.Reset();
            _turnRightAnimation.Reset();
            Position = new Vector2(Position.X - PlayerHorizontalSpeed, Position.Y);
        }

        public void MoveRight()
        {
            _movingRight = true;
            _movingLeft = false;
            _currentAnimation = _turnRightAnimation;
            _rightToCenterAnimation.Reset();
            _turnLeftAnimation.Reset();
            Position = new Vector2(Position.X + PlayerHorizontalSpeed, Position.Y);
        }

        public void MoveUp()
        {
            Position = new Vector2(Position.X, Position.Y - PlayerVerticalSpeed);
        }

        public void MoveDown()
        {
            Position = new Vector2(Position.X, Position.Y + PlayerVerticalSpeed);
        }
        
        public void Update(GameTime gametime)
        {
            if (_currentAnimation != null)
            {
                _currentAnimation.Update(gametime);
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var destinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, AnimationCellWidth, AnimationCellHeight);
            var sourceRectangle = _idleRectangle;
            if (_currentAnimation != null)
            {
                var currentFrame = _currentAnimation.CurrentFrame;
                if (currentFrame != null)
                {
                    sourceRectangle = currentFrame.SourceRectangle;
                }
            }

            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}