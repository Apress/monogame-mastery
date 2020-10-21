using chapter_07.Engine.Input;

namespace chapter_07.Input
{
    public class GameplayInputCommand : BaseInputCommand 
    { 
        public class GameExit : GameplayInputCommand { }
        public class PlayerMoveLeft : GameplayInputCommand { }
        public class PlayerMoveRight : GameplayInputCommand { }
        public class PlayerShoots : GameplayInputCommand { }
    }
}
