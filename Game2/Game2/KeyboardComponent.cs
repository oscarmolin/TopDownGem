using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Meny
{
    public class KeyboardComponent : Microsoft.Xna.Framework.GameComponent
    {
        public static KeyboardState CurrentState { get; set; }
        public static KeyboardState LastState { get; set; }
        public KeyboardComponent(Game game) : base(game)
        {  
        }
        public override void Update(GameTime gameTime)
        {
            LastState = CurrentState;
            CurrentState = Keyboard.GetState();

            base.Update(gameTime);
        }
        public static bool KeyPressed(Keys key)
        {
            return CurrentState.IsKeyDown(key) && !LastState.IsKeyDown(key);
        }
    }
}
