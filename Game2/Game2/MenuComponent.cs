using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Game2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Meny
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch _spriteBatch;
        SpriteFont _normalFont;
        SpriteFont _selectedFont;
        List<MenuChoice> _choices;
        MouseState _previousMouseState;
        OptionsMeny om;
        public enum GameState
        {
            MainMenu, Playing
        }

        public static GameState gs;
        public MenuComponent(Game game) : base(game)
        {
            _choices = new List<MenuChoice>();
            _choices.Add(new MenuChoice() {Text = "START", Selected = true, ClickAction = MenuStartClicked});           
            _choices.Add(new MenuChoice() { Text = "OPTIONS", ClickAction = MenuOptionsClicked });
            _choices.Add(new MenuChoice() { Text = "QUIT", ClickAction = MenuQuitClicked });
            om = new OptionsMeny();
            gs = GameState.MainMenu;
            
        }
        
        private void MenuStartClicked()
        {
            CoolGAme.GS = CoolGAme.GameState.Playing;
            gs = GameState.Playing;
        }
        private void MenuOptionsClicked()
        {
            om.Draw();
        }
        private void MenuQuitClicked()
        {
            Game.Exit();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _normalFont = Game.Content.Load<SpriteFont>("menuFontNormal");
            _selectedFont = Game.Content.Load<SpriteFont>("menuFontSelected");
            float startY = 0.2f * GraphicsDevice.Viewport.Height;

            
                    foreach (var choice in _choices)
                {
                    Vector2 size = _normalFont.MeasureString(choice.Text);
                    choice.Y = startY;
                    choice.X = GraphicsDevice.Viewport.Width/2.0f - size.X/2;
                    choice.HitBox = new Rectangle((int) choice.X, (int) choice.Y, (int) size.X, (int) size.Y);
                    startY += 70;
                }
              

            _previousMouseState = Mouse.GetState();
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            switch (gs)
            {

                case GameState.MainMenu:
                    
                        if (KeyboardComponent.KeyPressed(Keys.Down) || GamePadComponent.ButtonPressed(Buttons.LeftThumbstickDown) ||  KeyboardComponent.KeyPressed(Keys.S))
            {
                PreviousMenuChoice();
            }
            if (KeyboardComponent.KeyPressed(Keys.Up) || GamePadComponent.ButtonPressed(Buttons.LeftThumbstickUp) || KeyboardComponent.KeyPressed(Keys.W))
            {
                NextMenuChoice();
            }
            if (KeyboardComponent.KeyPressed(Keys.Enter) || GamePadComponent.ButtonPressed(Buttons.A) || KeyboardComponent.KeyPressed(Keys.Space))
            {
                
                    var selectedChoice = _choices.First(c => c.Selected);
                    selectedChoice.ClickAction.Invoke();
            }
            
            var mouseState = Mouse.GetState();
            foreach (var choice in _choices)
            {
                if (choice.HitBox.Contains(mouseState.X, mouseState.Y))
                {
                    _choices.ForEach(c => c.Selected = false);
                    choice.Selected = true;

                    if (_previousMouseState.LeftButton == ButtonState.Released
                        && mouseState.LeftButton == ButtonState.Pressed)
                        choice.ClickAction.Invoke();
                }
            }
            

            _previousMouseState = mouseState;
                    break;
                case GameState.Playing:
                    break;
            }
            base.Update(gameTime);
                       

        }
        private void PreviousMenuChoice()
        {
            switch (gs)
            {

                case GameState.MainMenu:

                    int selectedIndex = _choices.IndexOf(_choices.First(c => c.Selected));
                _choices[selectedIndex].Selected = false;
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = _choices.Count - 1;
                _choices[selectedIndex].Selected = true;
                    break;
                case GameState.Playing:
                    break;
            }
        }

        private void NextMenuChoice()
        {
            switch (gs)
            {

                case GameState.MainMenu:

                    int selectedIndex = _choices.IndexOf(_choices.First(c => c.Selected));
                _choices[selectedIndex].Selected = false;
                selectedIndex++;
                if (selectedIndex >= _choices.Count)
                selectedIndex = 0;
                _choices[selectedIndex].Selected = true;
                    break;
                case GameState.Playing:
                    break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            switch (gs)
            {

                case GameState.MainMenu:
                    _spriteBatch.Begin();
                    foreach (var choice in _choices)
                    {
                        //if(choice.IsVisible != null && !choice.IsVisible())
                        //    continue;
                        _spriteBatch.DrawString(choice.Selected ? _selectedFont : _normalFont,
                            choice.Text, new Vector2(choice.X, choice.Y), Color.White);
                    }
                    _spriteBatch.End();
                    base.Draw(gameTime);
                break;
                case GameState.Playing:
                break;
            }

        }
    }
}
