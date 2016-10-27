using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Game2
{
    public class ServiceBus
    {
        public TmxMap Map { get; set; }
        public PathFinder PathFinder { get; set; }
        public Player Player { get; set; }
        public TileEngineGood TileEngineG { get; set; }
    }
}
