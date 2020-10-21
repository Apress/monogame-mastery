using chapter_11.Engine.Input;

namespace chapter_11.Input
{
    public class GameplayInputCommand : BaseInputCommand 
    { 
        public class GameExit : GameplayInputCommand { }
        public class PlayerMoveLeft : GameplayInputCommand { }
        public class PlayerMoveRight : GameplayInputCommand { }
        public class PlayerMoveUp : GameplayInputCommand { }
        public class PlayerMoveDown : GameplayInputCommand { }
        public class PlayerStopsMoving : GameplayInputCommand { }
        public class PlayerShoots : GameplayInputCommand { }
    }
}
