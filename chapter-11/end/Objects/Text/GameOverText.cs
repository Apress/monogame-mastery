using chapter_11.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_11.Objects.Text
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
