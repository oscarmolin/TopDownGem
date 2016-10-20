using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Meny;
using TiledSharp;
using Microsoft.Xna.Framework.Audio;

namespace Game2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class CoolGAme : Game
    {
        Random r = new Random();

        public enum GameState
        {
            Start,
            Playing,
            Pause,
            GameOver
        }

        public SoundEffect effect;
        public static GameState GS;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Player;
        Vector2 PlayerPos;
        float pspeed;
        float pangle;
        MouseState ms;
        List<shot> shots;
        TileEngine tileEngine;
        bool faku;
        MenuComponent mc;
        PausMeny pm;
        KeyboardComponent kc;
        GamePadComponent gc;
        ServiceBus bus;
        TileEngineGood TileEngineG;
        Camera2D cam;
        float volume = 1.0f;
        float pitch = 0.5f;
        float pan = 0.0f;
        Vector2 mousePosition;

        Texture2D enemy_zombie;
        KeyboardState ks = new KeyboardState();

        Texture2D enemy_crippler;
        Texture2D enemy_spitter;
        Texture2D enemy_charger;
        Texture2D enemy_pistolzombie;
        Texture2D enemy_shotgunzombie;
        Vector2 enemyPos;
        float enemyAngle;
        EnemyStat enemyStat;

        public CoolGAme()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            // graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            enemyStat = Enemies.SpawnOne((EnemyType)r.Next(6));

            mc = new MenuComponent(this);
            Components.Add(mc);
            kc = new KeyboardComponent(this);
            Components.Add(kc);
            gc = new GamePadComponent(this);
            Components.Add(gc);
            pm = new PausMeny(this);
            Components.Add(pm);
            cam = new Camera2D();
            PlayerPos = new Vector2(300, 300);
            enemyPos = new Vector2(200, 200);
            pspeed = 4;
            shots = new List<shot>();
            GS = GameState.Start;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player = Content.Load<Texture2D>("1");

            bus = new ServiceBus();
            bus.Map = new TmxMap("data/house.tmx");
            bus.PathFinder = new PathFinder(bus);

            TileEngineG = new TileEngineGood(bus);
            TileEngineG.LoadContent(this);


            //tileEngine.TileMap = Content.Load<Texture2D>("1");
            enemy_zombie = this.Content.Load<Texture2D>("zombie2_hold");
            enemy_crippler = this.Content.Load<Texture2D>("zoimbie1_hold");
            enemy_spitter = this.Content.Load<Texture2D>("robot1_hold");
            enemy_charger = this.Content.Load<Texture2D>("robot2_hold");
            enemy_pistolzombie = this.Content.Load<Texture2D>("zombie2_silencer");
            enemy_shotgunzombie = this.Content.Load<Texture2D>("zombie2_machine");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            
            KeyboardState prevks = ks;
            ms = Mouse.GetState();
            GamePadState gs = GamePad.GetState(0);
            ks = Keyboard.GetState();
           
            switch (GS)
            {

                case GameState.Start:
                    ms = Mouse.GetState();
                    mousePosition = new Vector2(ms.Position.X, ms.Position.Y) + cam.pos -new Vector2(graphics.PreferredBackBufferWidth/2,graphics.PreferredBackBufferHeight/2);
                    mc.Update(gameTime);
                    break;
                case GameState.Playing:
                    ms = Mouse.GetState();
                    if (ks.IsKeyDown(Keys.Escape) && prevks.IsKeyUp(Keys.Escape))
                        GS = GameState.Pause;
                    if (PlayerPos.X > graphics.PreferredBackBufferWidth/2 &&
                        PlayerPos.Y > graphics.PreferredBackBufferHeight/2)
                        cam.pos = PlayerPos;
                    else
                        cam.pos = new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2);

                        mousePosition = new Vector2(ms.Position.X, ms.Position.Y) + cam.pos -
                                    new Vector2(graphics.PreferredBackBufferWidth/2,
                                        graphics.PreferredBackBufferHeight/2);
                    if (gs.IsConnected)
                    {
                        faku = true;
                        if (gs.ThumbSticks.Left != new Vector2(0, 0))
                            PlayerPos += pspeed*new Vector2(gs.ThumbSticks.Left.X, gs.ThumbSticks.Left.Y*-1);

                        if (gs.ThumbSticks.Right != new Vector2(0, 0))
                            pangle = (float) Math.Atan2(gs.ThumbSticks.Right.X, gs.ThumbSticks.Right.Y) +
                                     (float) Math.PI/2;
                    }
                    else
                    {
                        faku = false;

                        if (ks.IsKeyDown(Keys.W))
                            PlayerPos += new Vector2(0, -pspeed);
                        if (ks.IsKeyDown(Keys.A))
                            PlayerPos += new Vector2(-pspeed, 0);
                        if (ks.IsKeyDown(Keys.S))
                            PlayerPos += new Vector2(0, pspeed);
                        if (ks.IsKeyDown(Keys.D))
                            PlayerPos += new Vector2(pspeed, 0);

                        pangle = (float) Math.Atan2(PlayerPos.Y - mousePosition.Y, PlayerPos.X - mousePosition.X);

                    }

                    if (ks.IsKeyDown(Keys.R))
                        Initialize();
                    if (ks.IsKeyDown(Keys.Home))
                        graphics.ToggleFullScreen();
                    if (ks.IsKeyDown(Keys.Space) || ms.LeftButton == ButtonState.Pressed ||
                        gs.IsButtonDown(Buttons.RightTrigger))
                    {
                        shots.Add(new shot(PlayerPos, pangle));
                        effect = Content.Load<SoundEffect>("Pew");
                        effect.Play(volume, pitch, pan);
                    }
                    foreach (shot s in shots)
                    {
                        s.pos -= new Vector2(10*(float) Math.Cos(s.angle), 10*(float) Math.Sin(s.angle));
                    }
                    break;
                    case GameState.Pause:
                    if (ks.IsKeyDown(Keys.Escape) && prevks.IsKeyUp(Keys.Escape))
                    {
                        GS = GameState.Playing;
                    }
                    pm.Update(gameTime);
                    ms = Mouse.GetState();
                    mousePosition = new Vector2(ms.Position.X, ms.Position.Y) + cam.pos - new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                    break;

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                cam.get_transformation(GraphicsDevice));


            switch (GS)
            {
                case GameState.Start:
                    mc.Draw(gameTime);
                    spriteBatch.Draw(Player, new Vector2(mousePosition.X, mousePosition.Y), null, Color.Red,
                        pangle - (float) Math.PI/2, new Vector2(Player.Width/2, Player.Height/2), 0.05f,
                        SpriteEffects.None, 0);

                    break;
                case GameState.Playing:
                    TileEngineG.Draw(spriteBatch);
                    //tileEngine.Draw(gameTime,spriteBatch);
            if ( enemyStat.Enemytype == EnemyType.Zombie )
            {
                spriteBatch.Draw(enemy_zombie, enemyPos, null, Color.White, enemyAngle, new Vector2(enemy_zombie.Width / 2, enemy_zombie.Height / 2), 1.0f, SpriteEffects.None, 0);
            }
            else if (enemyStat.Enemytype == EnemyType.Crippler)
            {
                    spriteBatch.Draw(Player, PlayerPos, null, Color.White, pangle,
                        new Vector2(Player.Width/2, Player.Height/2),
                        0.1f, SpriteEffects.None, 0);

                    spriteBatch.Draw(Player, PlayerPos, null, Color.White, pangle,
                        new Vector2(Player.Width/2, Player.Height/2), 0.1f, SpriteEffects.None, 0);
                spriteBatch.Draw(enemy_crippler, enemyPos, null, Color.White, enemyAngle, new Vector2(enemy_crippler.Width / 2, enemy_crippler.Height / 2), 1.0f, SpriteEffects.None, 0);
            }
            else if (enemyStat.Enemytype == EnemyType.Spitter)
            {
                spriteBatch.Draw(enemy_spitter, enemyPos, null, Color.White, enemyAngle, new Vector2(enemy_spitter.Width / 2, enemy_spitter.Height / 2), 1.0f, SpriteEffects.None, 0);
            }
            else if (enemyStat.Enemytype == EnemyType.Charger)
            {
                spriteBatch.Draw(enemy_charger, enemyPos, null, Color.White, enemyAngle, new Vector2(enemy_charger.Width / 2, enemy_charger.Height / 2), 1.0f, SpriteEffects.None, 0);
            }
            else if (enemyStat.Enemytype == EnemyType.PistolZombie)
            {
                spriteBatch.Draw(enemy_pistolzombie, enemyPos, null, Color.White, enemyAngle, new Vector2(enemy_pistolzombie.Width / 2, enemy_pistolzombie.Height / 2), 1.0f, SpriteEffects.None, 0);
            }
            else if (enemyStat.Enemytype == EnemyType.ShotgunZombie)
            {
                spriteBatch.Draw(enemy_shotgunzombie, enemyPos, null, Color.White, enemyAngle, new Vector2(enemy_shotgunzombie.Width / 2, enemy_shotgunzombie.Height / 2), 1.0f, SpriteEffects.None, 0);
            }
                    if (!faku)
                        spriteBatch.Draw(Player, new Vector2(mousePosition.X, mousePosition.Y), null, Color.Red,
                            pangle - (float) Math.PI/2, new Vector2(Player.Width/2, Player.Height/2), 0.05f,
                            SpriteEffects.None, 0);
                    foreach (shot s in shots)
                        spriteBatch.Draw(Player, s.pos, null, Color.White, s.angle,
                            new Vector2(Player.Width/2, Player.Height/2), 0.05f, SpriteEffects.None, 0);
                    break;
                    case GameState.Pause:
                    pm.Draw(gameTime);
                    spriteBatch.Draw(Player, new Vector2(mousePosition.X, mousePosition.Y), null, Color.Red,pangle - (float)Math.PI / 2, new Vector2(Player.Width / 2, Player.Height / 2), 0.05f,SpriteEffects.None, 0);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
