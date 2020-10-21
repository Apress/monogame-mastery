using Microsoft.Xna.Framework.Input;
using System;

namespace chapter_07.Engine.Input
{
    public class InputManager
    {
        private readonly BaseInputMapper _inputMapper;

        public InputManager(BaseInputMapper inputMapper)
        {
            _inputMapper = inputMapper;
        }

        public void GetCommands(Action<BaseInputCommand> actOnState)
        {
            var keyboardState = Keyboard.GetState();
            foreach (var state in _inputMapper.GetKeyboardState(keyboardState))
            {
                actOnState(state);
            }

            var mouseState = Mouse.GetState();
            foreach (var state in _inputMapper.GetMouseState(mouseState))
            {
                actOnState(state);
            }

            // we're going to assume only 1 gamepad is being used
            var gamePadState = GamePad.GetState(0);
            foreach (var state in _inputMapper.GetGamePadState(gamePadState))
            {
                actOnState(state);
            }
        }
    }
}
