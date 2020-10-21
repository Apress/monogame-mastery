using Microsoft.Xna.Framework;

namespace chapter_11.Engine.Particles.EmitterTypes
{
    public abstract class EmitterParticleState
    {
        private RandomNumberGenerator _rnd = new RandomNumberGenerator();

        // how long a particle can live
        public abstract int MinLifespan { get; }
        public abstract int MaxLifespan { get; }

        // defines how the particles will move. Velocity and its deviation define the particle's initial velocity
        // then each update, we
        //  - increase velocity by acceleration
        //  - increase direction by gravity
        //  - multiply direction by velocity
        public abstract float Velocity { get; }
        public abstract float VelocityDeviation { get; }
        public abstract float Acceleration { get; }
        public abstract Vector2 Gravity { get; } 
 
        public abstract float Opacity { get; }
        public abstract float OpacityDeviation { get; }
        public abstract float OpacityFadingRate { get; }

        public abstract float Rotation { get; }
        public abstract float RotationDeviation { get; }

        public abstract float Scale { get; }
        public abstract float ScaleDeviation { get; }

        public int GenerateLifespan()
        {
            return _rnd.NextRandom(MinLifespan, MaxLifespan);
        }

        public float GenerateVelocity()
        {
            return GenerateFloat(Velocity, VelocityDeviation);
        }

        public float GenerateOpacity()
        {
            return GenerateFloat(Opacity, OpacityDeviation);
        }

        public float GenerateRotation()
        {
            return GenerateFloat(Rotation, RotationDeviation);
        }

        public float GenerateScale()
        {
            return GenerateFloat(Scale, ScaleDeviation);
        }

        protected float GenerateFloat(float startN, float deviation)
        {
            var halfDeviation = deviation / 2.0f;
            return _rnd.NextRandom(startN - halfDeviation, startN + halfDeviation);
        }
    }
}
