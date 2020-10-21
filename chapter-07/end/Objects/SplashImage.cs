using chapter_07.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_07.Objects
{
    public class SplashImage : BaseGameObject
    {
        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }
    }
}