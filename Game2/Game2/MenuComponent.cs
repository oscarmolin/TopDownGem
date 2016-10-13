﻿using System;
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
        SpriteBatch _spriteBatch;
        SpriteFont _normalFont;
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
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _normalFont = Game.Content.Load<SpriteFont>("menuFontNormal");
            _selectedFont = Game.Content.Load<SpriteFont>("menuFontSelected");
            _backgroundColor = Color.White;
            float startY = 0.2f * GraphicsDevice.Viewport.Height;

            foreach (var choice in _choices)
            {
                Vector2 size = _normalFont.MeasureString(choice.Text);
                choice.Y = startY;
                choice.X = GraphicsDevice.Viewport.Width / 2.0f - size.X / 2;
                choice.HitBox = new Rectangle((int)choice.X, (int)choice.Y, (int)size.X, (int)size.Y);
                startY += 70;
            }
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

            base.Update(gameTime);
           
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

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(_backgroundColor);
            _spriteBatch.Begin();
            foreach (var choice in _choices)
            {
                    _spriteBatch.DrawString(choice.Selected ? _selectedFont : _normalFont,
                    choice.Text, new Vector2(choice.X, choice.Y), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);

        }
        
    }
}
