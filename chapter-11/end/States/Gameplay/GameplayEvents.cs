using chapter_11.Engine.Objects;
using chapter_11.Engine.States;
using Microsoft.Xna.Framework;

namespace chapter_11.States.Gameplay
{
    public class GameplayEvents : BaseGameStateEvent
    {
        public class PlayerShootsBullets : GameplayEvents { }
        public class PlayerShootsMissile : GameplayEvents { }
        public class PlayerDies : GameplayEvents { }

        public class ObjectHitBy : GameplayEvents
        {
            public IGameObjectWithDamage HitBy { get; private set; }
            public ObjectHitBy(IGameObjectWithDamage gameObject)
            {
                HitBy = gameObject;
            }
        }

        public class ObjectLostLife : GameplayEvents
        {
            public int CurrentLife { get; private set; }
            public ObjectLostLife(int currentLife)
            {
                CurrentLife = currentLife;
            }
        }

        public class TurretShoots : GameplayEvents 
        { 
            public Vector2 Direction { get; private set; }
            public Vector2 Bullet1Position { get; private set; }
            public Vector2 Bullet2Position { get; private set; }
            public float Angle { get; private set; }

            public TurretShoots(Vector2 bullet1Pos, Vector2 bullet2Pos, float angle, Vector2 direction)
            {
                Direction = direction;
                Bullet1Position = bullet1Pos;
                Bullet2Position = bullet2Pos;
                Angle = angle;
            }
        }
    }
}
