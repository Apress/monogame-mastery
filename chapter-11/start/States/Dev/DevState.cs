using chapter_11.Engine.Input;
using chapter_11.Engine.States;
using chapter_11.Input;
using chapter_11.Objects;
using chapter_11.Objects.Text;
using chapter_11.States.Particles;
using Microsoft.Xna.Framework;
using System;

namespace chapter_11.States
{
    /// <summary>
    /// Used to test out new things, like particle engines and shooting missiles
    /// </summary>
    public class DevState : BaseGameState
    {
        private const string FighterSpriteSheet = "Sprites/Animations/FighterSpriteSheet";
        private PlayerSprite _player;

        public override void LoadContent()
        {
            _player = new PlayerSprite(LoadTexture(FighterSpriteSheet));
            _player.Position = new Vector2(200, 400);
            AddGameObject(_player);

            var font = LoadFont("Fonts/GameOver");
            var gameOverText = new GameOverText(font);
            var textPosition = new Vector2(460, 300);

            gameOverText.Position = textPosition;
            AddGameObject(gameOverText);
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is DevInputCommand.DevQuit)
                {
                    NotifyEvent(new BaseGameStateEvent.GameQuit());
                }

                if (cmd is DevInputCommand.DevLeft)
                {
                    _player.MoveLeft();
                }

                if (cmd is DevInputCommand.DevRight)
                {
                    _player.MoveRight();
                }

                if (cmd is DevInputCommand.DevNotMoving)
                {
                    _player.StopMoving();
                }
            });
        }

        public override void UpdateGameState(GameTime gameTime) 
        {
            _player.Update(gameTime);
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new DevInputMapper());
        }
    }
}