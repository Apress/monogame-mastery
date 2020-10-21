namespace chapter_11.Engine.States
{
    public class BaseGameStateEvent 
    {
        public class Nothing : BaseGameStateEvent { }
        public class GameQuit : BaseGameStateEvent { }
        public class GameTick : BaseGameStateEvent { }
    }
}
