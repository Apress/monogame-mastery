using System;
using System.Collections.Generic;
using System.Linq;

using chapter_06.Enum;
using chapter_06.Input.Base;
using chapter_06.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace chapter_06.States.Base
{
    public abstract class BaseGameState
    {
        private const string FallbackTexture = "Empty";

        private ContentManager _contentManager;
        protected int _viewportHeight;
        protected int _viewportWidth;

        private readonly List<BaseGameObject> _gameObjects = new List<BaseGameObject>();

        protected InputManager InputManager {get; set;}

        public void Initialize(ContentManager contentManager, int viewportWidth, int viewportHeight)
        {
            _contentManager = contentManager;
            _viewportHeight = viewportHeight;
            _viewportWidth = viewportWidth;

            SetInputManager();
        }

        public abstract void LoadContent();
        public virtual void Update(GameTime gameTime) { }
        public abstract void HandleInput(GameTime gameTime);

        public event EventHandler<BaseGameState> OnStateSwitched;
        public event EventHandler<Events> OnEventNotification;
        protected abstract void SetInputManager();

        public void UnloadContent()
        {
            _contentManager.Unload();
        }

        protected Texture2D LoadTexture(string textureName)
        {
            var texture = _contentManager.Load<Texture2D>(textureName);

            return texture ?? _contentManager.Load<Texture2D>(FallbackTexture);
        }
 
        protected void NotifyEvent(Events eventType, object argument = null)
        {
            OnEventNotification?.Invoke(this, eventType);

            foreach (var gameObject in _gameObjects)
            {
                gameObject.OnNotify(eventType, argument);
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

        protected void RemoveGameObject(BaseGameObject gameObject)
        {
            _gameObjects.Remove(gameObject);
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