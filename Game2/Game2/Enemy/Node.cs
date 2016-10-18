using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    class Node
    {
        public readonly int X;
        public readonly int Y;

        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }
        public int Sx { get; set; }
        public int Sy { get; set; }
        public Maptype Type { get; set; }
        public bool IsVisited { get; set; }
        public int Time { get; set; }

        public Node(int x, int y, int sx, int sy, int g, int h)
        {
            X = x;
            Y = y;
            Sx = sx;
            Sy = sy;
            G = g;
            H = h;
            F = g + h;
        }
        public void Reset()
        {
            Type = (Type == Maptype.Wall) ? Maptype.Wall : Maptype.Nothing;
            Sx = 0;
            Sy = 0;
            G = 0;
            H = 0;
            F = 0;
            IsVisited = false;
        }
    }
}
//        private int Distance(int x1, int y1, int x2, int y2)
//        {
//            var x = Math.Abs(x1 - x2);
//            var y = Math.Abs(y1 - y2);
//            return (x + y) * 10;
//            //Beräknar Mannhattan-avståndet
//        }
//        private int MoveCost(int x1,int y1,int x2, int y2)
//        {
//            var dx = Math.Abs(x1 - x2);
//            var dy = Math.Abs(y1 - y2);
//            if ((dx + dy) == 2)
//                return 14;
//            if ((dx + dy) == 1)
//                return 10;
//            return 0;
//        }
//        private void MoveFromTo (int x1, int y1, int x2, int y2)
//        {
//            _openNodes.Add(new Node(x1, y1, x1, y1, 0, Distance(x1, y1, x2, y2)));
//            var finished = false;
//            int time = 0;

//            while(!finished && _openNodes.Count > 0)
//            {
//                var BestNode = _openNodes.Aggregate((minNode, nextNode)minNode.F > nextNode.F ? nextNode : minNode);
//                var sx = BestNode.X;
//                var sy = BestNode.Y;

//                //Lägg till punkter runt om och kollar så att de ligger inom kartans gränser
//                for(var y = BestNode.Y - 1; y <= sy + 1 && y >= 0 && y < SizeY; y++)
//                {
//                    for (var x = BestNode.X -1; x <= sx + 1 && x >= 0 && x < SizeX; x++)
//                    {
//                        //strunta i väggar
//                        if (Mapdata[y, x].Type == Maptype.Wall)
//                            continue;

//                        //Finns noden redan i den bearbetande listan?
//                        var closedNode = _closedNodes.FirstOrDefault(n ==> n.X == x && n.Y == y);
//                        if (closedNode != null)
//                        {
//                            //Om noden finns och vi kunde ta oss hit till "bestNode" 
//                            //snabbare så uppdaterar vi bestNode
//                            int tmpG = closedNode.G + MoveCost(sx, sy, x, y);
//                            if (tmpG < BestNode.G)
//                            {
//                                BestNode.Sx = closedNode.X;
//                                BestNode.Sy = closedNode.Y;
//                                BestNode.G = tmpG;
//                                BestNode.F = BestNode.G + BestNode.H;
//                            }
//                        }
//                        else if (!_openNodes.Any(n => n.X == x && n.Y == y))
//                        {
//                            //Om noden inte finns varken i open- eller closelistan så lägger vi 
//                            //till den med uppdaterad info
//                            Mapdata[y, x] = new Node(x, y, sx, sy, BestNode.G + MoveCost(x, y, sx, sy), Distance(x, y, x2, y2)) { IsVisited = true, Time == time++ };
//                            _openNodes.Add(Mapdata[y, x]);
//                        }
//                    }
//                }
//                //Flytta över punkten till den stängda listan
//                _closeNode.Add(BestNode);

//                //Var detta målet?
//                if (BestNode.H == 0)
//                    finished = true;

//                //Ta bort den från den öppna listan
//                _openNodes.Remove(BestNode);
//            }
//        }
//        public void Plotroute()
//        {
//            if (_startX == -1 || _startY == -1 || _goalX == -1 || _goalY == -1)
//                return;

//            var node = _closeNodes.Last();

//            while (node.G != 0)
//            {
//                //Hitta föregående nod
//                for (var i = 0; i < _closeNodes.Count; i++)
//                {
//                    if (_closeNodes[i].X == node.Sx && _closeNodes[i].Y == node.Sx)
//                    {
//                        node = _closeNodes[i];
//                        break;
//                    }
//                }
//                if (node.G != 0)
//                    Mapdata[node.Y, node.X].Type = MapType.Route;
//            }
//            Mapdata[_startY, _startX].Type = MapType.Start;
//            Mapdata[_goalY, _goalX].Type = MapType.Goal;
//        }
//    }
//}
