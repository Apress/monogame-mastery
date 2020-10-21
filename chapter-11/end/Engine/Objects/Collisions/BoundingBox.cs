using Microsoft.Xna.Framework;

namespace chapter_11.Engine.Objects
{
    public class BoundingBox
    {
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
            }
        }

        public BoundingBox(Vector2 position, float width, float height)
        {
            Position = position;
            Width = width;
            Height = height;
        }

        public bool CollidesWith(BoundingBox otherBB)
        {
            if (Position.X < otherBB.Position.X + otherBB.Width &&
                Position.X + Width > otherBB.Position.X &&
                Position.Y < otherBB.Position.Y + otherBB.Height &&
                Position.Y + Height > otherBB.Position.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CollidesWith(Segment segment)
        {
            if (CollidesWith(segment.P1) || CollidesWith(segment.P2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CollidesWith(Vector2 p)
        {
            if (p.X < Position.X + Width &&
                p.X > Position.X &&
                p.Y < Position.Y + Height &&
                p.Y > Position.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
