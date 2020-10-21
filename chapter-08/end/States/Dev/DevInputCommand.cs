using chapter_08.Engine.Input;

namespace chapter_08.Input
{
    public class DevInputCommand : BaseInputCommand 
    {
        // Out of Game Commands
        public class DevQuit : DevInputCommand { }
        public class DevShoot : DevInputCommand { }
    }
}
