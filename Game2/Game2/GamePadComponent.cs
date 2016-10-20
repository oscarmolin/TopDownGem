using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game2
{
    public class GamePadComponent : Microsoft.Xna.Framework.GameComponent
    {
        public static GamePadState NowState { get; set; }
        public static GamePadState FormerState { get; set; }
        public GamePadComponent(Game game) : base(game)
        {
        }
        public override void Update(GameTime gameTime)
        {
            FormerState = NowState;
            NowState = GamePad.GetState(0);

            base.Update(gameTime);
        }
        public static bool ButtonPressed(Buttons button)
        {
            return NowState.IsButtonDown(button) && !FormerState.IsButtonDown(button);
        }
    }
}
