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
        Player player1;
        Player player2;
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
            player1 = new Player(new Vector2(300, 300), Controller.Keyboard, 6);
            enemyPos = new Vector2(200, 200);
            player2 = new Player(new Vector2(300, 500), Controller.Controller1, 6);
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
            player1.LoadContent(this, "1");

            player2.LoadContent(this, "1");
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
            ks = Keyboard.GetState();
            mousePosition = new Vector2(ms.Position.X, ms.Position.Y) + cam.pos -new Vector2(graphics.PreferredBackBufferWidth/2,graphics.PreferredBackBufferHeight/2);
            switch (GS)
            {

                case GameState.Start:                    
                    mc.Update(gameTime);
                    break;
                case GameState.Playing:
                    player1.Update(mousePosition,ks);
                    player2.Update(mousePosition, ks);                    
                    if (ks.IsKeyDown(Keys.Escape) && prevks.IsKeyUp(Keys.Escape))
                        GS = GameState.Pause;
                    //if (player1.X > graphics.PreferredBackBufferWidth / 2 && player1.Y > graphics.PreferredBackBufferHeight / 2)
                        cam.pos = player1.position;                  
                    //else
                    //    cam.pos = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

                    if (ks.IsKeyDown(Keys.R))
                        Initialize();
                    if (ks.IsKeyDown(Keys.Home))
                        graphics.ToggleFullScreen();

                    foreach (shot s in player1.shots)
                    {
                        s.pos -= new Vector2(10 * (float)Math.Cos(s.angle), 10 * (float)Math.Sin(s.angle));
                    }
                    foreach (shot s in player2.shots)
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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,  RasterizerState.CullNone, null, cam.get_transformation(GraphicsDevice));
            
            switch (GS)
            {
                case GameState.Start:
                    mc.Draw(gameTime);
                    break;

                case GameState.Playing:
                    TileEngineG.Draw(spriteBatch);
                    player1.draw(spriteBatch);
            if ( enemyStat.Enemytype == EnemyType.Zombie )
            {
                spriteBatch.Draw(enemy_zombie, enemyPos, null, Color.White, enemyAngle, new Vector2(enemy_zombie.Width / 2, enemy_zombie.Height / 2), 1.0f, SpriteEffects.None, 0);
                    player2.draw(spriteBatch);
                    foreach (shot s in player1.shots)
                        spriteBatch.Draw(player1.texture, s.pos, null, Color.White, s.angle, new Vector2(player1.texture.Width / 2, player1.texture.Height / 2), 0.05f, SpriteEffects.None, 0);
            }
            else if (enemyStat.Enemytype == EnemyType.Crippler)
            {
                    
                    foreach (shot s in player2.shots)
                        spriteBatch.Draw(player1.texture, s.pos, null, Color.White, s.angle, new Vector2(player1.texture.Width / 2, player1.texture.Height / 2), 0.05f, SpriteEffects.None, 0);
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
                    break;

                case GameState.Pause:
                    pm.Draw(gameTime);
                    break;
            }
            if (player1.controller == Controller.Keyboard||GS!=GameState.Playing)
                spriteBatch.Draw(player1.texture, new Vector2(mousePosition.X, mousePosition.Y), null, Color.Red, 0, new Vector2(player1.texture.Width / 2, player1.texture.Height / 2), 0.05f, SpriteEffects.None, 0);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
