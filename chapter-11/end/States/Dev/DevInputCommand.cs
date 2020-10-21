using chapter_11.Engine.Input;

namespace chapter_11.Input
{
    public class DevInputCommand : BaseInputCommand 
    {
        // Out of Game Commands
        public class DevQuit : DevInputCommand { }
        public class DevExplode : DevInputCommand { }
        public class DevMissileExplode : DevInputCommand { }
        public class DevBulletSparks : DevInputCommand { }
        public class DevLeft : DevInputCommand { }
        public class DevRight : DevInputCommand { }
        public class DevNotMoving : DevInputCommand { }
        public class DevShoot : DevInputCommand { }
    }
}
