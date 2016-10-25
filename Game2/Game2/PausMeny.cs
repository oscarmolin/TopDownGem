//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using Game2;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;

//namespace Meny
//{
//    /// <summary>
//    /// This is the main type for your game.
//    /// </summary>
//    public class PausMeny : Microsoft.Xna.Framework.DrawableGameComponent
//    {
//        public enum GameState
//        {
//            PausMenu, Playing
//        }
//        SpriteBatch _spriteBatch;
//        SpriteFont _normalFont;
//        SpriteFont _selectedFont;
//        List<MenuChoice> _Paus;
//        MouseState _previousMouseState;
//        public static GameState PS;
        
//        public PausMeny(Game game) : base(game)
//        {
//            _Paus = new List<MenuChoice>();
//            _Paus.Add(new MenuChoice { Text = "CONTINUE", Selected = true, ClickAction = MenuStartClicked });
//            _Paus.Add(new MenuChoice { Text = "QUIT", ClickAction = MenuQuitClicked });
//        }

//        private void MenuStartClicked()
//        {
//            CoolGAme.GS = CoolGAme.GameState.Playing;
//            PS = GameState.Playing;
//        }
//        private void MenuQuitClicked()
//        {
//            Game.Exit();
//        }
//        protected override void LoadContent()
//        {
//            _spriteBatch = new SpriteBatch(GraphicsDevice);
//            _normalFont = Game.Content.Load<SpriteFont>("menuFontNormal");
//            _selectedFont = Game.Content.Load<SpriteFont>("menuFontSelected");
//            float startY = 0.2f * GraphicsDevice.Viewport.Height;

            
//                    foreach (var choice in _Paus)
//                    {
//                        Vector2 size = _normalFont.MeasureString(choice.Text);
//                        choice.Y = startY;
//                        choice.X = GraphicsDevice.Viewport.Width/2.0f - size.X/2;
//                        choice.HitBox = new Rectangle((int) choice.X, (int) choice.Y, (int) size.X, (int) size.Y);
//                        startY += 70;
//                    }
            

//            _previousMouseState = Mouse.GetState();
//            base.LoadContent();
//        }
//        public override void Update(GameTime gameTime)
//        {
//            switch (PS)
//            {
//                case GameState.PausMenu:
//                    if (KeyboardComponent.KeyPressed(Keys.Down) ||
//                    GamePadComponent.ButtonPressed(Buttons.LeftThumbstickDown) || KeyboardComponent.KeyPressed(Keys.S))
//                {
//                    PreviousMenuChoice();
//                }
//                if (KeyboardComponent.KeyPressed(Keys.Up) || GamePadComponent.ButtonPressed(Buttons.LeftThumbstickUp) ||
//                    KeyboardComponent.KeyPressed(Keys.W))
//                {
//                    NextMenuChoice();
//                }
//                if (KeyboardComponent.KeyPressed(Keys.Enter) || GamePadComponent.ButtonPressed(Buttons.A) ||
//                    KeyboardComponent.KeyPressed(Keys.Space))
//                {

//                    var selectedChoice = _Paus.First(c => c.Selected);
//                    selectedChoice.ClickAction.Invoke();
//                }

//                var mouseState = Mouse.GetState();
//                foreach (var choice in _Paus)
//                {
//                    if (choice.HitBox.Contains(mouseState.X, mouseState.Y))
//                    {
//                        _Paus.ForEach(c => c.Selected = false);
//                        choice.Selected = true;

//                        if (_previousMouseState.LeftButton == ButtonState.Released
//                            && mouseState.LeftButton == ButtonState.Pressed)
//                            choice.ClickAction.Invoke();
//                    }
//                }
            
            


//            _previousMouseState = mouseState;
//                    break;
//                case GameState.Playing:
//                    break;
//            }
//            base.Update(gameTime);

//        }
//        private void PreviousMenuChoice()
//        {
            
//                    int selectedIndex = _Paus.IndexOf(_Paus.First(c => c.Selected));
//            _Paus[selectedIndex].Selected = false;
//            selectedIndex--;
//            if (selectedIndex < 0)
//            selectedIndex = _Paus.Count - 1;
//            _Paus[selectedIndex].Selected = true;
           
//        }

    

//        private void NextMenuChoice()
//        {
//                    int selectedIndex = _Paus.IndexOf(_Paus.First(c => c.Selected));
//            _Paus[selectedIndex].Selected = false;
//            selectedIndex++;
//            if (selectedIndex >= _Paus.Count)
//            selectedIndex = 0;
          

//        }

//        public void Draw(GameTime gameTime)
//        {
           
//                    _spriteBatch.Begin();
//                    foreach (var choice in _Paus)
//                    {
//                        _spriteBatch.DrawString(choice.Selected ? _selectedFont : _normalFont,
//                            choice.Text, new Vector2(choice.X, choice.Y), Color.White);
//                    }
//                    _spriteBatch.End();

//                    base.Draw(gameTime);
            
//        }
//    }
//}