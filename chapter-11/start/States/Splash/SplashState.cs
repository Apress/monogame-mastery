using chapter_11.Engine.Input;
using chapter_11.Engine.States;
using chapter_11.Input;
using chapter_11.Objects;
using Microsoft.Xna.Framework;

namespace chapter_11.States
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("Images/splash")));
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