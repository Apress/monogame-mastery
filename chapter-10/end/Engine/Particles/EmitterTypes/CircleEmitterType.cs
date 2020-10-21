using Microsoft.Xna.Framework;
using System;

namespace chapter_10.Engine.Particles.EmitterTypes
{
    public class CircleEmitterType : IEmitterType
    {
        public float Radius { get; private set; }

        private RandomNumberGenerator _rnd = new RandomNumberGenerator();

        public CircleEmitterType(float radius)
        {
            Radius = radius;
        }

        public Vector2 GetParticleDirection()
        {
            return new Vector2(0f, 0f);
        }

        public Vector2 GetParticlePosition(Vector2 emitterPosition)
        {
            var newAngle = _rnd.NextRandom(0, 2 * MathHelper.Pi);
            var positionVector = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
            positionVector.Normalize();

            var distance = _rnd.NextRandom(0, Radius);
            var position = positionVector * distance;

            var x = emitterPosition.X + position.X;
            var y = emitterPosition.Y + position.Y;

            return new Vector2(x, y);
        }
    }
}
