using Microsoft.Xna.Framework;

namespace chapter_11.Engine.Objects
{
    public class Segment
    {
        public Vector2 P1 { get; private set; }
        public Vector2 P2 { get; private set; }

        public Segment(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }
}
