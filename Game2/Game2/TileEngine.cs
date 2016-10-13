using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game2
{
    class TileEngine : GameComponent
    {
        
        public float MaxZoom { get; set; }
        public float MinZoom { get; set; }

        private float zoom;
        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom > MaxZoom)
                    zoom = MaxZoom;
                if (zoom < MinZoom)
                    zoom = MinZoom;
            }
        }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int[,] Data { get; set; }
        private Texture2D tileMap;
        public Texture2D TileMap
        {
            get { return tileMap; }
            set
            {
                tileMap = value;
                int x, y;
                tiles = TextureTool.Split(tileMap, TileWidth, TileHeight, out x, out y);
            }
        }

        protected Texture2D[] tiles;
        public Vector2 CameraPosition { get; set; }

        private int viewportWidth, viewportHeight;

        public override void Initialize()
        {
            viewportWidth = Game.GraphicsDevice.Viewport.Width;
            viewportHeight = Game.GraphicsDevice.Viewport.Height;
            MaxZoom = 4.0f;
            MinZoom = 0.5f;
            Zoom = 1.0f;

            base.Initialize();
        }

        public TileEngine(Game game) : base(game)
        {
            game.Components.Add(this);
            CameraPosition = Vector2.Zero;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (Data == null || TileMap == null)
                return;

            int screenCenterX = viewportWidth / 2;
            int screenCenterY = viewportHeight / 2;

            float zoomTileWidth = (TileWidth * Zoom);
            float zoomTileHeight = (TileHeight * Zoom);
            Vector2 zoomCameraPosition = CameraPosition * Zoom;
            int startX = (int)((zoomCameraPosition.X - screenCenterX) / zoomTileWidth);
            int startY = (int)((zoomCameraPosition.Y - screenCenterY) / zoomTileHeight);

            int endX = (int)(startX + viewportWidth / zoomTileWidth) + 1;
            int endY = (int)(startY + viewportHeight / zoomTileHeight) + 1;

            if (startX < 0)
                startX = 0;
            if (startY < 0)
                startY = 0;

            Vector2 position = Vector2.Zero;
            int tilesPerLine = TileMap.Width / TileWidth;

            for (int y = startY; y < Data.GetLength(0);y++)//&& y <= endY; y++)
            {
                for (int x = startX; x < Data.GetLength(1);x++ )//&& x <= endX; x++)
                {
                    position.X = (x * zoomTileWidth - zoomCameraPosition.X);//+ screenCenterX);
                    position.Y = (y * zoomTileHeight - zoomCameraPosition.Y); //+ screenCenterY);

                    spriteBatch.Draw(tiles[Data[y, x]],
                       position, null, Color.White, 0f, Vector2.Zero, zoom, SpriteEffects.None, 0f);
                }
            }
        }
        }
    
}