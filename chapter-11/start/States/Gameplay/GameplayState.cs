using chapter_11.Engine.Input;
using chapter_11.Engine.Objects;
using chapter_11.Engine.States;
using chapter_11.Input;
using chapter_11.Objects;
using chapter_11.Objects.Text;
using chapter_11.States.Gameplay;
using chapter_11.States.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace chapter_11.States
{
    public class GameplayState : BaseGameState
    {
        private const string BackgroundTexture = "Sprites/Barren";
        private const string PlayerFighter = "Sprites/Animations/FighterSpriteSheet";
        private const string BulletTexture = "Sprites/bullet";
        private const string ExhaustTexture = "Sprites/Cloud";
        private const string MissileTexture = "Sprites/Missile";
        private const string ChopperTexture = "Sprites/Chopper";
        private const string ExplosionTexture = "Sprites/explosion";

        private const string TextFont = "Fonts/Lives";
        private const string GameOverFont = "Fonts/GameOver";

        private const string BulletSound = "Sounds/bulletSound";
        private const string MissileSound = "Sounds/missileSound";

        private const string Soundtrack1 = "Music/FutureAmbient_1";
        private const string Soundtrack2 = "Music/FutureAmbient_2";

        private const int StartingPlayerLives = 3;
        private int _playerLives = StartingPlayerLives;

        private const int MaxExplosionAge = 600; // 10 seconds
        private const int ExplosionActiveLength = 75; // emit particles for 1.2 seconds and let them fade out for 10 seconds

        private Texture2D _missileTexture;
        private Texture2D _exhaustTexture;
        private Texture2D _bulletTexture;
        private Texture2D _explosionTexture;
        private Texture2D _chopperTexture;
        private Texture2D _screenBoxTexture;

        private LivesText _livesText;
        private PlayerSprite _playerSprite;
        private bool _playerDead;
        private bool _gameOver = false;

        private bool _isShootingBullets;
        private bool _isShootingMissile;
        private TimeSpan _lastBulletShotAt;
        private TimeSpan _lastMissileShotAt;

        private List<BulletSprite> _bulletList = new List<BulletSprite>();
        private List<MissileSprite> _missileList = new List<MissileSprite>();
        private List<ExplosionEmitter> _explosionList = new List<ExplosionEmitter>();
        private List<ChopperSprite> _enemyList = new List<ChopperSprite>();

        private ChopperGenerator _chopperGenerator;

        public override void LoadContent()
        {
            _missileTexture = LoadTexture(MissileTexture);
            _exhaustTexture = LoadTexture(ExhaustTexture);
            _bulletTexture = LoadTexture(BulletTexture);
            _explosionTexture = LoadTexture(ExplosionTexture);
            _chopperTexture = LoadTexture(ChopperTexture);

            _playerSprite = new PlayerSprite(LoadTexture(PlayerFighter));
            _livesText = new LivesText(LoadFont(TextFont));
            _livesText.NbLives = StartingPlayerLives;
            _livesText.Position = new Vector2(10.0f, 690.0f);

            AddGameObject(new TerrainBackground(LoadTexture(BackgroundTexture)));
            AddGameObject(_livesText);

            // load sound effects and register in the sound manager
            var bulletSound = LoadSound(BulletSound);
            var missileSound = LoadSound(MissileSound);
            _soundManager.RegisterSound(new GameplayEvents.PlayerShootsBullets(), bulletSound);
            _soundManager.RegisterSound(new GameplayEvents.PlayerShootsMissile(), missileSound, 0.4f, -0.2f, 0.0f);

            // load soundtracks into sound manager
            var track1 = LoadSound(Soundtrack1).CreateInstance();
            var track2 = LoadSound(Soundtrack2).CreateInstance();
            _soundManager.SetSoundtrack(new List<SoundEffectInstance>() { track1, track2 });

            ResetGame();
        }
        
        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    NotifyEvent(new BaseGameStateEvent.GameQuit());
                }

                if (cmd is GameplayInputCommand.PlayerMoveLeft && !_playerDead)
                {
                    _playerSprite.MoveLeft();
                    KeepPlayerInBounds();
                }

                if (cmd is GameplayInputCommand.PlayerMoveRight && !_playerDead)
                {
                    _playerSprite.MoveRight();
                    KeepPlayerInBounds();
                }

                if (cmd is GameplayInputCommand.PlayerStopsMoving && !_playerDead)
                {
                    _playerSprite.StopMoving();
                }

                if (cmd is GameplayInputCommand.PlayerShoots && !_playerDead)
                {
                    Shoot(gameTime);
                }
            });
        }

        public override void UpdateGameState(GameTime gameTime)
        {
            _playerSprite.Update(gameTime);

            foreach (var bullet in _bulletList)
            {
                bullet.MoveUp();
            }

            foreach (var missile in _missileList)
            {
                missile.Update(gameTime);
            }

            foreach (var chopper in _enemyList)
            {
                chopper.Update();
            }

            UpdateExplosions(gameTime);
            RegulateShootingRate(gameTime);
            DetectCollisions();

            // get rid of bullets and missiles that have gone out of view
            _bulletList = CleanObjects(_bulletList);
            _missileList = CleanObjects(_missileList);
            _enemyList = CleanObjects(_enemyList);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            if (_gameOver)
            {
                // draw black rectangle at 30% transparency
                var screenBoxTexture = GetScreenBoxTexture(spriteBatch.GraphicsDevice);
                var viewportRectangle = new Rectangle(0, 0, _viewportWidth, _viewportHeight);
                spriteBatch.Draw(screenBoxTexture, viewportRectangle, Color.Black * 0.3f);
            }
        }

        private Texture2D GetScreenBoxTexture(GraphicsDevice graphicsDevice)
        {
            if (_screenBoxTexture == null)
            {
                _screenBoxTexture = new Texture2D(graphicsDevice, 1, 1);
                _screenBoxTexture.SetData<Color>(new Color[] { Color.White });
            }

            return _screenBoxTexture;
        }

        private void RegulateShootingRate(GameTime gameTime)
        {
            // can't shoot bullets more than every 0.2 second
            if (_lastBulletShotAt != null && gameTime.TotalGameTime - _lastBulletShotAt > TimeSpan.FromSeconds(0.2))
            {
                _isShootingBullets = false;
            }

            // can't shoot missiles more than every 1 second
            if (_lastMissileShotAt != null && gameTime.TotalGameTime - _lastMissileShotAt > TimeSpan.FromSeconds(1.0))
            {
                _isShootingMissile = false;
            }
        }

        private void DetectCollisions()
        {
            var bulletCollisionDetector = new AABBCollisionDetector<BulletSprite, ChopperSprite>(_bulletList);
            var missileCollisionDetector = new AABBCollisionDetector<MissileSprite, ChopperSprite>(_missileList);
            var playerCollisionDetector = new AABBCollisionDetector<ChopperSprite, PlayerSprite>(_enemyList);

            bulletCollisionDetector.DetectCollisions(_enemyList, (bullet, chopper) =>
            {
                var hitEvent = new GameplayEvents.ChopperHitBy(bullet);
                chopper.OnNotify(hitEvent);
                _soundManager.OnNotify(hitEvent);
                bullet.Destroy();
            });

            missileCollisionDetector.DetectCollisions(_enemyList, (missile, chopper) =>
            {
                var hitEvent = new GameplayEvents.ChopperHitBy(missile);
                chopper.OnNotify(hitEvent);
                _soundManager.OnNotify(hitEvent);
                missile.Destroy();
            });

            if (!_playerDead)
            {
                playerCollisionDetector.DetectCollisions(_playerSprite, (chopper, player) =>
                {
                    KillPlayer();
                });
            }
        }

        private void ResetGame()
        {
            if (_chopperGenerator != null)
            {
                _chopperGenerator.StopGenerating();
            }

            foreach(var bullet in _bulletList)
            {
                RemoveGameObject(bullet);
            }

            foreach(var missile in _missileList)
            {
                RemoveGameObject(missile);
            }

            foreach(var chopper in _enemyList)
            {
                RemoveGameObject(chopper);
            }

            foreach(var explosion in _explosionList)
            {
                RemoveGameObject(explosion);
            }

            _bulletList = new List<BulletSprite>();
            _missileList = new List<MissileSprite>();
            _explosionList = new List<ExplosionEmitter>();
            _enemyList = new List<ChopperSprite>();

            _chopperGenerator = new ChopperGenerator(_chopperTexture, 4, AddChopper);
            _chopperGenerator.GenerateChoppers();

            AddGameObject(_playerSprite);

            // position the player in the middle of the screen, at the bottom, leaving a slight gap at the bottom
            var playerXPos = _viewportWidth / 2 - _playerSprite.Width / 2;
            var playerYPos = _viewportHeight - _playerSprite.Height - 30;
            _playerSprite.Position = new Vector2(playerXPos, playerYPos);

            _playerDead = false;
        }

        private async void KillPlayer()
        {
            _playerDead = true;
            _playerLives -= 1;
            _livesText.NbLives = _playerLives;

            AddExplosion(_playerSprite.Position);
            RemoveGameObject(_playerSprite);

            await Task.Delay(TimeSpan.FromSeconds(2));

            if (_playerLives > 0)
            {
                ResetGame();
            }
            else
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            var font = LoadFont(GameOverFont);
            var gameOverText = new GameOverText(font);
            var textPositionOnScreen = new Vector2(460, 300);

            gameOverText.Position = textPositionOnScreen;
            AddGameObject(gameOverText);
            _gameOver = true;
        }

        private void AddChopper(ChopperSprite chopper)
        {
            chopper.OnObjectChanged += _chopperSprite_OnObjectChanged;
            _enemyList.Add(chopper);
            AddGameObject(chopper);
        }

        private List<T> CleanObjects<T>(List<T> objectList) where T : BaseGameObject
        {
            List<T> listOfItemsToKeep = new List<T>();
            foreach(T item in objectList)
            {
                var offScreen = item.Position.Y < -50;

                if (offScreen || item.Destroyed)
                {
                    RemoveGameObject(item);
                }
                else
                {
                    listOfItemsToKeep.Add(item);
                }
            }

            return listOfItemsToKeep;
        }

        private void _chopperSprite_OnObjectChanged(object sender, BaseGameStateEvent e)
        {
            var chopper = (ChopperSprite)sender;
            switch (e)
            {
                case GameplayEvents.EnemyLostLife ge:
                    if (ge.CurrentLife <= 0)
                    {
                        AddExplosion(new Vector2(chopper.Position.X - 40, chopper.Position.Y - 40));
                        chopper.Destroy();
                    }
                    break;
            }
        }

        private void AddExplosion(Vector2 position)
        {
            var explosion = new ExplosionEmitter(_explosionTexture, position);
            AddGameObject(explosion);
            _explosionList.Add(explosion);
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            foreach (var explosion in _explosionList)
            {
                explosion.Update(gameTime);
                
                if (explosion.Age > ExplosionActiveLength)
                {
                    explosion.Deactivate();
                }

                if (explosion.Age > MaxExplosionAge)
                {
                    RemoveGameObject(explosion);
                }
            }
        }
 
        private void Shoot(GameTime gameTime)
        {
            if (!_isShootingBullets)
            {
                CreateBullets();
                _isShootingBullets = true;
                _lastBulletShotAt = gameTime.TotalGameTime;

                NotifyEvent(new GameplayEvents.PlayerShootsBullets());
            }

            if (!_isShootingMissile)
            {
                CreateMissile();
                _isShootingMissile = true;
                _lastMissileShotAt = gameTime.TotalGameTime;

                NotifyEvent(new GameplayEvents.PlayerShootsMissile());
            }
        }

        private void CreateBullets()
        {
            var bulletSpriteLeft = new BulletSprite(_bulletTexture);
            var bulletSpriteRight = new BulletSprite(_bulletTexture);

            var bulletY = _playerSprite.Position.Y + 30;
            var bulletLeftX = _playerSprite.Position.X + _playerSprite.Width / 2 - 40;
            var bulletRightX = _playerSprite.Position.X + _playerSprite.Width / 2 + 10;

            bulletSpriteLeft.Position = new Vector2(bulletLeftX, bulletY);
            bulletSpriteRight.Position = new Vector2(bulletRightX, bulletY);

            _bulletList.Add(bulletSpriteLeft);
            _bulletList.Add(bulletSpriteRight);

            AddGameObject(bulletSpriteLeft);
            AddGameObject(bulletSpriteRight);
        }

        private void CreateMissile()
        {
            var missileSprite = new MissileSprite(_missileTexture, _exhaustTexture);
            missileSprite.Position = new Vector2(_playerSprite.Position.X + 33, _playerSprite.Position.Y - 25);

            _missileList.Add(missileSprite);
            AddGameObject(missileSprite);
        }

        private void KeepPlayerInBounds()
        {
            if (_playerSprite.Position.X < 0)
            {
                _playerSprite.Position = new Vector2(0, _playerSprite.Position.Y);
            }

            if (_playerSprite.Position.X > _viewportWidth - _playerSprite.Width)
            {
                _playerSprite.Position = new Vector2(_viewportWidth - _playerSprite.Width, _playerSprite.Position.Y);
            }

            if (_playerSprite.Position.Y < 0)
            {
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, 0);
            }

            if (_playerSprite.Position.Y > _viewportHeight - _playerSprite.Height)
            {
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, _viewportHeight - _playerSprite.Height);
            }
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }
    }
}