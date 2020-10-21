using System;
using System.Collections.Generic;
using System.Linq;

using chapter_04.Enum;
using chapter_04.Objects.Base;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_04.States.Base
{
    public abstract class BaseGameState
    {
        private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();

        public abstract void LoadContent(ContentManager contentManager);

        public abstract void UnloadContent(ContentManager contentManager);

        public abstract void HandleInput();

        public event EventHandler<BaseGameState> OnStateSwitched;

        public event EventHandler<Events> OnEventNotification;

        protected void NotifyEvent(Events eventType, object argument = null)
        {
            OnEventNotification?.Invoke(this, eventType);

            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnNotify(eventType);
            }
        }

        protected void SwitchState(BaseGameState gameState)
        {
            OnStateSwitched?.Invoke(this, gameState);
        }

        protected void AddGameObject(BaseGameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in _gameObjects.OrderBy(a => a.zIndex))
            {
                gameObject.Render(spriteBatch);
            }
        }
    }
}