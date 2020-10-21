using chapter_10.Engine.Input;

namespace chapter_10.Input
{
    public class GameplayInputCommand : BaseInputCommand 
    { 
        public class GameExit : GameplayInputCommand { }
        public class PlayerMoveLeft : GameplayInputCommand { }
        public class PlayerMoveRight : GameplayInputCommand { }
        public class PlayerStopsMoving : GameplayInputCommand { }
        public class PlayerShoots : GameplayInputCommand { }
    }
}
