using System;

namespace chapter_09.Engine.Particles
{
    public class RandomNumberGenerator
    {
        private Random _rnd;

        public RandomNumberGenerator()
        {
            _rnd = new Random();
        }

        public int NextRandom() => _rnd.Next();
        public int NextRandom(int max) => _rnd.Next(max);
        public int NextRandom(int min, int max) => _rnd.Next(min, max);

        public float NextRandom(float max) => (float)_rnd.NextDouble() * max;
        public float NextRandom(float min, float max) => ((float)_rnd.NextDouble() * (max - min)) + min;

    }
}
