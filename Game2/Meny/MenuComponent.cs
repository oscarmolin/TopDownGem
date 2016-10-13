using System;
using System.Collections.Generic;
using System.Linq;
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
        SpriteFont _normalFont;   
        SpriteBatch _spriteBatch;
        SpriteFont _selectedFont;
        List<MenuChoice> _choices;
        Color _backgroundColor;
        MouseState _previousMouseState;

        public MenuComponent(Game game) : base(game)
        {
        }
        public override void Initialize()
        {
            _choices = new List<MenuChoice>();
            _choices.Add(new MenuChoice() { Text = "START", Selected = true, ClickAction = MenuStartClicked });
            _choices.Add(new MenuChoice() { Text = "SELECT LEVEL", ClickAction = MenuSelectClicked });
            _choices.Add(new MenuChoice() { Text = "OPTIONS", ClickAction = MenuOptionsClicked });
            _choices.Add(new MenuChoice() { Text = "QUIT", ClickAction = MenuQuitClicked });

            
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _normalFont = Game.Content.Load<SpriteFont>("MenuFontNormal");
            _selectedFont = Game.Content.Load<SpriteFont>("menuFontSelected");
            _backgroundColor = Color.White;

            _previousMouseState = Mouse.GetState();
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (KeyboardComponent.KeyPressed(Keys.Down))
            NextMenuChoice();
            if (KeyboardComponent.KeyPressed(Keys.Up))
            PreviousMenuChoice();
            if (KeyboardComponent.KeyPressed(Keys.Enter))
            {
                var selectedChoice = _choices.First(c => c.Selected);
                selectedChoice.ClickAction.Invoke();
            }
            var mouseState = Mouse.GetState();


            _previousMouseState = mouseState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_backgroundColor);
            _spriteBatch.Begin();

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void MenuStartClicked()
        {
            _backgroundColor = Color.Turquoise;
        }

        private void MenuSelectClicked()
        {
            _backgroundColor = Color.Teal;
        }

        private void MenuOptionsClicked()
        {
            _backgroundColor = Color.Silver;
        }

        private void MenuQuitClicked()
        {
            this.Game.Exit();
        }

        private void PreviousMenuChoice()
        {
            int selectedIndex = _choices.IndexOf(_choices.First(c => c.Selected));
            _choices[selectedIndex].Selected = false;
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = _choices.Count - 1;
            _choices[selectedIndex].Selected = true;
        }

        private void NextMenuChoice()
        {
            int selectedIndex = _choices.IndexOf(_choices.First(c => c.Selected));
            _choices[selectedIndex].Selected = false;
            selectedIndex++;
            if (selectedIndex >= _choices.Count)
                selectedIndex = 0;
            _choices[selectedIndex].Selected = true;
        }
    }
}
