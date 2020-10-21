using Microsoft.Xna.Framework;
using System;

namespace chapter_11.Engine.Particles.EmitterTypes
{
    public class ConeEmitterType : IEmitterType
    {
        public Vector2 Direction { get; private set; }
        public float Spread { get; private set; }

        private RandomNumberGenerator _rnd = new RandomNumberGenerator();

        public ConeEmitterType(Vector2 direction, float spread)
        {
            Direction = direction;
            Spread = spread;
        }

        public Vector2 GetParticleDirection()
        {
            if (Direction == null)
            {
                return new Vector2(0, 0);
            }

            var angle = (float) Math.Atan2(Direction.Y, Direction.X);
            var newAngle = _rnd.NextRandom(angle - Spread / 2.0f, angle + Spread / 2.0f);

            var particleDirection = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
            particleDirection.Normalize();
            return particleDirection;
        }

        public Vector2 GetParticlePosition(Vector2 emitterPosition)
        {
            // return the same position for this type of emitter, but otherwise we could tweak this to start particles a bit further
            // away from the center of the cone.

            var x = emitterPosition.X;
            var y = emitterPosition.Y;

            return new Vector2(x, y);
        }
    }
}
