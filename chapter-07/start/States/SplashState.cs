using chapter_07.Enum;
using chapter_07.Input;
using chapter_07.Input.Base;
using chapter_07.Objects;
using chapter_07.States.Base;

using System.Collections.Generic;

namespace chapter_07.States
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("splash")));
        }

        public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is SplashInputCommand.GameSelect)
                {
                    SwitchState(new GameplayState());
                }
            });
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new SplashInputMapper());
        }
    }
}