using chapter_10.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_10.Objects.Text
{
    public class LivesText : BaseTextObject
    {
        private int _nbLives = -1;

        public int NbLives {
            get
            {
                return _nbLives;
            }
            set
            {
                _nbLives = value;
                Text = $"Lives: {_nbLives}";
            }
        }

        public LivesText(SpriteFont font)
        {
            _font = font;
        }
    }
}
