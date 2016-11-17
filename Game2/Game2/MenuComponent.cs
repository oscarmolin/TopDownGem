using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
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
        MouseState _previousMouseState;
        Menu _menu;
        Menu _activeMenu;

        public enum GameState
        {
            MainMenu,
            Playing,
            OptionsMenu
        }

        public static GameState gs;

        public enum Sound
        {
            On,
            Off
        }

        public static Sound SD;

        public enum Full
        {
            on,
            off
        }

        public static Full FL;

        public enum Controll
        {
            Cont,
            Key
        }

        public static Controll CL;

        public enum TwoPlayer
        {
            One,
            Two
        }

        public static TwoPlayer TP;
        public enum SelMap
        {
            Forrest,
            Stone
        }

        public static SelMap SP;

        public enum Graphics
        {
            set1,
            set2,
            set3,
            set4,
            set5
        }

        public static Graphics GR;
        
        public MenuComponent(Game game)
            : base(game)
        {
            _menu = new Menu();
            _activeMenu = _menu;
            var MapMenu = new Menu();
            var optionsMenu = new Menu();
            var graphicsMenu = new Menu();
            var soundMenu = new Menu();
            var TwoPlayers = new Menu();
            var controllMenu = new Menu();
            var returnToMenu = new Menu();
            var exitMenu = new Menu();
            _menu.Items = new List<MenuChoice>
            {
                new MenuChoice(null) { Text = "CoolGAme", IsEnabled = false},
                new MenuChoice(null) { Text = "START", Selected = true, ClickAction = MoveClick, SubMenu = MapMenu, IsVisible = () => CoolGAme.GS != CoolGAme.GameState.Pause },
                new MenuChoice(null) { Text = "PAUSED", ClickAction = MenuStartClicked, IsVisible = () => CoolGAme.GS == CoolGAme.GameState.Pause, IsEnabled = false },
                new MenuChoice(null) { Text = "OPTIONS", ClickAction = MoveClick, SubMenu = optionsMenu},
                new MenuChoice(null) { Text = "EXIT TO MENU", ClickAction = MoveClick, SubMenu = returnToMenu, IsVisible = () => CoolGAme.GS == CoolGAme.GameState.Pause},
                new MenuChoice(null) { Text = "QUIT", ClickAction = MoveClick, SubMenu = exitMenu}
            };
            TwoPlayers.Items = new List<MenuChoice>
            {
                new MenuChoice(MapMenu) { Text = "Start game", Selected = true, ClickAction = MenuStartClicked},
                new MenuChoice(MapMenu) { Text = "Twoplayer off", IsVisible = () => TP == TwoPlayer.One, ClickAction = PlayerNum },
                new MenuChoice(MapMenu) { Text = "Twoplayer on", IsVisible = () => TP == TwoPlayer.Two, ClickAction = PlayerNum },
                new MenuChoice(MapMenu) { Text = "Back", ClickAction = MoveUpClick}
            };
            MapMenu.Items = new List<MenuChoice>
            {
                new MenuChoice(_menu) { Text = "Select your map", IsEnabled = false},
                new MenuChoice(_menu) { Text = "ForrestMap", Selected = true, ClickAction = ForrestMap, SubMenu = TwoPlayers},
                new MenuChoice(_menu) { Text = "StoneMap", ClickAction = StoneMap, SubMenu = TwoPlayers},
                new MenuChoice(_menu) { Text = "Back", ClickAction = MoveUpClick}
            };
            optionsMenu.Items = new List<MenuChoice>
            {
                new MenuChoice(_menu) { Text = "Options Menu", ClickAction = MoveClick, IsEnabled = false},
                new MenuChoice(_menu) { Text = "Grahpics Menu", Selected = true, ClickAction = MoveClick, SubMenu = graphicsMenu},
                new MenuChoice(_menu) { Text = "Controll Menu", ClickAction = MoveClick, SubMenu = controllMenu},
                new MenuChoice(_menu) { Text = "Sound Menu", ClickAction = MoveClick, SubMenu = soundMenu},
                new MenuChoice(_menu) { Text = "Back to Main", ClickAction = MoveUpClick}
            };
            graphicsMenu.Items = new List<MenuChoice>
            {
                new MenuChoice(optionsMenu) { Text = "Graphics Menu", IsEnabled = false},
                new MenuChoice(optionsMenu) { Text = "Fullscreen On", Selected = true, IsVisible = () => FL == Full.on, ClickAction = FullMenu },
                new MenuChoice(optionsMenu) { Text = "Fullscreen Off", IsVisible = () => FL == Full.off, ClickAction = FullMenu },
                new MenuChoice(optionsMenu) { Text = "1920 x 1080", IsVisible = () => GR == Graphics.set1, ClickAction = Grafik},
                new MenuChoice(optionsMenu) { Text = "1024 x 700", IsVisible = () => GR == Graphics.set2, ClickAction = Grafik},
                new MenuChoice(optionsMenu) { Text = "1366 x 768", IsVisible = () => GR == Graphics.set3, ClickAction = Grafik},
                new MenuChoice(optionsMenu) { Text = "1440 x 900", IsVisible = () => GR == Graphics.set4, ClickAction = Grafik},
                new MenuChoice(optionsMenu) { Text = "1600 x 900", IsVisible = () => GR == Graphics.set5, ClickAction = Grafik},
                new MenuChoice(optionsMenu) { Text = "Back to Options", ClickAction = MoveUpClick}
            };
            soundMenu.Items = new List<MenuChoice>
            {
                new MenuChoice(optionsMenu) { Text = "Sound Menu", IsEnabled = false},
                new MenuChoice(optionsMenu) { Text = "Sound On", Selected = true, IsVisible = () => SD == Sound.On,ClickAction = SoundMenu },
                new MenuChoice(optionsMenu) { Text = "Sound Off", IsVisible = () => SD == Sound.Off, ClickAction = SoundMenu },
                new MenuChoice(optionsMenu) { Text = "Back to Options", ClickAction = MoveUpClick}
            };
            controllMenu.Items = new List<MenuChoice>
            {
                new MenuChoice(optionsMenu) { Text = "Controll Menu", IsEnabled = false},
                new MenuChoice(optionsMenu) { Text = "Keyboard Active", Selected = true, IsVisible = () => CL == Controll.Key, ClickAction = ControlMenu },
                new MenuChoice(optionsMenu) { Text = "Controll Active", IsVisible = () => CL == Controll.Cont, ClickAction = ControlMenu },
                new MenuChoice(optionsMenu) { Text = "Back to Options", ClickAction = MoveUpClick}
            };
            exitMenu.Items = new List<MenuChoice>
            {
                new MenuChoice(_menu) { Text = "Are you sure?", IsEnabled = false},
                new MenuChoice(_menu) { Text = "No", Selected = true, ClickAction = MoveUpClick},
                new MenuChoice(_menu) { Text = "Yes", ClickAction = MenuQuitClicked}
            };
            returnToMenu.Items = new List<MenuChoice>
            {
                new MenuChoice(_menu) { Text = "Are you sure?", IsEnabled = false},
                new MenuChoice(_menu) { Text = "No", Selected = true, ClickAction = MoveUpClick},
                new MenuChoice(_menu) { Text = "Yes", ClickAction = PausMenuQuitClicked}
            };
        }
        public override void Initialize()
        {
            CL = Controll.Key;
            gs = GameState.MainMenu;
            SD = Sound.On;
            FL = Full.off;
            TP = TwoPlayer.One;
            SP = SelMap.Forrest;
            GR = Graphics.set1;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _normalFont = Game.Content.Load<SpriteFont>("menuFontNormal");
            _selectedFont = Game.Content.Load<SpriteFont>("menuFontSelected");
            _previousMouseState = Mouse.GetState();
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            switch (gs)
            {
                case GameState.MainMenu:
                    if (KeyboardComponent.KeyPressed(Keys.Escape))
                    {
                        var selectedChoice = _activeMenu.Items.First(c => c.Selected);
                        if (selectedChoice.ParentMenu != null)
                            _activeMenu = selectedChoice.ParentMenu;
                    }
                    if (KeyboardComponent.KeyPressed(Keys.Down) || GamePadComponent.ButtonPressed(Buttons.LeftThumbstickDown) || KeyboardComponent.KeyPressed(Keys.S))
                    {
                        NextMenuChoice();
                    }
                    if (KeyboardComponent.KeyPressed(Keys.Up) || GamePadComponent.ButtonPressed(Buttons.LeftThumbstickUp) || KeyboardComponent.KeyPressed(Keys.W))
                    {
                        PreviousMenuChoice();
                    }
                    if (KeyboardComponent.KeyPressed(Keys.Enter) || GamePadComponent.ButtonPressed(Buttons.A) || KeyboardComponent.KeyPressed(Keys.Space))
                    {
                        var selectedChoice = _activeMenu.Items.First(c => c.Selected);
                        selectedChoice.ClickAction.Invoke();

                        if (selectedChoice.SubMenu != null)
                        _activeMenu = selectedChoice.SubMenu;
                    }
                    var mouseState = Mouse.GetState();
                    foreach (var choice in _activeMenu.Items)
                    {
                        if (choice.HitBox.Contains(mouseState.X, mouseState.Y) && choice.IsEnabled && choice.IsVisible())
                        {
                            _activeMenu.Items.ForEach(c => c.Selected = false);
                            choice.Selected = true;
                            if (_previousMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed && choice.IsVisible())
                            {
                                choice.ClickAction.Invoke();
                                if (choice.SubMenu != null)
                                _activeMenu = choice.SubMenu;
                                break;
                            }
                        }
                    }
                    _previousMouseState = mouseState;
                    float startY = 0.2f*GraphicsDevice.Viewport.Height;
                    foreach (var choice in _activeMenu.Items)
                    {
                        if (!choice.IsVisible())
                            continue;

                        Vector2 size = _normalFont.MeasureString(choice.Text);
                        choice.Y = startY;
                        choice.X = GraphicsDevice.Viewport.Width/2.0f - size.X/2;
                        choice.HitBox = new Rectangle((int) choice.X, (int) choice.Y, (int) size.X, (int) size.Y - 15);
                        startY += 70;
                    }
                    break;
                case GameState.Playing:
                    break;
            }
            base.Update(gameTime);
        }
        private void PreviousMenuChoice()
        {
            int selectedIndex = _activeMenu.Items.IndexOf(_activeMenu.Items.First(c => c.Selected));
            _activeMenu.Items[selectedIndex].Selected = false;
            for (int i = 0; i < _activeMenu.Items.Count; i++)
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = _activeMenu.Items.Count - 1;
                if (_activeMenu.Items[selectedIndex].IsVisible() && _activeMenu.Items[selectedIndex].IsEnabled)
                {
                    _activeMenu.Items[selectedIndex].Selected = true;
                    break;
                }
            }
        }
        private void NextMenuChoice()
        {
            int selectedIndex = _activeMenu.Items.IndexOf(_activeMenu.Items.First(c => c.Selected));
            _activeMenu.Items[selectedIndex].Selected = false;
            for (int i = 0; i < _activeMenu.Items.Count; i++)
            {
                selectedIndex++;
                if (selectedIndex >= _activeMenu.Items.Count)
                    selectedIndex = 0;
                if (_activeMenu.Items[selectedIndex].IsVisible() && _activeMenu.Items[selectedIndex].IsEnabled)
                {
                    _activeMenu.Items[selectedIndex].Selected = true;
                    break;
                }
            }
        }
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            foreach (var choice in _activeMenu.Items)
            {
                if (!choice.IsVisible())
                    continue;
                _spriteBatch.DrawString(choice.Selected ? _selectedFont : _normalFont, choice.Text, new Vector2(choice.X, choice.Y), Color.White);
            }
            _spriteBatch.End();
        }
        #region Menu Clickers
        private void MenuStartClicked()
        {
            CoolGAme.GS = CoolGAme.GameState.Playing;
            gs = GameState.Playing;
            _activeMenu = _menu;
        }
        private void ForrestMap()
        {
            SP = SelMap.Forrest;
            CoolGAme g = Game as CoolGAme;
            g.LoadMap(SP);
        }
        private void StoneMap()
        {
            SP = SelMap.Stone;
            CoolGAme g = Game as CoolGAme;
            g.LoadMap(SP);
        }
        private void MoveUpClick()
        {
            var selectedChoice = _activeMenu.Items.First(c => c.Selected);
            if (selectedChoice.ParentMenu != null)
            _activeMenu = selectedChoice.ParentMenu;
        }
        private void MoveClick()
        {
            // Krav på att en ClickAction ska tillkallas. Då ingen behövs kallas en som inte gör något.
        }
        private void SoundMenu()
        {
            SD = (SD == Sound.On) ? Sound.Off : Sound.On;
        }
        private void ControlMenu()
        {
            CL = (CL == Controll.Cont) ? Controll.Key : Controll.Cont;
        }
        private void PlayerNum()
        {
            TP = (TP == TwoPlayer.One) ? TwoPlayer.Two : TwoPlayer.One;
        }
        private void FullMenu()
        {
            FL = (FL == Full.off) ? Full.on : Full.off;
            var coolGame = (CoolGAme) Game;
            coolGame.Graphics.IsFullScreen = FL == Full.on;
            coolGame.Graphics.ApplyChanges();
        }

        private void Grafik()
        {
            if (GR == Graphics.set1)
            {
                GR = Graphics.set2;
            }
            else if (GR == Graphics.set2)
            {
                GR = Graphics.set3;
            }
            else if (GR == Graphics.set3)
            {
                GR = Graphics.set4;
            }
            else if (GR == Graphics.set4)
            {
                GR = Graphics.set5;
            }
            else if (GR == Graphics.set5)
            {
                GR = Graphics.set1;
            }
            (Game as CoolGAme).Grafitti();
        }
        private void PausMenuQuitClicked()
        {
            CoolGAme.GS = CoolGAme.GameState.Start;
            gs = GameState.MainMenu;
            (Game as CoolGAme).Restart();
            var selectedChoice = _activeMenu.Items.First(c => c.Selected);
            if (selectedChoice.ParentMenu != null)
                _activeMenu = selectedChoice.ParentMenu;
        }
        private void MenuQuitClicked()
        {
            Game.Exit();
        }
        #endregion
    }
}
