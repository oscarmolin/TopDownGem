using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using TiledSharp;
namespace Game2
{
    enum Controller
    { 
        Controller1,
        Controller2,
        Controller3,
        controller4,
        Keyboard
        
    }
    class Player
    {
        SoundEffect effect;
        public Controller controller;
        public Texture2D texture;
        public Vector2 position { get; private set; }
        Vector2 velocity;
        float speed;
        float maxspeed;
        float acc;
        float sprintmaxspeed;
        bool accelerating;
        float angle;
        public List<shot> shots;
        float volume = 1.0f;
        float pitch = -1.0f;
        float pan = 0.0f;
        Vector2 anglevector ;
        Vector2 prevangelvector;
        Rectangle  rect;
        List<Rectangle> map;
        bool colided= false;

        public float X { get { return position.X; } }
        public float Y{ get { return position.Y; } }
        public Player( Vector2 Position,Controller Controler, float maxspeed,List<Rectangle> Map)
        {
            this.maxspeed = maxspeed;
            this.position = Position;
            this.controller = Controler;
            shots = new List<shot>();
            acc = 0.5f;
            anglevector = new Vector2();
            map = Map;
            
        }
        public void LoadContent(Game game, string texture)
        {
            
            this.texture = game.Content.Load<Texture2D>(texture);
            effect = game.Content.Load<SoundEffect>("Pew");
            rect = new Rectangle((int)position.X,(int)position.Y,this.texture.Width,this.texture.Height);
        }
        public void  Update(Vector2 mousePosition,KeyboardState ks)
        {

           


            if (controller == Controller.Keyboard)
            {
                accelerating = false;
            if(anglevector != new Vector2())
            prevangelvector = anglevector;
            anglevector = new Vector2();
                
                angle = (float)Math.Atan2(position.Y - mousePosition.Y, position.X - mousePosition.X);

                if (ks.IsKeyDown(Keys.W))
                {
                    anglevector += new Vector2(0, -1);
                    accelerating = true;
                }

                if (ks.IsKeyDown(Keys.A))
                {
                    anglevector += new Vector2(-1, 0);
                    accelerating = true;
                }
                if (ks.IsKeyDown(Keys.S))
                {
                    anglevector += new Vector2(0, 1);
                    accelerating = true;
                }
                if (ks.IsKeyDown(Keys.D))
                {
                    anglevector += new Vector2(1, 0);
                    accelerating = true;
                }
                if(accelerating)
                {
                    if (speed >= maxspeed)
                    speed = maxspeed;
                    else
                    speed += acc;
                   
                }
                else
                {
                    if (speed < 0.5f)
                        speed = 0;
                    else
                        speed -= 2 * acc;
                }


               

                if (anglevector != new Vector2())
                {
                    anglevector.Normalize();
                    velocity = speed * anglevector;
                }
                else
                    velocity = speed * prevangelvector;
                if (ks.IsKeyDown(Keys.Space))
                {
                    shots.Add(new shot(position, angle));

                    effect.Play(volume, pitch, pan);
                }

            }
            else
            {
                GamePadState gs = GamePad.GetState((int)controller);

                if (gs.ThumbSticks.Left != new Vector2(0, 0))
                {
                    if (speed > maxspeed)
                        speed = maxspeed;
                    else
                        speed += acc;
                }
                else
                {
                   if (speed < 0.5f)
                        speed = 0;
                   else 
                        speed -= 0.5f * acc;
                }
                 velocity = speed * new Vector2(gs.ThumbSticks.Left.X, gs.ThumbSticks.Left.Y * -1);
                if (gs.ThumbSticks.Right != new Vector2(0, 0))
                    angle = (float)Math.Atan2(gs.ThumbSticks.Right.X, gs.ThumbSticks.Right.Y) + (float)Math.PI / 2;
                if (gs.IsButtonDown(Buttons.RightTrigger))
                {
                    shots.Add(new shot(position, angle));

                    effect.Play(volume, pitch, pan);
                }
            }


            rect = new Rectangle((int)position.X + (int)velocity.X  -(int)(texture.Width/2), (int)position.Y + (int)velocity.Y - (int)(texture.Height/2), texture.Width, texture.Height);
            for (int i = 0; i < map.Count; i++)
            {
                if (rect.Intersects(map[i]))
                {
                    colided = true;
                    break;
                }
             }
            if(!colided)
            position += velocity;
            colided = false;
        }
        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, null, Color.White, angle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
        }


    }
}
