using chapter_09.Engine.Input;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace chapter_09.Input
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

            if (state.IsKeyDown(Keys.Left))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveLeft());
            }

            if (state.IsKeyDown(Keys.Right))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveRight());
            }

            if (state.IsKeyDown(Keys.Space))
            {
                commands.Add(new GameplayInputCommand.PlayerShoots());
            }

            return commands;
        }
    }
}
