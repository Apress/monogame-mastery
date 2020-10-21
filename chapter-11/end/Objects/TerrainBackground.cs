using chapter_11.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_11.Objects
{
    public class TerrainBackground : BaseGameObject
    {
        private float _scrolling_speed;

        public TerrainBackground(Texture2D texture, float scrollingSpeed)
        {
            _texture = texture;
            _position = new Vector2(0, 0);
            _scrolling_speed = scrollingSpeed;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var viewport = spriteBatch.GraphicsDevice.Viewport;

            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);

            // tile the textures to fill the screen. Add an extra row of tiles above the viewport so they can scroll into view and not leave a gap
            for (int nbVertical = -1; nbVertical < viewport.Height / _texture.Height + 1; nbVertical++)
            {
                var y = (int) _position.Y + nbVertical * _texture.Height;
                for (int nbHorizontal = 0; nbHorizontal < viewport.Width / _texture.Width + 1; nbHorizontal++)
                {
                    var x = (int) _position.X + nbHorizontal * _texture.Width;
                    var destinationRectangle = new Rectangle(x, y, _texture.Width, _texture.Height);
                    spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }

            _position.Y = (int)(_position.Y + _scrolling_speed) % _texture.Height;
        }
    }
}
