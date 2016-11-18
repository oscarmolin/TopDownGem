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
        Texture2D spit_projectile;
        Texture2D zombie_bullet;
        Texture2D gfx_spitterPool;
        Texture2D health_Gauge;
        Texture2D health_Bar;
        int round = 1;
        int MaxHealth;
        int Resistance;
        int CollisionDamage;
        Random ran = new Random();
        ServiceBus bus;
        int frame = 0;

        public Color[] textureDataZombie;
        public Color[] textureDataCrippler;
        public Color[] textureDataSpitter;
        public Color[] textureDataCharger;
        public Color[] textureDataPistolZombie;
        public Color[] textureDataShotgunZombie;

        shot shots;
        private List<EnemyStat> _enemies;
        List<Rectangle> SpawnPoints = new List<Rectangle>();
        List<SpitProjectile> SpitProjectile = new List<SpitProjectile>();
        List<ZombieBullet> ZombieBullet = new List<ZombieBullet>();
        List<AcidPool> AcidPool = new List<AcidPool>();
        List<shot> playerShot = new List<shot>();
        Rectangle RectangleBar;
        Rectangle RectangleGauge;

        public EnemyManager(ServiceBus Bus)
        {
            _enemies = new List<EnemyStat>();
            bus = Bus;
         
            var e = Enemies.SpawnOne(new Vector2());
            e.Position = new Vector2(80, 80);
            e.Angle = new Vector2(1, 0);

            _enemies.Add(e);
            MaxHealth = e.Health;
            Resistance = e.Resistance;
            CollisionDamage = e.CollisionDamage;
        }

        public void LoadContent(Game Game)
        {
            enemy_zombie = Game.Content.Load<Texture2D>("zombie2_hold");
            enemy_crippler = Game.Content.Load<Texture2D>("zoimbie1_hold");
            enemy_spitter = Game.Content.Load<Texture2D>("robot1_hold");
            enemy_charger = Game.Content.Load<Texture2D>("robot2_hold");
            enemy_pistolzombie = Game.Content.Load<Texture2D>("zombie2_silencer");
            enemy_shotgunzombie = Game.Content.Load<Texture2D>("zombie2_machine");
            spit_projectile = Game.Content.Load<Texture2D>("Spitter projectile");
            zombie_bullet = Game.Content.Load<Texture2D>("ZombieBullet");
            gfx_spitterPool = Game.Content.Load<Texture2D>("Spitter Pool 1");
            health_Bar = Game.Content.Load<Texture2D>("GreenHealthBar");
            health_Gauge = Game.Content.Load<Texture2D>("RedHealthBar");


            textureDataZombie = new Color[enemy_zombie.Width * enemy_zombie.Height];
            textureDataCrippler = new Color[enemy_crippler.Width * enemy_crippler.Height];
            textureDataSpitter = new Color[enemy_spitter.Width * enemy_spitter.Height];
            textureDataCharger = new Color[enemy_charger.Width * enemy_charger.Height];
            textureDataPistolZombie = new Color[enemy_pistolzombie.Width * enemy_pistolzombie.Height];
            textureDataShotgunZombie = new Color[enemy_shotgunzombie.Width * enemy_shotgunzombie.Height];

            enemy_zombie.GetData(textureDataZombie);
            enemy_crippler.GetData(textureDataCrippler);
            enemy_spitter.GetData(textureDataSpitter);
            enemy_charger.GetData(textureDataCharger);
            enemy_pistolzombie.GetData(textureDataPistolZombie);
            enemy_shotgunzombie.GetData(textureDataShotgunZombie);
        }

        public void Update(GameTime gametime)
        {
            frame++;
            if (frame == 1000)
            {
                frame = 0;
                GetSpawningTiles();
                CalculateZombies();
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
            foreach (var enemyStat in _enemies)
            {
                if (enemyStat.Enemytype == EnemyType.Spitter)
                {
                    enemyStat.ShootTimer++;
                    if (enemyStat.ShootTimer >= 150)
                    {
                        enemyStat.ShootTimer = 0;
                        Spit(enemyStat);
                    }
                    UpdateSpit(gametime);
                }
                else if (enemyStat.Enemytype == EnemyType.PistolZombie)
                {
                    enemyStat.ShootTimer++;
                    if(enemyStat.ShootTimer >= 150)
                    {
                        enemyStat.ShootTimer = 0;
                        Shoot(enemyStat);
                    }
                }
                else if (enemyStat.Enemytype == EnemyType.ShotgunZombie)
                {
                    enemyStat.ShootTimer++;
                    if(enemyStat.ShootTimer >= 150)
                    {
                        enemyStat.ShootTimer = 0;
                        Shoot(enemyStat);
                        Shoot(enemyStat);
                        Shoot(enemyStat);
                        Shoot(enemyStat);
                        Shoot(enemyStat);
                        Shoot(enemyStat);
                    }
                }
            }
            foreach(AcidPool Pool in AcidPool)
            {
                Pool.Update(gametime);
            }
            AcidPool.RemoveAll(z => z.Decay);
            UpdateBullet();
        }
        public void UpdateBullet()
        {
            foreach(ZombieBullet bullet in ZombieBullet)
            {
                bullet.BulletPosition += bullet.BulletVelocity;
                if(bullet.BulletPosition.X < 0)
                {
                    bullet.BulletIsVisible = false;
                }
            }
            ZombieBullet.RemoveAll(z => !z.BulletIsVisible);
        }
        public void UpdateSpit(GameTime time)
        {
            foreach(SpitProjectile projectile in SpitProjectile)
            {
                projectile.Update(time);
                if (projectile.pos.X < 0)
                {
                    projectile.isVisible = false;
                }
            }
            foreach (SpitProjectile projectile in SpitProjectile.Where(s => s.hasLanded))
            {
                AcidPool.Add(new AcidPool(gfx_spitterPool, 2, 4) { location = projectile.pos });
            }
            SpitProjectile.RemoveAll(z => !z.isVisible || z.hasLanded);
        }
        public void Shoot(EnemyStat enemyStat)
        {
            ZombieBullet newShoot = new ZombieBullet(zombie_bullet);

            Vector2 direction = bus.Player.position - enemyStat.Position;
            direction.Normalize();

            var d = (ran.NextDouble() - 0.5) / 10.0;
            direction = Vector2.Transform(direction, Matrix.CreateRotationZ((float)d));

            direction = direction * 10;

            newShoot.BulletVelocity = direction * 2;
            newShoot.BulletPosition = new Vector2(enemyStat.Position.X + newShoot.BulletVelocity.X, enemyStat.Position.Y + (enemy_pistolzombie.Height / 2) - (zombie_bullet.Height / 2));

            newShoot.BulletIsVisible = true;
            ZombieBullet.Add(newShoot);
        }

        public void Spit(EnemyStat enemyStat)
        {
            SpitProjectile newSpit = new SpitProjectile(spit_projectile);

            Vector2 direction = bus.Player.position - enemyStat.Position;
            direction.Normalize();
            direction = direction * 10;

            newSpit.velocity = direction / 2;
            newSpit.angle = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.Pi;

            newSpit.pos = new Vector2(enemyStat.Position.X + newSpit.velocity.X, enemyStat.Position.Y + (enemy_spitter.Height / 2) - (spit_projectile.Height / 2));

            newSpit.isVisible = true;
            SpitProjectile.Add(newSpit);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AcidPool pool in AcidPool)
            {
                pool.Draw(spriteBatch);
            }
            foreach (SpitProjectile projectile in SpitProjectile)
            {
                projectile.Draw(spriteBatch);
            }
            foreach(ZombieBullet bullet in ZombieBullet)
            {
                bullet.Draw(spriteBatch);
            }
            foreach (var enemyStat in _enemies)
            {
                float enemyAngle = (float)(Math.Atan2(enemyStat.Angle.X, -enemyStat.Angle.Y) - MathHelper.PiOver2);
                if (enemyStat.Health > 0)
                {
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
                    RectangleBar = new Rectangle(((int)enemyStat.Position.X - 25), ((int)enemyStat.Position.Y - 35), enemyStat.Health / MaxHealth * 50, 2);
                    RectangleGauge = new Rectangle(((int)enemyStat.Position.X - 25), ((int)enemyStat.Position.Y - 35), 50, 2);
                    spriteBatch.Draw(health_Gauge, RectangleGauge, Color.White);
                    spriteBatch.Draw(health_Bar, RectangleBar, Color.White);
                }
            }
        }
        static bool IntersectsPixel(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
        {
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colour1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                    Color colour2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                    if (colour1.A != 0 && colour2.A != 0)
                        return true;
                }
            }

            return false;
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
                var e = Enemies.SpawnOne(new Vector2(SpawnPoints[random].X,SpawnPoints[random].Y));
                _enemies.Add(e);
            }
        }
    }
}
