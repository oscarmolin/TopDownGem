using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace Game2
{
    class TileEngineGood
    {
        //static TmxMap map;
        ServiceBus bus;
        Texture2D tileset;

        int tileWidth;
        int tileHeight;
        int tilesetTilesWide;
        int tilesetTilesHigh;
        public TileEngineGood(ServiceBus serviceBus)
        {
            this.bus = serviceBus;
        }
        public void LoadContent(Game Game)
        {
            tileset = Game.Content.Load<Texture2D>("tilesheet_complete");

            tileWidth = bus.Map.Tilesets[0].TileWidth;
            tileHeight = bus.Map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int l = 0; l < bus.Map.Layers.Count; l++)
            {
                for (var i = 0; i < bus.Map.Layers[l].Tiles.Count; i++)
                {
                    int gid = bus.Map.Layers[l].Tiles[i].Gid;

                    // Empty tile, do nothing
                    if (gid == 0)
                    {

                    }
                    else
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                        float x = (i % bus.Map.Width) * bus.Map.TileWidth;
                        float y = (float)Math.Floor(i / (double)bus.Map.Width) * bus.Map.TileHeight;

                        Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                        spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);
                    }
                }
            }
        }
    }
}
