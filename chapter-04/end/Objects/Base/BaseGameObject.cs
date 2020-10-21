using chapter_04.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_04.Objects.Base
{
    public class BaseGameObject
    {
        protected Texture2D _texture;

        private Vector2 _position;

        public int zIndex;

        public virtual void OnNotify(Events eventType) { }

        public void Render(SpriteBatch spriteBatch)
        {
            // TODO: Drawing call here
            spriteBatch.Draw(_texture, Vector2.One, Color.White);
        }
    }
}