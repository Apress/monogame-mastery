using Microsoft.Xna.Framework;

namespace chapter_10.Engine.Particles.EmitterTypes
{
    public interface IEmitterType
    {
        Vector2 GetParticleDirection();
        Vector2 GetParticlePosition(Vector2 emitterPosition);
    }
}
