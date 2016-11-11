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
        public GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;
        Player player1;
        Player player2;
        MouseState ms;
        TileEngine tileEngine;
        bool faku;
        MenuComponent mc;
        KeyboardComponent kc;
        GamePadComponent gc;
        TmxMap map;
        TileEngineGood TileEngineG;
        Camera2D cam;

        Vector2 mousePosition;
        KeyboardState ks = new KeyboardState();
        GamePadState gs = GamePad.GetState(0);

        public CoolGAme()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 1080;
            Graphics.IsFullScreen = false;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            mc = new MenuComponent(this);
            Components.Add(mc);
            kc = new KeyboardComponent(this);
            Components.Add(kc);
            gc = new GamePadComponent(this);
            Components.Add(gc);
            cam = new Camera2D();
            player1 = new Player(new Vector2(900, 300), Controller.Keyboard, 6);
            player2 = new Player(new Vector2(900, 500), Controller.Controller1, 6);
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
            
            

            //tileEngine.TileMap = Content.Load<Texture2D>("1");
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
            GamePadState prevgs = gs;
            gs = GamePad.GetState(0);
            ms = Mouse.GetState();
            ks = Keyboard.GetState();
            mousePosition = new Vector2(ms.Position.X, ms.Position.Y) + cam.pos -new Vector2(Graphics.PreferredBackBufferWidth/2,Graphics.PreferredBackBufferHeight/2);
            switch (GS)
            {

                case GameState.Start:
                    
                    break;
                case GameState.Playing:
                    

                    TileEngineG = new TileEngineGood(map);
                    TileEngineG.LoadContent(this);
                    player1.Update(mousePosition,ks);
                    player2.Update(mousePosition, ks);
                    
                    if (ks.IsKeyDown(Keys.Escape) && prevks.IsKeyUp(Keys.Escape) || gs.IsButtonDown(Buttons.Start) && prevgs.IsButtonUp(Buttons.Start))
                    {
                        GS = GameState.Pause;
                        MenuComponent.gs = MenuComponent.GameState.MainMenu;
                    }
                   
                        cam.pos = player1.position;                  
                    

                    if (ks.IsKeyDown(Keys.R))
                        Initialize();
                    if (ks.IsKeyDown(Keys.Home))
                        Graphics.ToggleFullScreen();
                    
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
                    ms = Mouse.GetState();
                    mousePosition = new Vector2(ms.Position.X, ms.Position.Y) + cam.pos - new Vector2(Graphics.PreferredBackBufferWidth / 2, Graphics.PreferredBackBufferHeight / 2);
                    if (ks.IsKeyDown(Keys.Escape) && prevks.IsKeyUp(Keys.Escape) || gs.IsButtonDown(Buttons.Start) && prevgs.IsButtonUp(Buttons.Start))
                    {
                        GS = GameState.Playing;
                        MenuComponent.gs = MenuComponent.GameState.Playing;
                    }
                    if (MenuComponent.CL == MenuComponent.Controll.Cont)
                        player1.controller = Controller.Controller1;
                    else
                        player1.controller = Controller.Keyboard;
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
                    if (MenuComponent.SP == MenuComponent.SelMap.Forrest)
                        map = new TmxMap("ForrestMap.tmx");
                    if (MenuComponent.SP == MenuComponent.SelMap.Stone)
                        map = new TmxMap("StoneMap.tmx");

                    TileEngineG = new TileEngineGood(map);
                    TileEngineG.LoadContent(this);
                    TileEngineG.Draw(spriteBatch);
                    player1.draw(spriteBatch);
                    player2.draw(spriteBatch);
                    foreach (shot s in player1.shots)
                        spriteBatch.Draw(player1.texture, s.pos, null, Color.White, s.angle, new Vector2(player1.texture.Width / 2, player1.texture.Height / 2), 0.05f, SpriteEffects.None, 0);

                    foreach (shot s in player2.shots)
                        spriteBatch.Draw(player1.texture, s.pos, null, Color.White, s.angle, new Vector2(player1.texture.Width / 2, player1.texture.Height / 2), 0.05f, SpriteEffects.None, 0);
                    break;
                case GameState.Pause:
                    TileEngineG.Draw(spriteBatch);
                    player1.draw(spriteBatch);
                    player2.draw(spriteBatch);
                    foreach (shot s in player1.shots)
                        spriteBatch.Draw(player1.texture, s.pos, null, Color.White, s.angle, new Vector2(player1.texture.Width / 2, player1.texture.Height / 2), 0.05f, SpriteEffects.None, 0);
                    
                    foreach (shot s in player2.shots)
                        spriteBatch.Draw(player1.texture, s.pos, null, Color.White, s.angle, new Vector2(player1.texture.Width / 2, player1.texture.Height / 2), 0.05f, SpriteEffects.None, 0);
                    spriteBatch.End();
                    mc.Draw(gameTime);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, cam.get_transformation(GraphicsDevice));
                    //if (player1.controller == Controller.Keyboard || GS != GameState.Playing) spriteBatch.Draw(player1.texture, new Vector2(mousePosition.X, mousePosition.Y), null, Color.Red, 0, new Vector2(player1.texture.Width/2, player1.texture.Height/2), 0.05f, SpriteEffects.None, 0);
                    //spriteBatch.End();



                    break;
                    
            }
            if (player1.controller == Controller.Keyboard || GS != GameState.Playing)
                spriteBatch.Draw(player1.texture, new Vector2(mousePosition.X, mousePosition.Y), null, Color.Red, 0, new Vector2(player1.texture.Width / 2, player1.texture.Height / 2), 0.05f, SpriteEffects.None, 0);
            spriteBatch.End();

            //if (GS == GameState.Pause)
            //{
               
            //}
            base.Draw(gameTime);
        }
    }
}