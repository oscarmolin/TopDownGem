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
        static TmxMap map;
        Texture2D tileset;

        int tileWidth;
        int tileHeight;
        int tilesetTilesWide;
        int tilesetTilesHigh;
        public TileEngineGood(TmxMap Map)
        {
            map = Map;
        }
        public void LoadContent(Game Game)
        {
            tileset = Game.Content.Load<Texture2D>("tilesheet_complete");

            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int l = 0; l < map.Layers.Count; l++)
            {


                for (var i = 0; i < map.Layers[l].Tiles.Count; i++)
                {
                    int gid = map.Layers[l].Tiles[i].Gid;

                    // Empty tile, do nothing
                    if (gid == 0)
                    {

                    }
                    else
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                        float x = (i % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                        Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                        spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);
                    }
                }
            }
        }
        public Tuple<List<Rectangle>,List<Rectangle>>  RegHitBoxes()
        {
            List<Rectangle> playerMapHitBoxes = new List<Rectangle>();
            List<Rectangle> mapHitBoxes = new List<Rectangle>();
            //for (int y = 0; y < length; y++)
            //{
            List<List<Rectangle>> rects = new List<List<Rectangle>>();
            rects.Add(new List<Rectangle>());
                for (var x = 0; x < map.Layers[1].Tiles.Count; x++)
                {
                    int gid = map.Layers[1].Tiles[x].Gid;

                    // Empty tile, do nothing
                    if (gid == 0)
                    {

                    }
                    else
                    {
                        float X = (x % map.Width) * map.TileWidth;
                        float Y = (float)Math.Floor(x / (double)map.Width) * map.TileHeight;

                        playerMapHitBoxes.Add(new Rectangle((int)X, (int)Y, 64, 64));

                        if (gid == 27)
                        {

                        }
                        else
                        {
                            mapHitBoxes.Add(new Rectangle((int)X, (int)Y, 64, 64));
                        }
                    }
                }
            //}
            return (new Tuple<List<Rectangle>, List<Rectangle>>(playerMapHitBoxes,mapHitBoxes));
        }
    }
}
