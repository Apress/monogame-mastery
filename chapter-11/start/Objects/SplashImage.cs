using chapter_11.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_11.Objects
{
    public class SplashImage : BaseGameObject
    {
        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }
    }
}