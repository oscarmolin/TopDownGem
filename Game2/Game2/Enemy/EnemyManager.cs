using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game2
{
    public class EnemyManager
    {
        Texture2D enemy_zombie;
        Texture2D enemy_crippler;
        Texture2D enemy_spitter;
        Texture2D enemy_charger;
        Texture2D enemy_pistolzombie;
        Texture2D enemy_shotgunzombie;
        int round = 1;
        Random ran = new Random();
        ServiceBus bus;
        int frame = 0;

        private List<EnemyStat> _enemies;
        Vector2 spawnpoint = new Vector2();
        List<Rectangle> SpawnPoints = new List<Rectangle>();

        public EnemyManager(ServiceBus Bus)
        {
            _enemies = new List<EnemyStat>();
            bus = Bus;
         
            var e = Enemies.SpawnOne();
            e.Position = new Vector2(80, 80);
            e.Angle = new Vector2(1, 0);

            _enemies.Add(e);
        }

        public void LoadContent(Game Game)
        {
            enemy_zombie = Game.Content.Load<Texture2D>("zombie2_hold");
            enemy_crippler = Game.Content.Load<Texture2D>("zoimbie1_hold");
            enemy_spitter = Game.Content.Load<Texture2D>("robot1_hold");
            enemy_charger = Game.Content.Load<Texture2D>("robot2_hold");
            enemy_pistolzombie = Game.Content.Load<Texture2D>("zombie2_silencer");
            enemy_shotgunzombie = Game.Content.Load<Texture2D>("zombie2_machine");
        }

        public void Update()
        {
            frame++;
            if (frame == 200)
            {
                GetSpawningTiles();
                CalculateZombies();
                frame = 0;
            }
            foreach (var e in _enemies)
            {
                e.Timer++;
                if (e.Timer == 10)
                {
                    e.Timer = 0;
                    var list = bus.PathFinder.MoveFromTo(e.Position, bus.Player.position);
                    if (list == null || list.Count == 0)
                        continue;
                    var node = list.First();
                    var posTo = new Vector2(node.X * 64 + 32, node.Y * 64 + 32);
                    var angle = (posTo - e.Position);
                    angle.Normalize();
                    e.Angle = angle;
                }
                e.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemyStat in _enemies)
            {
                float enemyAngle = (float)(Math.Atan2(enemyStat.Angle.X, -enemyStat.Angle.Y) - MathHelper.PiOver2);

                if (enemyStat.Enemytype == EnemyType.Zombie)
                {
                    spriteBatch.Draw(enemy_zombie, enemyStat.Position, null, Color.White, enemyAngle, new Vector2(enemy_zombie.Width / 2, enemy_zombie.Height / 2), 1.0f, SpriteEffects.None, 0);
                }
                else if (enemyStat.Enemytype == EnemyType.Crippler)
                {
                    spriteBatch.Draw(enemy_crippler, enemyStat.Position, null, Color.White, enemyAngle, new Vector2(enemy_crippler.Width / 2, enemy_crippler.Height / 2), 1.0f, SpriteEffects.None, 0);
                }
                else if (enemyStat.Enemytype == EnemyType.Spitter)
                {
                    spriteBatch.Draw(enemy_spitter, enemyStat.Position, null, Color.White, enemyAngle, new Vector2(enemy_spitter.Width / 2, enemy_spitter.Height / 2), 1.0f, SpriteEffects.None, 0);
                }
                else if (enemyStat.Enemytype == EnemyType.Charger)
                {
                    spriteBatch.Draw(enemy_charger, enemyStat.Position, null, Color.White, enemyAngle, new Vector2(enemy_charger.Width / 2, enemy_charger.Height / 2), 1.0f, SpriteEffects.None, 0);
                }
                else if (enemyStat.Enemytype == EnemyType.PistolZombie)
                {
                    spriteBatch.Draw(enemy_pistolzombie, enemyStat.Position, null, Color.White, enemyAngle, new Vector2(enemy_pistolzombie.Width / 2, enemy_pistolzombie.Height / 2), 1.0f, SpriteEffects.None, 0);
                }
                else if (enemyStat.Enemytype == EnemyType.ShotgunZombie)
                {
                    spriteBatch.Draw(enemy_shotgunzombie, enemyStat.Position, null, Color.White, enemyAngle, new Vector2(enemy_shotgunzombie.Width / 2, enemy_shotgunzombie.Height / 2), 1.0f, SpriteEffects.None, 0);
                }
            }
        }
        public void GetSpawningTiles()
        {
            SpawnPoints= bus.TileEngineG.FindSpawnZones();
           
        }
        public void CalculateZombies()
        {
            for (int i = 0; i< round * 2; i++)
            {
                int random = ran.Next(0, SpawnPoints.Count);
                spawnpoint = new Vector2(SpawnPoints[random].X + 32,
                SpawnPoints[random].Y + 32);
                if(ran.Next(1,3) == 2)
                {
                    var e = Enemies.SpawnOne();
                    _enemies.Add(e);
                }
            }
        }
    }
}
