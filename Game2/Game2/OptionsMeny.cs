using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using Meny;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Game2
{
    public class OptionsMeny
    {
        
        int selectedIndex;

        Color normal = Color.White;
        Color hlite = Color.Yellow;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        List<MenuChoice> choice;

        OptionsMeny optionsMeny;
        MouseState _previousMouseState;

        Vector2 position;
        float width = 0f;
        float height = 0f;
        private Game game;

        public int SelectedIndex
        {
            get { return SelectedIndex; }
            set
            {
                SelectedIndex = value;
                if (SelectedIndex < 4)
                {
                    SelectedIndex = 0;
                }
                
            }
        }
        public OptionsMeny(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont)

        {
            choice = new List<MenuChoice>();
            choice.Add(new MenuChoice() { Text = "Graphics", IsSelectable = false});
            choice.Add(new MenuChoice() { Text = "1024 x 700", Selected = true, ClickAction = MenuGraphicsClicked });
            choice.Add(new MenuChoice() { Text = "Fullscreen", IsSelectable = false});
            choice.Add(new MenuChoice() { Text = "Fullscreen Enabled", ClickAction = MenuFullClicked });
            choice.Add(new MenuChoice() { Text = "Sound", IsSelectable = false});
            choice.Add(new MenuChoice() { Text = "Sound on", ClickAction = MenuSoundClicked });
            choice.Add(new MenuChoice() { Text = "Exit", ClickAction = MenuExitClicked });
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            MeasureMenu();
        }
        private void MenuGraphicsClicked()
        {
            
        }
        private void MenuFullClicked()
        {

        }
        private void MenuSoundClicked()
        {

        }
        private void MenuExitClicked()
        {
            game.Exit();
        }
        private void PreviousMenuChoice()
        {
            int selectedIndex = choice.IndexOf(choice.First(c => c.Selected));
            choice[selectedIndex].Selected = false;
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = choice.Count - 1;
            choice[selectedIndex].Selected = true;
        }
        private void NextMenuChoice()
        {
            int selectedIndex = choice.IndexOf(choice.First(c => c.Selected));
            choice[selectedIndex].Selected = false;
            selectedIndex++;
            if (selectedIndex >= choice.Count)
                selectedIndex = 0;
            choice[selectedIndex].Selected = true;
        }
        
        private void MeasureMenu()
        {
            height = 0;
            width = 0;
            foreach (MenuChoice item in choice)
            {
                Vector2 size = spriteFont.MeasureString(item.Text);
                if (size.X > width)
                {
                    width = size.X;
                }
                height += spriteFont.LineSpacing + 5;
            }

            position = new Vector2((game.Window.ClientBounds.Width - width) / 2, (game.Window.ClientBounds.Height - height) / 2);
        }
        public void Update(GameTime gameTime)
        {
            if (KeyboardComponent.KeyPressed(Keys.Down) || GamePadComponent.ButtonPressed(Buttons.LeftThumbstickDown) ||
                KeyboardComponent.KeyPressed(Keys.S))
            {
                PreviousMenuChoice();
            }
            if (KeyboardComponent.KeyPressed(Keys.Up) || GamePadComponent.ButtonPressed(Buttons.LeftThumbstickUp) ||
                KeyboardComponent.KeyPressed(Keys.W))
            {
                NextMenuChoice();
            }
            if (KeyboardComponent.KeyPressed(Keys.Enter) || GamePadComponent.ButtonPressed(Buttons.A) ||
                KeyboardComponent.KeyPressed(Keys.Space))
            {

                var selectedChoice = choice.First(c => c.Selected);
                selectedChoice.ClickAction.Invoke();
            }

            var mouseState = Mouse.GetState();
            foreach (var choices in choice)
            {
                if (choices.HitBox.Contains(mouseState.X, mouseState.Y))
                {
                    choice.ForEach(c => c.Selected = false);
                    choices.Selected = true;

                    if (_previousMouseState.LeftButton == ButtonState.Released
                        && mouseState.LeftButton == ButtonState.Pressed)
                        choices.ClickAction.Invoke();
                }
            }
            _previousMouseState = mouseState;
            oldKeyboardState = keyboardState;
        }
        public void Draw(GameTime gameTime)
        {
            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            
            Vector2 location = position;
            Color tint;

            for (int i = 0; i < choice.Count; i++)
            {
                
                if (i == selectedIndex)
                {
                    tint = hlite;
                }
                else
                {
                    tint = normal;
                }
                spriteBatch.DrawString(spriteFont, choice[i].Text, location, tint);
                location.Y += spriteFont.LineSpacing + 5;
            }

            spriteBatch.End();
        }
    }
}
