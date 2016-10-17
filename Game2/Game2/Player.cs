using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TiledSharp;
namespace Game2
{
    enum Controler
    { 
        Controler1,
        Controler2,
        Controler3,
        Keyboard,
    }
    class Player
    {
        Controler controler;
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        float speed;
        float angle;

        public Player(Texture2D Texture, Vector2 Position,Controler Controler)
        {
            this.texture = Texture;
            this.position = Position;
            this.controler = Controler;
        }
        public void  Update(Vector2 mousePosition,KeyboardState ks, GamePadState gs)
        {          

           

            if (controler == Controler.Keyboard)
            {

                if (ks.IsKeyDown(Keys.W))
                    position += velocity;
                //if (ks.IsKeyDown(Keys.A))
                //    PlayerPos += new Vector2(-pspeed, 0);
                if (ks.IsKeyDown(Keys.S))
                    position -= velocity;
                //if (ks.IsKeyDown(Keys.D))
                //    PlayerPos += new Vector2(pspeed, 0);

                angle = (float)Math.Atan2(position.Y - mousePosition.Y, position.X - mousePosition.X);
            }
            else
            {
                
                if (gs.ThumbSticks.Left != new Vector2(0, 0))
                    position += velocity.Length() * new Vector2(gs.ThumbSticks.Left.X, gs.ThumbSticks.Left.Y * -1);

                if (gs.ThumbSticks.Right != new Vector2(0, 0))
                    angle = (float)Math.Atan2(gs.ThumbSticks.Right.X, gs.ThumbSticks.Right.Y) + (float)Math.PI / 2;
            }
        }

    }
}
