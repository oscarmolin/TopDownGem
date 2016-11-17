using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class AcidPool
    {
        public Vector2 location { get; set; }
        public Texture2D gfx_SpitterPool { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool Decay { get; set; }
        private int AcidTimer;
        private int currentFrame;
        private int totalFrame;

        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 150;

        public AcidPool(Texture2D gfx_spitterPool, int rows, int columns)
        {
            gfx_SpitterPool = gfx_spitterPool;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrame = Rows * Columns;
        }
        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;

                currentFrame++;
                timeSinceLastFrame = 0;
                if (currentFrame == totalFrame)
                {
                    currentFrame = 0;
                }
            }
            AcidTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (AcidTimer >= 3000)
            {
                Decay = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = gfx_SpitterPool.Width / Columns;
            int height = gfx_SpitterPool.Height / Rows;
            int row = (int)(currentFrame / Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Draw(gfx_SpitterPool, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
