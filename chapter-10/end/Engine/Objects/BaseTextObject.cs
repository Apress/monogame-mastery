using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_10.Engine.Objects
{
    public class BaseTextObject : BaseGameObject
    {
        protected SpriteFont _font;

        public string Text { get; set; }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Text, _position, Color.White);
        }
    }
}
