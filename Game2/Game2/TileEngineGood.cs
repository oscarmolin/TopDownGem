﻿using Microsoft.Xna.Framework;
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
        public int[,] RegHitBoxes()
        {
            
            int[,] mapHitBoxes = new int[map.Width,map.Height];
            
            List<List<Rectangle>> rects = new List<List<Rectangle>>();
            rects.Add(new List<Rectangle>());
                foreach(var tile in map.Layers[1].Tiles)
                    if (tile.Gid != 0)
                    { 
                        if (tile.Gid == 27)
                        mapHitBoxes[tile.X, tile.Y] = 2;
                        else                        
                        mapHitBoxes[tile.X, tile.Y] = 1;                    
                    }
                
            
            return (mapHitBoxes);
        }
    }
}
