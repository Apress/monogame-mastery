using chapter_08.Engine.Objects;
using chapter_08.States.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_08.Objects
{
    public class MissileSprite : BaseGameObject
    {
        private const float StartSpeed = 0.5f;
        private const float Acceleration = 0.15f;

        private float _speed = StartSpeed;

        // keep track of scaled down texture size
        private int _missileHeight;
        private int _missileWidth;

        // missiles are attached to their own particle emitter
        private ExhaustEmitter _exhaustEmitter;

        public override Vector2 Position 
        { 
            set 
            { 
                _position = value;
                _exhaustEmitter.Position = new Vector2(_position.X + 18, _position.Y + _missileHeight - 10);
            }
        }

        public MissileSprite(Texture2D missleTexture, Texture2D exhaustTexture)
        {
            _texture = missleTexture;
            _exhaustEmitter = new ExhaustEmitter(exhaustTexture, _position);

            var ratio = (float) _texture.Height / (float) _texture.Width;
            _missileWidth = 50;
            _missileHeight = (int) (_missileWidth * ratio);
        }

        public void Update(GameTime gameTime)
        {
            _exhaustEmitter.Update(gameTime);

            Position = new Vector2(Position.X, Position.Y - _speed);
            _speed = _speed + Acceleration;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            // need to scale down the sprite. The original texture is very big
            var destRectangle = new Rectangle((int) Position.X, (int) Position.Y, _missileWidth, _missileHeight);
            spriteBatch.Draw(_texture, destRectangle, Color.White);

            _exhaustEmitter.Render(spriteBatch);
        }
    }
}
