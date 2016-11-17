using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class ZombieBullet
    {
        Texture2D zombieBullet;
        public Vector2 BulletVelocity;
        public float BulletAngle;
        public Vector2 BulletPosition;
        public bool BulletIsVisible;
        public ZombieBullet(Texture2D newTexture)
        {
            zombieBullet = newTexture;
            BulletIsVisible = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(zombieBullet, BulletPosition, Color.White);
        }
    }
}
