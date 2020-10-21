using chapter_09.Engine.Input;

namespace chapter_09.Input
{
    public class DevInputCommand : BaseInputCommand 
    {
        // Out of Game Commands
        public class DevQuit : DevInputCommand { }
        public class DevExplode : DevInputCommand { }
        public class DevMissileExplode : DevInputCommand { }
        public class DevBulletSparks : DevInputCommand { }
    }
}
