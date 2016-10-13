using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class shot
    {
        
        public float angle;
        public Vector2 pos;
        public shot(Vector2 Pos, float Angle)
        {
            pos = Pos;
            angle = Angle;
        }
    }
}
