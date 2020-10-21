using chapter_05.Enum;
using chapter_05.Objects;
using chapter_05.States.Base;

using Microsoft.Xna.Framework.Input;

namespace chapter_05.States
{
    public class GameplayState : BaseGameState
    {
        private const string PlayerFighter = "fighter";

        private const string BackgroundTexture = "Barren";

        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture(BackgroundTexture)));
            AddGameObject(new PlayerSprite(LoadTexture(PlayerFighter)));
        }

        public override void HandleInput()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
            {
                NotifyEvent(Events.GAME_QUIT);
            }
        }
    }
}