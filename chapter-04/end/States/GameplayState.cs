using chapter_04.Enum;
using chapter_04.States.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace chapter_04.States
{
    public class GameplayState : BaseGameState
    {
        public override void LoadContent(ContentManager contentManager)
        {
            
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            contentManager.Unload();
        }

        public override void HandleInput()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                NotifyEvent(Events.GAME_QUIT);
            }
        }
    }
}