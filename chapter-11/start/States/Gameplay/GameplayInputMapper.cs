using chapter_11.Engine.Input;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace chapter_11.Input
{
    public class GameplayInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<GameplayInputCommand>();

            if (state.IsKeyDown(Keys.Escape))
            {
                commands.Add(new GameplayInputCommand.GameExit());
            }

            if (state.IsKeyDown(Keys.Space))
            {
                commands.Add(new GameplayInputCommand.PlayerShoots());
            }

            if (state.IsKeyDown(Keys.Right))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveRight());
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveLeft());
            }
            else
            {
                commands.Add(new GameplayInputCommand.PlayerStopsMoving());
            }

            return commands;
        }
    }
}
