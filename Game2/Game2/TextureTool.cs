using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    class TextureTool
    {
        public static Texture2D[] Split(Texture2D original, int partWidth,
            int partHeight, out int xCount, out int yCount)
        {
            yCount = original.Height / partHeight;
            xCount = original.Width / partWidth;
            Texture2D[] r = new Texture2D[xCount * yCount];
            int dataPerPart = partWidth * partHeight;

            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            int index = 0;
            for (int y = 0; y < yCount * partHeight; y += partHeight)
                for (int x = 0; x < xCount * partWidth; x += partWidth)
                {
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    Color[] partData = new Color[dataPerPart];

                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;

                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }

                    part.SetData<Color>(partData);
                    r[index++] = part;
                }
            return r;
        }
    }
}