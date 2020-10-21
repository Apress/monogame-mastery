using chapter_06.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_06.Objects.Base
{
    public class BaseGameObject
    {
        protected Texture2D _texture;
        protected Vector2 _position = Vector2.One;

        public int zIndex;

        public int Width { get { return _texture.Width; } }
        public int Height { get { return _texture.Height; } }
        public Vector2 Position 
        { 
            get { return _position; } 
            set { _position = value; } 
        }

        public virtual void OnNotify(Events eventType, object argument = null) { }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}