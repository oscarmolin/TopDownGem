using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2
{
    public class EnemyStat
    {
        public int EnemyId { get; set; }
        public int Health { get; set; }
        public int Rescitense { get; set; }
        public float Speed { get; set; }
        public string Name { get; set; }
        public EnemyType Enemytype { get; set; }
        public int Timer { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Angle { get; set; }

        public void Update()
        {
            Position += Angle * Speed;
        }
    }
    public enum EnemyType
    {
        Zombie,
        Crippler,
        Spitter,
        Charger,
        PistolZombie,
        ShotgunZombie
    }
    class Enemies
    {
        private static Random rnd = new Random();
        public static EnemyStat SpawnOne(Vector2 position)
        {
            return SpawnOne((EnemyType)rnd.Next(6),position);
        } 
        public static EnemyStat SpawnOne(EnemyType type,Vector2 position)
        {
            switch (type)
            {
                case EnemyType.Zombie:
                    EnemyStat zombie = new EnemyStat();
                    zombie.Position = position;
                    zombie.EnemyId = 1;
                    zombie.Health = 20;
                    zombie.Rescitense = 0;
                    zombie.Speed = 2;
                    zombie.Name = "Zombie";
                    zombie.Enemytype = type;
                    return zombie;
                    break;
                case EnemyType.Crippler:
                    EnemyStat crippler = new EnemyStat();
                    crippler.Position = position;
                    crippler.EnemyId = 2;
                    crippler.Health = 30;
                    crippler.Rescitense = 10;
                    crippler.Speed = 1;
                    crippler.Name = "Crippler";
                    crippler.Enemytype = type;
                    return crippler;
                    break;
                case EnemyType.Spitter:
                    EnemyStat spitter = new EnemyStat();
                    spitter.Position = position;
                    spitter.EnemyId = 3;
                    spitter.Health = 25;
                    spitter.Rescitense = 5;
                    spitter.Speed = 2;
                    spitter.Name = "Spitter";
                    spitter.Enemytype = type;
                    return spitter;
                    break;
                case EnemyType.Charger:
                    EnemyStat charger = new EnemyStat();
                    charger.Position = position;
                    charger.EnemyId = 4;
                    charger.Health = 40;
                    charger.Rescitense = 10;
                    charger.Speed = 5;
                    charger.Name = "Charger";
                    charger.Enemytype = type;
                    return charger;
                    break;
                case EnemyType.PistolZombie:
                    EnemyStat pistolzombie = new EnemyStat();
                    pistolzombie.Position = position;
                    pistolzombie.EnemyId = 5;
                    pistolzombie.Health = 35;
                    pistolzombie.Rescitense = 15;
                    pistolzombie.Speed = 2;
                    pistolzombie.Name = "Pistol Zombie";
                    pistolzombie.Enemytype = type;
                    return pistolzombie;
                    break;
                case EnemyType.ShotgunZombie:
                    EnemyStat shotgunzombie = new EnemyStat();
                    shotgunzombie.Position = position;
                    shotgunzombie.EnemyId = 6;
                    shotgunzombie.Health = 35;
                    shotgunzombie.Rescitense = 15;
                    shotgunzombie.Speed = 2;
                    shotgunzombie.Name = "Shotgun Zombie";
                    shotgunzombie.Enemytype = type;
                    return shotgunzombie;
                    break;
            }
            return null;
        }

    }
}