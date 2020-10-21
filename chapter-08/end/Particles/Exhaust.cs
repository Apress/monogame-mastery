using chapter_08.Engine.Particles;
using chapter_08.Engine.Particles.EmitterTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_08.States.Particles
{
    public class ExhaustParticleState : EmitterParticleState
    {
        public override int MinLifespan => 60; // equivalent to 1 second

        public override int MaxLifespan => 90;

        public override float Velocity => 4.0f;

        public override float VelocityDeviation => 1.0f;

        public override float Acceleration => 0.8f;

        public override Vector2 Gravity => new Vector2(0, 0);

        public override float Opacity => 0.4f;

        public override float OpacityDeviation => 0.1f;

        public override float OpacityFadingRate => 0.86f;

        public override float Rotation => 0.0f;

        public override float RotationDeviation => 0.0f;

        public override float Scale => 0.1f;

        public override float ScaleDeviation => 0.05f;
    }

    public class ExhaustEmitter : Emitter
    {
        private const int NbParticles = 10;
        private const int MaxParticles = 1000;
        private static Vector2 Direction = new Vector2(0.0f, 1.0f); // pointing downward
        private const float Spread = 1.5f;

        public ExhaustEmitter(Texture2D texture, Vector2 position) : 
            base(texture, position, new ExhaustParticleState(), new ConeEmitterType(Direction, Spread), NbParticles, MaxParticles) { }
    }
}
