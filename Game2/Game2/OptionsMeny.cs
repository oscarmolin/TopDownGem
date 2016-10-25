//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Net.Mime;
//using Meny;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//namespace Game2
//{
//    public class OptionsMeny : Microsoft.Xna.Framework.DrawableGameComponent
//    {
//        int selectedIndex;

//        Color normal = Color.White;
//        Color hlite = Color.Yellow;

//        KeyboardState keyboardState;
//        KeyboardState oldKeyboardState;

//        SpriteBatch spriteBatch;
//        SpriteFont spriteFont;

//        SpriteFont selectFont;
//        SpriteFont NormalFont;

//        List<MenuChoice> choice;

//        OptionsMeny optionsMeny;
//        MouseState _previousMouseState;

//        Vector2 position;
//        float width = 0f;
//        float height = 0f;
//        private Game game;

//        public enum GameState
//        {
//            OptMenu, Playing
//        }
//        public static GameState OPt;
//        public int SelectedIndex
//        {
//            get { return selectedIndex; }
//            set
//            {
//                SelectedIndex = value;
//                if (SelectedIndex < 4)
//                {
//                    SelectedIndex = 0;
//                }
//            }
//        }
//        public OptionsMeny(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont) : base(game)

//        { 
//            choice = new List<MenuChoice>();
//            choice.Add(new MenuChoice { Text = "Graphics", IsVisible = false});
//            choice.Add(new MenuChoice { Text = "1024 x 700", Selected = true, ClickAction = MenuGraphicsClicked });
//            choice.Add(new MenuChoice { Text = "Fullscreen", IsVisible = false});
//            choice.Add(new MenuChoice { Text = "Fullscreen Disabled", ClickAction = MenuFullClicked });
//            choice.Add(new MenuChoice { Text = "Sound", IsVisible = false});
//            choice.Add(new MenuChoice { Text = "Sound on", ClickAction = MenuSoundClicked });
//            choice.Add(new MenuChoice { Text = "Exit menu", ClickAction = MenuExitClicked });
//            NormalFont = game.Content.Load<SpriteFont>("menuFontNormal");
//            selectFont = game.Content.Load<SpriteFont>("menuFontSelected");
//            this.game = game;
//            this.spriteBatch = spriteBatch;
//            this.spriteFont = spriteFont;
//            MeasureMenu();
//            OPt = GameState.Playing;   
//        }

//        protected override void LoadContent()
//        {
//            spriteBatch = new SpriteBatch(GraphicsDevice);
//            NormalFont = Game.Content.Load<SpriteFont>("menuFontNormal");
//            selectFont = Game.Content.Load<SpriteFont>("menuFontSelected");
//            float startY = 0.2f * GraphicsDevice.Viewport.Height;

//            foreach (var choices in choice)
//            {
//                Vector2 size = NormalFont.MeasureString(choices.Text);
//                choices.Y = startY;
//                choices.X = GraphicsDevice.Viewport.Width / 2.0f - size.X / 2;
//                choices.HitBox = new Rectangle((int)choices.X, (int)choices.Y, (int)size.X, (int)size.Y);
//                startY += 70;
//            }
//        }

//        private void MenuGraphicsClicked()
//        {
            
//        }
//        private void MenuFullClicked()
//        {
            
//        }
//        private void MenuSoundClicked()
//        {

//        }
//        private void MenuExitClicked()
//        {
//            OPt = GameState.Playing;
//            MenuComponent.gs = MenuComponent.GameState.MainMenu;
//        }
//        private void PreviousMenuChoice()
//        {
//            selectedIndex = choice.IndexOf(choice.First(c => c.Selected));
//            choice[selectedIndex].Selected = false;
//            selectedIndex--;
//            if (selectedIndex < 0)
//                selectedIndex = choice.Count - 1;
//            choice[selectedIndex].Selected = true;
//        }
//        private void NextMenuChoice()
//        {
//            selectedIndex = choice.IndexOf(choice.First(c => c.Selected));
//            choice[selectedIndex].Selected = false;
//            selectedIndex++;
//            if (selectedIndex >= choice.Count)
//                selectedIndex = 0;
//            choice[selectedIndex].Selected = true;
//        }
        
//        private void MeasureMenu()
//        {
//            height = 0;
//            width = 0;
//            foreach (MenuChoice item in choice)
//            {
//                Vector2 size = spriteFont.MeasureString(item.Text);
//                if (size.X > width)
//                {
//                    width = size.X;
//                }
//                height += spriteFont.LineSpacing + 5;
//            }

//            position = new Vector2((game.Window.ClientBounds.Width - width) / 2, (game.Window.ClientBounds.Height - height) / 2);
//        }
//        public void Update(GameTime gameTime)
//        {
//            switch (OPt)
//            {
//                case GameState.OptMenu:
//                    if (KeyboardComponent.KeyPressed(Keys.Down) || GamePadComponent.ButtonPressed(Buttons.LeftThumbstickDown) || KeyboardComponent.KeyPressed(Keys.S))
//                    {
//                        PreviousMenuChoice();
//                    }
//                    if (KeyboardComponent.KeyPressed(Keys.Up) || GamePadComponent.ButtonPressed(Buttons.LeftThumbstickUp) || KeyboardComponent.KeyPressed(Keys.W))
//                    {

//                        NextMenuChoice();
//                    }
//                    if (KeyboardComponent.KeyPressed(Keys.Enter) || GamePadComponent.ButtonPressed(Buttons.A) || KeyboardComponent.KeyPressed(Keys.Space))
//                    {

//                        var selectedChoice = choice.First(c => c.Selected);
//                        selectedChoice.ClickAction.Invoke();
//                    }
//                    var mouseState = Mouse.GetState();
//                    foreach (var choices in choice)
//                    {
//                        if (choices.HitBox.Contains(mouseState.X, mouseState.Y))
//                        {
//                            choice.ForEach(c => c.Selected = false);
//                            choices.Selected = true;

//                            if (_previousMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
//                                choices.ClickAction.Invoke();
//                        }
//                    }
//                    _previousMouseState = mouseState;
//                    oldKeyboardState = keyboardState;
//                    break;
//                case GameState.Playing:
//                    break;
//            }
            
//        }
//        public void Draw(GameTime gameTime)
//        {

//            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
//            Vector2 location = position;
//            Color tint;
            
//                for (int i = 0; i < choice.Count; i++)
//            {
//                if (i == selectedIndex)
//                {
//                    tint = hlite;
//                }
//                else
//                {
//                    tint = normal;
//                }
                
//                    spriteBatch.DrawString(spriteFont, choice[i].Text, location, tint);
//                    location.Y += spriteFont.LineSpacing + 5;
//                }
            
//            spriteBatch.End();
//            //spriteBatch.Begin();
//            ////foreach (var choices in choice)
//            ////{
                
//            //    //spriteBatch.DrawString(choices.Selected ? selectFont : NormalFont,
//            //    //    choices.Text, position, Color.White);
//            //}
            
//            //spriteBatch.End();
            

//        }
//    }
//}
