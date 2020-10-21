using chapter_04.Objects;
using chapter_04.States.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace chapter_04.States
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent(ContentManager contentManager)
        {
            // TODO: Add Content Loading

            AddGameObject(new SplashImage(contentManager.Load<Texture2D>("Barren")));
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
                SwitchState(new GameplayState());
            }
        }
    }
}