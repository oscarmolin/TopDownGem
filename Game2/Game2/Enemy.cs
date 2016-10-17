﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game2
{
    public class EnemyStat
    {
        public int EnemyId { get; set; }
        public int Health { get; set; }
        public int Rescitense { get; set; }
        public int Speed { get; set; }
        public string Name { get; set; }
    }
    enum EnemyType
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
        public EnemyType Type { get; private set; }
        public Enemies(EnemyType type)
        {
            Type = type;
            switch (Type)
            {
                case EnemyType.Zombie:
                    EnemyStat zombie = new EnemyStat();
                    zombie.EnemyId = 1;
                    zombie.Health = 20;
                    zombie.Rescitense = 0;
                    zombie.Speed = 2;
                    zombie.Name = "Zombie";
                    break;
                case EnemyType.Crippler:
                    EnemyStat crippler = new EnemyStat();
                    crippler.EnemyId = 2;
                    crippler.Health = 30;
                    crippler.Rescitense = 10;
                    crippler.Speed = 1;
                    crippler.Name = "Crippler";
                    break;
                case EnemyType.Spitter:
                    EnemyStat spitter = new EnemyStat();
                    spitter.EnemyId = 3;
                    spitter.Health = 25;
                    spitter.Rescitense = 5;
                    spitter.Speed = 2;
                    spitter.Name = "Spitter";
                    break;
                case EnemyType.Charger:
                    EnemyStat charger = new EnemyStat();
                    charger.EnemyId = 4;
                    charger.Health = 40;
                    charger.Rescitense = 10;
                    charger.Speed = 5;
                    charger.Name = "Charger";
                    break;
                case EnemyType.PistolZombie:
                    EnemyStat pistolzombie = new EnemyStat();
                    pistolzombie.EnemyId = 5;
                    pistolzombie.Health = 35;
                    pistolzombie.Rescitense = 15;
                    pistolzombie.Speed = 2;
                    pistolzombie.Name = "Pistol Zombie";
                    break;
                case EnemyType.ShotgunZombie:
                    EnemyStat shotgunzombie = new EnemyStat();
                    shotgunzombie.EnemyId = 6;
                    shotgunzombie.Health = 35;
                    shotgunzombie.Rescitense = 15;
                    shotgunzombie.Speed = 2;
                    shotgunzombie.Name = "Shotgun Zombie";
                    break;
            }

        }
    }
}