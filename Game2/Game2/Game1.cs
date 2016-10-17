﻿using Microsoft.Xna.Framework;
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
            Start, Playing, Pause, GameOver
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
        TmxMap map;
        TileEngineGood TileEngineG;
        Camera2D cam;
        float volume = 1.0f;
        float pitch= 0.5f;
        float pan = 0.0f;
        Vector2 mousePosition;
        public CoolGAme()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight =  1080;
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
            //var version = map.Version;
            //var myTileset = map.Tilesets["myTileset"];
            //var myLayer = map.Layers[2];
            //var hiddenChest = map.ObjectGroups["Chests"].Objects["hiddenChest"];
            mc = new  MenuComponent(this);
            Components.Add(mc);
            //tileEngine.Data = new int[,]
            //   {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //    {0,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,0},
            //    {0,1,0,0,1,0,0,0,0,0,1,0,1,1,1,0,0,0,1,0,0,1,0,0,1,1,1,0},
            //    {0,1,0,0,1,1,1,1,1,0,1,0,1,1,1,0,0,0,1,0,0,1,0,0,1,1,1,0},
            //    {0,1,0,0,1,0,0,0,0,0,1,0,1,1,1,0,0,1,1,0,0,1,0,0,1,1,1,0},
            //    {0,1,0,1,1,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,1,0,0,0,1,1,0},
            //    {0,1,0,1,1,1,1,1,0,0,0,0,0,1,0,1,1,1,1,1,1,1,0,1,0,1,1,0},
            //    {0,1,0,0,1,0,0,1,1,1,1,0,0,1,0,0,0,0,1,0,0,0,0,1,0,1,0,0},
            //    {0,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,0,1,1,1,1,0,1,0,0},
            //    {0,1,1,1,0,1,1,1,1,1,1,0,0,1,0,1,1,1,1,1,1,0,0,1,0,1,1,0},
            //    {0,1,1,1,0,1,0,0,0,0,1,0,0,1,0,1,0,1,1,1,1,0,0,1,0,0,1,0},
            //    {0,1,1,1,0,1,0,1,1,1,1,0,0,1,0,1,0,0,0,0,0,0,1,1,1,0,1,0},
            //    {0,1,1,1,0,1,0,1,0,0,0,0,0,1,0,1,0,1,1,1,0,0,0,1,0,0,1,0},
            //    {0,1,1,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,0,1,0,0,0,1,0,0,1,0},
            //    {0,1,1,1,0,1,0,0,0,0,0,1,0,1,0,1,0,1,0,1,0,1,0,1,1,0,1,0},
            //    {0,1,0,0,0,1,0,1,1,1,1,1,0,1,0,1,0,1,0,1,0,1,0,0,0,0,1,0},
            //    {0,1,0,0,0,1,0,1,0,0,0,0,0,1,0,1,0,1,0,1,1,1,1,1,0,0,1,0},
            //    {0,1,0,0,0,1,0,1,1,1,1,1,1,1,0,0,0,1,0,0,1,1,0,1,0,1,1,0},
            //    {0,1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1,0,0,1,1,0,1,1,1,0,0},
            //    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};

            cam = new Camera2D();
            PlayerPos = new Vector2(300, 300);
            pspeed = 4;
            shots = new List<shot>();
            GS =GameState.Start;
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
            map = new TmxMap("house.tmx");
            TileEngineG = new TileEngineGood(map);
            TileEngineG.LoadContent(this);

            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            ms = Mouse.GetState();
            GamePadState gs = GamePad.GetState(0);
            KeyboardState ks = Keyboard.GetState();
            switch (GS)
            {
               
                    case GameState.Start:
                    ms = Mouse.GetState();
                    mousePosition = new Vector2(ms.Position.X, ms.Position.Y) + cam.pos - new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
                    
                    mc.Update(gameTime);
                    break;
                    case GameState.Playing:
                    
            ms = Mouse.GetState();
            if (PlayerPos.X > graphics.PreferredBackBufferWidth / 2 && PlayerPos.Y > graphics.PreferredBackBufferHeight / 2)
                cam.pos = PlayerPos;
            else
                cam.pos = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            mousePosition = new Vector2(ms.Position.X,ms.Position.Y) + cam.pos - new Vector2(graphics.PreferredBackBufferWidth/2,graphics.PreferredBackBufferHeight/2);
                    if (gs.IsConnected)
                    {
                        faku = true;
                        if (gs.ThumbSticks.Left != new Vector2(0, 0))
                            PlayerPos += pspeed * new Vector2(gs.ThumbSticks.Left.X, gs.ThumbSticks.Left.Y * -1);

                        if (gs.ThumbSticks.Right != new Vector2(0, 0))
                            pangle = (float)Math.Atan2(gs.ThumbSticks.Right.X, gs.ThumbSticks.Right.Y) + (float)Math.PI / 2;
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
                        
                        pangle = (float)Math.Atan2(PlayerPos.Y - mousePosition.Y, PlayerPos.X - mousePosition.X);

                    }
            
                    if (ks.IsKeyDown(Keys.R))
                        Initialize();
                    if (ks.IsKeyDown(Keys.Home))
                        graphics.ToggleFullScreen();
                    if (ks.IsKeyDown(Keys.Space) || ms.LeftButton == ButtonState.Pressed|| gs.IsButtonDown(Buttons.RightTrigger))
                    {
                        shots.Add(new shot(PlayerPos, pangle));
                        effect = Content.Load<SoundEffect>("Pew");
                        effect.Play(volume, pitch, pan);
                    }
                    foreach (shot s in shots)
                    {
                        s.pos -= new Vector2(10 * (float)Math.Cos(s.angle), 10 * (float)Math.Sin(s.angle));
                    }
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
            spriteBatch.Begin( SpriteSortMode.Deferred,
                               BlendState.AlphaBlend,
                               SamplerState.PointClamp,
                               DepthStencilState.Default,
                               RasterizerState.CullNone,
                               null,
                               cam.get_transformation(GraphicsDevice));
            
            TileEngineG.Draw(spriteBatch);
            //tileEngine.Draw(gameTime,spriteBatch);
            spriteBatch.Draw(Player,PlayerPos,null,Color.White,pangle,new Vector2(Player.Width/2,Player.Height/2),0.1f,SpriteEffects.None,0);
            switch (GS)
            {
                case GameState.Start:
                   mc.Draw(gameTime);
                    spriteBatch.Draw(Player, new Vector2(mousePosition.X, mousePosition.Y), null, Color.Red, pangle - (float)Math.PI / 2, new Vector2(Player.Width / 2, Player.Height / 2), 0.05f, SpriteEffects.None, 0);

                    break;
                case GameState.Playing:

                    
                    spriteBatch.Draw(Player, PlayerPos, null, Color.White, pangle, new Vector2(Player.Width/2, Player.Height/2), 0.1f, SpriteEffects.None, 0);
                   if(!faku)
                spriteBatch.Draw(Player, new Vector2(mousePosition.X, mousePosition.Y), null, Color.Red,pangle - (float)Math.PI/2, new Vector2(Player.Width / 2, Player.Height / 2), 0.05f, SpriteEffects.None, 0);
                    foreach (shot s in shots)
                spriteBatch.Draw(Player, s.pos, null, Color.White, s.angle, new Vector2(Player.Width / 2, Player.Height / 2), 0.05f, SpriteEffects.None, 0);
                    break;
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
