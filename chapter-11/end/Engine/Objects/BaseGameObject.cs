using chapter_11.Engine.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace chapter_11.Engine.Objects
{
    public class BaseGameObject
    {
        protected Texture2D _texture;
        protected Texture2D _boundingBoxTexture;

        protected Vector2 _position = Vector2.One;
        protected float _angle;
        protected Vector2 _direction;
        protected List<BoundingBox> _boundingBoxes = new List<BoundingBox>();

        public int zIndex;
        public event EventHandler<BaseGameStateEvent> OnObjectChanged;

        public bool Destroyed { get; private set; }

        public virtual int Width { get { return _texture.Width; } }
        public virtual int Height { get { return _texture.Height; } }
        public virtual Vector2 Position 
        { 
            get { return _position; } 
            set 
            {
                var deltaX = value.X - _position.X;
                var deltaY = value.Y - _position.Y;
                _position = value; 

                foreach(var bb in _boundingBoxes)
                {
                    bb.Position = new Vector2(bb.Position.X + deltaX, bb.Position.Y + deltaY);
                }
            } 
        }

        public List<BoundingBox> BoundingBoxes
        {
            get
            {
                return _boundingBoxes;
            }
        }

        public virtual void OnNotify(BaseGameStateEvent gameEvent) { }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            if (!Destroyed)
            {
                spriteBatch.Draw(_texture, _position, Color.White);
            }
        }

        public void RenderBoundingBoxes(SpriteBatch spriteBatch)
        {
            if (Destroyed)
            {
                return;
            }

            if (_boundingBoxTexture == null)
            {
                CreateBoundingBoxTexture(spriteBatch.GraphicsDevice);
            }

            foreach (var bb in _boundingBoxes)
            {
                spriteBatch.Draw(_boundingBoxTexture, bb.Rectangle, Color.Red);
            }
        }

        public void Destroy()
        {
            Destroyed = true;
        }
        
        public void SendEvent(BaseGameStateEvent e)
        {
            OnObjectChanged?.Invoke(this, e);
        }

        public void AddBoundingBox(BoundingBox bb)
        {
            _boundingBoxes.Add(bb);
        }

        protected Vector2 CalculateDirection(float angleOffset = 0.0f)
        {
            _direction = new Vector2((float)Math.Cos(_angle - angleOffset), (float)Math.Sin(_angle - angleOffset));
            _direction.Normalize();

            return _direction;
        }


        private void CreateBoundingBoxTexture(GraphicsDevice graphicsDevice)
        {
            _boundingBoxTexture = new Texture2D(graphicsDevice, 1, 1);
            _boundingBoxTexture.SetData<Color>(new Color[] { Color.White });
        }
    }
}