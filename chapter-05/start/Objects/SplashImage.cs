using chapter_05.Objects.Base;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_05.Objects
{
    public class SplashImage : BaseGameObject
    {
        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }
    }
}