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
    public class shot
    {
        Texture2D texture;
        public float angle;//make vector
        public Vector2 pos;
        public Rectangle Rectangle;
        public Color[] textureData;
        public shot(Vector2 Pos, float Angle)
        {
            pos = Pos;
            angle = Angle;
        }
        public void Load()
        {
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }
        public void Update()
        {
            Rectangle = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
        }
    }
}
