using chapter_09.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_09.Objects
{
    public class SplashImage : BaseGameObject
    {
        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }
    }
}