using chapter_11.Engine.Input;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace chapter_11.Input
{
    public class DevInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<DevInputCommand>();

            if (state.IsKeyDown(Keys.Escape))
            {
                commands.Add(new DevInputCommand.DevQuit());
            }

            if (state.IsKeyDown(Keys.Z))
            {
                commands.Add(new DevInputCommand.DevBulletSparks());
            }

            if (state.IsKeyDown(Keys.X))
            {
                commands.Add(new DevInputCommand.DevMissileExplode());
            }

            if (state.IsKeyDown(Keys.C))
            {
                commands.Add(new DevInputCommand.DevExplode());
            }

            if (state.IsKeyDown(Keys.Right))
            {
                commands.Add(new DevInputCommand.DevRight());
            }

            if (state.IsKeyDown(Keys.Left))
            {
                commands.Add(new DevInputCommand.DevLeft());
            }

            if (state.IsKeyDown(Keys.Space))
            {
                commands.Add(new DevInputCommand.DevShoot());
            }


            return commands;
        }
    }
}
