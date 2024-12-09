//using Digger.Architecture;
using System;

namespace Digger.Architecture
{
    public static class CreatureMapCreator
    {
        public static ICreature[,] CreateMap(string map)
        {
            // Убираем символы возврата каретки \r
            var lines = map.Replace("\r", "").Trim().Split('\n');
            var width = lines[0].Length;
            var height = lines.Length;
            var creatures = new ICreature[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    creatures[x, y] = CreateCreature(lines[y][x]);
                }
            }

            return creatures;
        }

        private static ICreature? CreateCreature(char symbol)
        {
            return symbol switch
            {
                'W' => new Wall(),
                'P' => new Player(),
                'B' => new Box(),
                'E' => new EndPoint(),
                ' ' => null,
                _ => throw new ArgumentException($"Unknown symbol: {symbol}")
            };
        }
    }
}
