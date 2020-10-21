using chapter_10.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace chapter_10.States.Gameplay
{
    public class ChopperGenerator
    {
        private bool _generateLeft = true;
        private Vector2 _leftVector = new Vector2(-1, 0);
        private Vector2 _downLeftVector = new Vector2(-1, 1);
        private Vector2 _rightVector = new Vector2(1, 0);
        private Vector2 _downRightVector = new Vector2(1, 1);

        private Texture2D _texture;
        private System.Timers.Timer _timer;
        private Action<ChopperSprite> _chopperHandler;
        private int _maxChoppers = 0;
        private int _choppersGenerated = 0;
        private bool _generating = false;

        public ChopperGenerator(Texture2D texture, int nbChoppers, Action<ChopperSprite> handler)
        {
            _texture = texture;
            _chopperHandler = handler;

            _downLeftVector.Normalize();
            _downRightVector.Normalize();

            _maxChoppers = nbChoppers;

            _timer = new System.Timers.Timer(500);
            _timer.Elapsed += _timer_Elapsed;
        }

        public void GenerateChoppers()
        {
            if (_generating)
            {
                return;
            }

            _choppersGenerated = 0;
            _timer.Start();
        }

        public void StopGenerating()
        {
            _timer.Stop();
            _generating = false;
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<(int, Vector2)> path;
            if (_generateLeft)
            {
                path = new List<(int, Vector2)>
                {
                    (0, _rightVector),
                    (2 * 60, _downRightVector),
                };

                var chopper = new ChopperSprite(_texture, path);
                chopper.Position = new Vector2(-200, 100);
                _chopperHandler(chopper);
            }
            else
            {
                path = new List<(int, Vector2)>
                {
                    (0, _leftVector),
                    (2 * 60, _downLeftVector),
                };

                var chopper = new ChopperSprite(_texture, path);
                chopper.Position = new Vector2(1500, 100);
                _chopperHandler(chopper);
            }

            _generateLeft = !_generateLeft;

            _choppersGenerated++;
            if (_choppersGenerated == _maxChoppers)
            {
                StopGenerating();
            }
        }
    }
}
