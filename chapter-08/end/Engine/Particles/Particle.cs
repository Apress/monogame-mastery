using Microsoft.Xna.Framework;
using System;

namespace chapter_08.Engine.Particles
{
    public class Particle
    {
        public Vector2 Position { get; private set; }
        public float Scale { get; private set; }
        public float Opacity { get; private set; }

        private int _lifespan; // will tick up every update and monogame updates 60 times per second
        private int _age;
        private Vector2 _direction;
        private Vector2 _gravity;
        private float _velocity;
        private float _acceleration;
        private float _rotation;
        private float _opacityFadingRate;

        public Particle() { }

        public void Activate(int lifespan, Vector2 position, Vector2 direction, Vector2 gravity,
                             float velocity, float acceleration,
                             float scale, float rotation, float opacity, float opacityFadingRate)
        {
            _lifespan = lifespan;
            _direction = direction;
            _velocity = velocity;
            _gravity = gravity;
            _acceleration = acceleration;
            _rotation = rotation;
            _opacityFadingRate = opacityFadingRate;
            _age = 0;

            Position = position;
            Opacity = opacity;
            Scale = scale;
        }
 
        // returns false if it went past its lifespan
        public bool Update(GameTime gameTime)
        {
            // TODO: update rotation and scale
            _velocity *= _acceleration;
            _direction += _gravity;
            
            var positionDelta = _direction * _velocity;

            Position += positionDelta;

            Opacity *= _opacityFadingRate;

            // return true if particle can stay alive
            _age++;
            return _age < _lifespan;
        }
    }
}
