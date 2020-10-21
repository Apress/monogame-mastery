using chapter_04.Objects.Base;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_04.Objects
{
    public class SplashImage : BaseGameObject
    {
        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }
    }
}