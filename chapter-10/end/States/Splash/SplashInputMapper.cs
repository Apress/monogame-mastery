using chapter_10.Engine.Input;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace chapter_10.Input
{
    public class SplashInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<SplashInputCommand>();

            if (state.IsKeyDown(Keys.Enter))
            {
                commands.Add(new SplashInputCommand.GameSelect());
            }

            return commands;
        }
    }
}
