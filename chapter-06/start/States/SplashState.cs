using chapter_06.Objects;
using chapter_06.States.Base;

using Microsoft.Xna.Framework.Input;

namespace chapter_06.States
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture("splash")));
        }

        public override void HandleInput()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter))
            {
                SwitchState(new GameplayState());
            }
        }
    }
}