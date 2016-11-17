using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class SpitProjectile
    {
        Texture2D Spitprojectile;
        public Vector2 velocity;
        public float angle;
        public Vector2 pos;
        public bool isVisible;
        public float DistanceTimer;
        public bool hasLanded;
        public SpitProjectile(Texture2D newTexture)
        {
            Spitprojectile = newTexture;
            isVisible = false;
        }
        public void Update(GameTime gameTime)
        {
            pos += velocity;

            DistanceTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (DistanceTimer > 1000)
            {
                hasLanded = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Spitprojectile, pos, null, Color.White, angle, new Vector2(Spitprojectile.Width /2, Spitprojectile.Height/2), 1.0f, SpriteEffects.None, 0);
        }
    }
}
