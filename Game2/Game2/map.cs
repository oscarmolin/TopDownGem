﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class Map
    {
        public Node[,] MapData { get; private set; }
        public int SizeX { get { return MapData.GetLength(1); } }
        public int SizeY { get { return MapData.GetLength(0); } }
        private readonly List<Node> _openNodes = new List<Node>();
        private readonly List<Node> _closeNodes = new List<Node>();

        private int _startX = -1;
        private int _startY = -1;
        private int _playerX = -1;
        private int _playerY = -1;

        public Map(int[,] data)
        {
            MapData = new Node[data.GetLength(1), data.GetLength(0)];

            for (var y=0; y< SizeY; y++)
            {
                for(var x=0;x< SizeX; x++)
                {
                    MapData[y, x] = new Node(x, y, 0, 0, 0, 0);
                    MapData[y, x].Type = data[y, x] == 1 ? Maptype.Wall : Maptype.Nothing; 
                }
            }
        }
        public void MoveFromTo()
        {
            if (_startX == -1 || _startY == -1 || _playerX == -1 || _playerY == -1)
                return;

            _openNodes.Clear();
            _closeNodes.Clear();
            ResetMap();

            MoveFromTo(_startX, _startY, _playerX, _playerY);
        }
        public void Plotroute()
        {
            if (_startX == -1 || _startY == -1 || _playerX == -1 || _playerY == -1)
                return;

            var node = _closeNodes.Last();

            while (node.G != 0)
            {
                //Hitta föregående nod
                for (var i = 0; i < _closeNodes.Count; i++)
                {
                    if (_closeNodes[i].X == node.Sx && _closeNodes[i].Y == node.Sx)
                    {
                        node = _closeNodes[i];
                        break;
                    }
                }
                if (node.G != 0)
                    MapData[node.Y, node.X].Type = Maptype.Route;
            }
            MapData[_startY, _startX].Type = Maptype.Go;
            MapData[_playerY, _playerX].Type = Maptype.Attack;
        }
        public void SetWall(int x, int y)
        {
            if (x < 0 || y < 0 || x >= SizeX || y >= SizeY)
                return;

            MapData[y, x].Type = Maptype.Wall;
        }
        public void NoWall(int x, int y)
        {
            if (x < 0 || y < 0 || x >= SizeX || y >= SizeY)
                return;

            MapData[y, x].Type = Maptype.Nothing;
        }
        public bool SetStart(int x, int y)
        {
            if (x < 0 || y < 0 || x >= SizeX || y >= SizeY)
                return false;
            
            if(MapData[y, x].Type != Maptype.Wall)
            {
                NoWall(_startX, _startY);
                _startX = x;
                _startY = y;
                MapData[y, x].Type = Maptype.Go;
                return true;
            }
            return false;
        }
        public bool SetPlayer(int x, int y)
        {
            if (x < 0 || y < 0 || x >= SizeX || y >= SizeY)
                return false;

            if(MapData[y,x].Type != Maptype.Wall)
            {
                NoWall(_playerX, _playerY);
                _startX = x;
                _startY = y;
                MapData[y, x].Type = Maptype.Attack;
                return true;
            }
            return false;
        }

        #region Private Methods

        private int Distance(int x1, int y1, int x2, int y2)
        {
            var x = Math.Abs(x1 - x2);
            var y = Math.Abs(y1 - y2);
            return (x + y) * 10;
            //Beräknar Mannhattan-avståndet
        }
        private int MoveCost(int x1, int y1, int x2, int y2)
        {
            var dx = Math.Abs(x1 - x2);
            var dy = Math.Abs(y1 - y2);
            if ((dx + dy) == 2)
                return 14;
            if ((dx + dy) == 1)
                return 10;
            return 0;
        }
        private void ResetMap()
        {
            foreach (var Node in MapData)
            {
                Node.Reset();
            }
        }

        private void MoveFromTo(int x1,int y1,int x2,int y2)
        {
            _openNodes.Add(new Node(x1, y1, x1, y1, 0, Distance(x1, y1, x2, y2)));
            var finished = false;
            int time = 0;

            while (!finished && _openNodes.Count > 0)
            {
                var bestNode = _openNodes.Aggregate((minNode, nextNode) => minNode.F > nextNode.F ? nextNode : minNode);
                var sx = bestNode.X;
                var sy = bestNode.Y;

                for (var y = bestNode.Y - 1; y <= sy + 1 && y >= 0 && y < SizeY; y++)
                {
                    for (var x = bestNode.X - 1; x <= sx + 1 && x >= 0 && x < SizeX; x++)
                    {
                        if (MapData[y, x].Type == Maptype.Wall)
                            continue;

                        var closedNode = _closeNodes.FirstOrDefault(n => n.X == x && n.Y == y);
                        if (closedNode != null)
                        {
                            int tmpG = closedNode.G + MoveCost(sx, sy, x, y);
                            if (tmpG < bestNode.G)
                            {
                                bestNode.Sx = closedNode.X;
                                bestNode.Sy = closedNode.Y;
                                bestNode.G = tmpG;
                                bestNode.F = bestNode.G + bestNode.H;
                            }
                        }
                        else if (!_openNodes.Any(n => n.X == x && n.Y == y))
                        {
                            MapData[y, x] = new Node(x, y, sx, sy, bestNode.G + MoveCost(x, y, sx, sy), Distance(x, y, x2, y2)) { IsVisited = true, Time = time++ };
                            _openNodes.Add(MapData[y, x]);
                        }
                    }
                }

                _closeNodes.Add(bestNode);

                if (bestNode.H == 0)
                    finished = true;

                _openNodes.Remove(bestNode);
            }
        }
        #endregion
    }
}
