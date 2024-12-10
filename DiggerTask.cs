using Digger.Architecture;
using System;

namespace Digger;

public class Wall : ICreature
{
    public string GetImageFileName() => "Wall.png";
}

public class Player : ICreature
{
    public string GetImageFileName() => "Player.png";

    public void Move(int deltaX, int deltaY)
    {
        var (x, y) = FindPlayerPosition();

        var newX = x + deltaX;
        var newY = y + deltaY;

        if (!IsInBounds(newX, newY)) return;

        var target = Game.Map[newX, newY];

        if (target == null || target is EndPoint)
        {
            Game.Map[x, y] = null;
            Game.Map[newX, newY] = this;
            Game.RestoreEndPoints();
        }
        else if (target is Box box)
        {
            var nextX = newX + deltaX;
            var nextY = newY + deltaY;

            if (IsInBounds(nextX, nextY) && (Game.Map[nextX, nextY] == null || Game.Map[nextX, nextY] is EndPoint))
            {
                Game.Map[nextX, nextY] = box;
                Game.Map[newX, newY] = this;
                Game.Map[x, y] = null;
                Game.RestoreEndPoints();
            }
        }
    }

    private (int x, int y) FindPlayerPosition()
    {
        for (int x = 0; x < Game.MapWidth; x++)
            for (int y = 0; y < Game.MapHeight; y++)
                if (Game.Map[x, y] == this)
                    return (x, y);

        throw new InvalidOperationException("Player not found on the map!");
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight;
    }
}

public class Box : ICreature
{
    public string GetImageFileName() => "Box.png";

    public bool IsOnEndPoint()
    {
        var (x, y) = FindPosition();

        return Game.Map[x, y] is EndPoint;
    }

    private (int x, int y) FindPosition()
    {
        for (int x = 0; x < Game.MapWidth; x++)
            for (int y = 0; y < Game.MapHeight; y++)
                if (Game.Map[x, y] == this)
                    return (x, y);

        throw new InvalidOperationException("Box not found on the map!");
    }
}

public class EndPoint : ICreature
{
    public string GetImageFileName() => "EndPoint.png";
}
