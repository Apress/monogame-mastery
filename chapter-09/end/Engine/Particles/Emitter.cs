using chapter_09.Engine.Objects;
using chapter_09.Engine.Particles.EmitterTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace chapter_09.Engine.Particles
{
    public class Emitter : BaseGameObject
    {
        private LinkedList<Particle> _activeParticles = new LinkedList<Particle>();
        private LinkedList<Particle> _inactiveParticles = new LinkedList<Particle>();
        private EmitterParticleState _emitterParticleState;
        private IEmitterType _emitterType;
        private int _nbParticleEmittedPerUpdate = 0;
        private int _maxNbParticle = 0;
        private bool _active = true;

        public int Age { get; set; }

        public Emitter(Texture2D texture, Vector2 position, EmitterParticleState particleState, IEmitterType emitterType, int nbParticleEmittedPerUpdate, int maxParticles)
        {
            _emitterParticleState = particleState;
            _emitterType = emitterType;
            _texture = texture;
            _nbParticleEmittedPerUpdate = nbParticleEmittedPerUpdate;
            _maxNbParticle = maxParticles;
            Position = position;
            Age = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (_active)
            {
                EmitParticles();
            }

            var particleNode = _activeParticles.First;
            while (particleNode != null)
            {
                var nextNode = particleNode.Next;
                var stillAlive = particleNode.Value.Update(gameTime);
                if (!stillAlive)
                {
                    _activeParticles.Remove(particleNode);
                    _inactiveParticles.AddLast(particleNode.Value);
                }

                particleNode = nextNode;
            }

            Age++;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);

            foreach (var particle in _activeParticles)
            {
                spriteBatch.Draw(_texture, particle.Position, sourceRectangle, Color.White * particle.Opacity, 0.0f, new Vector2(0, 0), particle.Scale, SpriteEffects.None, zIndex);
            }
        }
        public void Deactivate()
        {
            _active = false;
        }

        private void EmitParticles()
        {
            // make sure we're not at max particles
            if (_activeParticles.Count >= _maxNbParticle)
            {
                return;
            }

            var maxAmountThatCanBeCreated = _maxNbParticle - _activeParticles.Count;
            var neededParticles = Math.Min(maxAmountThatCanBeCreated, _nbParticleEmittedPerUpdate);

            // reuse inactive particles first before creating new ones
            var nbToReuse = Math.Min(_inactiveParticles.Count, neededParticles);
            var nbToCreate = neededParticles - nbToReuse;

            for(var i = 0; i < nbToReuse; i++)
            {
                var particleNode = _inactiveParticles.First;

                EmitNewParticle(particleNode.Value);
                _inactiveParticles.Remove(particleNode);
            }

            for(var i = 0; i < nbToCreate; i++)
            {
                EmitNewParticle(new Particle());
            }
        }

        private void EmitNewParticle(Particle particle)
        {
            var lifespan = _emitterParticleState.GenerateLifespan();
            var velocity = _emitterParticleState.GenerateVelocity();
            var scale = _emitterParticleState.GenerateScale();
            var rotation = _emitterParticleState.GenerateRotation();
            var opacity = _emitterParticleState.GenerateOpacity();
            var gravity = _emitterParticleState.Gravity;
            var acceleration = _emitterParticleState.Acceleration;
            var opacityFadingRate = _emitterParticleState.OpacityFadingRate;

            var direction = _emitterType.GetParticleDirection();
            var position = _emitterType.GetParticlePosition(_position);

            particle.Activate(lifespan, position, direction, gravity, velocity, acceleration, scale, rotation, opacity, opacityFadingRate);
            _activeParticles.AddLast(particle);
        }
    }
}
