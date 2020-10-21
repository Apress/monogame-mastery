using chapter_10.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_10.Objects.Text
{
    public class GameOverText : BaseTextObject
    {
        public GameOverText(SpriteFont font)
        {
            _font = font;
            Text = "Game Over";
        }
    }
}
