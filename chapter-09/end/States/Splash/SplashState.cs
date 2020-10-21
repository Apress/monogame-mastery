using chapter_09.Engine.Input;
using chapter_09.Engine.States;
using chapter_09.Input;
using chapter_09.Objects;
using Microsoft.Xna.Framework;

namespace chapter_09.States
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

        public override void UpdateGameState(GameTime _) { }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new SplashInputMapper());
        }
    }
}