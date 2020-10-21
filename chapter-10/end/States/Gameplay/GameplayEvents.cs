using chapter_10.Engine.Objects;
using chapter_10.Engine.States;

namespace chapter_10.States.Gameplay
{
    public class GameplayEvents : BaseGameStateEvent
    {
        public class PlayerShootsBullets : GameplayEvents { }
        public class PlayerShootsMissile : GameplayEvents { }
        public class PlayerDies : GameplayEvents { }

        public class ChopperHitBy : GameplayEvents
        {
            public IGameObjectWithDamage HitBy { get; private set; }
            public ChopperHitBy(IGameObjectWithDamage gameObject)
            {
                HitBy = gameObject;
            }
        }

        public class EnemyLostLife : GameplayEvents
        {
            public int CurrentLife { get; private set; }
            public EnemyLostLife(int currentLife)
            {
                CurrentLife = currentLife;
            }
        }
    }
}
