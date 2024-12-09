using Digger.Architecture;
using System;
using System.Collections.Generic;
using System.Net;

namespace Digger
{
    public static class Game
    {
        // Карта уровня
        private const string mapWithPlayerBoxEndPoint = @"
WWWWW
W P W
W B W
W E W
WWWWW";

        public static ICreature[,] Map { get; set; }
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);

        // Список координат EndPoints
        public static List<(int x, int y)> EndPoints { get; private set; } = new();

        public static void CreateMap()
        {
            Map = CreatureMapCreator.CreateMap(mapWithPlayerBoxEndPoint);

            // Определяем позиции EndPoint
            EndPoints.Clear();
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    if (Map[x, y] is EndPoint)
                    {
                        EndPoints.Add((x, y));
                    }
                }
            }

            RestoreEndPoints();
        }

        // Метод для восстановления EndPoints после перемещений
        public static void RestoreEndPoints()
        {
            foreach (var (x, y) in EndPoints)
            {
                if (Map[x, y] == null) // Только если клетка пустая
                {
                    Map[x, y] = new EndPoint();
                }
            }
        }
    }
}
