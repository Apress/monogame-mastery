using chapter_09.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_09.Objects
{
    public class PlayerSprite : BaseGameObject
    {
        private const float PlayerSpeed = 10.0f;

        private const int BB1PosX = 29;
        private const int BB1PosY = 2;
        private const int BB1Width = 57;
        private const int BB1Height = 147;

        private const int BB2PosX = 2;
        private const int BB2PosY = 77;
        private const int BB2Width = 111;
        private const int BB2Height = 37;

        public PlayerSprite(Texture2D texture)
        {
            _texture = texture;
            AddBoundingBox(new Engine.Objects.BoundingBox(new Vector2(BB1PosX, BB1PosY), BB1Width, BB1Height));
            AddBoundingBox(new Engine.Objects.BoundingBox(new Vector2(BB2PosX, BB2PosY), BB2Width, BB2Height));
        }

        public void MoveLeft()
        {
            Position = new Vector2(Position.X - PlayerSpeed, Position.Y);
        }

        public void MoveRight()
        {
            Position = new Vector2(Position.X + PlayerSpeed, Position.Y);
        }
    }
}